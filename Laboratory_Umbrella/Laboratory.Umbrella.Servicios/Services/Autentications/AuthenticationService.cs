using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Entities;
using Laboratory.Umbrella.Dominio.Exception;
using Laboratory.Umbrella.Dominio.Helpers;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;
using Laboratory.Umbrella.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Services.Services.Autentications;

public class AuthenticationService: BaseService, IAuthenticationService
{
    #region Properties
    private readonly IRepository<UserAuthentication> _dbUserAutentication;
    private readonly IRepository<Usuarios> _dbUser;
    private readonly IRepository<UserProfile> _dbUserProfile;
    private readonly IRepository<Profile> _dbProfile;
    private readonly IRepository<Opciones> _dbOpciones;
    private readonly IRepository<Secciones> _dbSecciones;
    private readonly IRepository<ProfileOptionPermission> _dbProfileOptions;
    #endregion

    #region Constructor
    public AuthenticationService(IRepository<UserAuthentication> dbUserAutentication,
                                 IRepository<Usuarios> dbUser,
                                 IRepository<UserProfile> dbUserProfile,
                                 IRepository<Profile> dbProfile,
                                 IRepository<Opciones> dbOpciones,
                                 IRepository<Secciones> dbSecciones,
                                 IRepository<ProfileOptionPermission> dbProfileOptions)
    {
        _dbUserAutentication = dbUserAutentication;
        _dbUser = dbUser;
        _dbUserProfile = dbUserProfile;
        _dbProfile = dbProfile;
        _dbOpciones = dbOpciones;
        _dbSecciones = dbSecciones;
        _dbProfileOptions = dbProfileOptions;
    }
    #endregion

    #region Methods
    public async Task<LoginResponse> Login(LoginRequest request)
    {
        var user = await ValidateUserCredentials(request);

        await ExpireValidToken(request);

        var userAuth = await CreateUserAuthentication(user.UserName);

        var accessData = await LoadAccessData(user.Id);

        return BuildLoginResponse(user, userAuth, accessData);
    }
    #endregion

    #region Auxiliary Methods
    private async Task<Usuarios> ValidateUserCredentials(LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            throw new CustomException(Constants.Code.Error.InvalidUserCredentials, Constants.Message.Error.InvalidUserCredentials);

        var user = (await _dbUser.FindAsync(u => u.UserName == request.Username)).FirstOrDefault();

        if (user is null || user.Status != (int)GeneralStatus.GlobalStatus.Status.Activo)
            throw new CustomException(Constants.Code.Error.InvalidUserCredentials, Constants.Message.Error.InvalidUserCredentials);

        var hashPassword = SecureChecksumHelper.ComputeHMACSHA256(request.Password, user.hashSalt);

        if (!SecureChecksumHelper.Validate(hashPassword, user.PasswordHash))
            throw new CustomException(Constants.Code.Error.InvalidUserCredentials, Constants.Message.Error.InvalidUserCredentials);

        return user;
    }

    private async Task<UserAuthentication> CreateUserAuthentication(string userName)
    {
        var token = Guid.NewGuid().ToString();
        var now = DateTime.UtcNow;

        var userAuth = new UserAuthentication
        {
            UserName = userName,
            Token = token,
            DateIssue = now,
            DateExpired = now.AddDays(1),
            CreatedAt = now
        };

        await _dbUserAutentication.AddAsync(userAuth);

        return userAuth;
    }

    private async Task<(List<Profile> Profiles, List<Opciones> Opciones, List<Secciones> Secciones)> LoadAccessData(string userId)
    {
        var userProfiles = (await _dbUserProfile.FindAsync(up => up.UserId == userId)).ToList();
        var profileIds = userProfiles.Select(up => up.ProfileId).Distinct().ToList();

        var profiles = (await _dbProfile
            .FindAsync(p => profileIds.Contains(p.Id) && p.Status == (int)GeneralStatus.GlobalStatus.Status.Activo))
            .ToList();

        var profilePermissions = (await _dbProfileOptions
            .FindAsync(pop => profileIds.Contains(pop.ProfileId) && pop.Granted))
            .ToList();

        var optionIds = profilePermissions.Select(pop => pop.OptionId).Distinct().ToList();
        var opciones = (await _dbOpciones
            .FindAsync(o => optionIds.Contains(o.Id) && o.Status == (int)GeneralStatus.GlobalStatus.Status.Activo))
            .ToList();

        var seccionIds = opciones.Select(o => o.SectionId).Distinct().ToList();
        var secciones = (await _dbSecciones
            .FindAsync(s => seccionIds.Contains(s.Id) && s.Status == (int)GeneralStatus.GlobalStatus.Status.Activo))
            .ToList();

        return (profiles, opciones, secciones);
    }

    private LoginResponse BuildLoginResponse(Usuarios user, UserAuthentication userAuth, (List<Profile> Profiles, List<Opciones> Opciones, List<Secciones> Secciones) accessData)
    {
        return new LoginResponse
        {
            IsLoggedIn = true,
            DateExpired = DateToString(userAuth.DateExpired),
            Token = userAuth.Token,
            UserName = user.UserName,
            FullName = user.fullName,
            RoleName = accessData.Profiles.Select(p => p.Description).Distinct().ToList(),
            AllowSection = accessData.Secciones.Select(s => new SeccionesResponse
            {
                Id = s.Id,
                Description = s.Description,
                Icon = s.Icon,
                Order = s.Order,
                Status = s.Status
            }).ToList(),
            Permissions = accessData.Opciones.Select(o => o.Id).Distinct().ToList()
        };
    }

    private async Task ExpireValidToken(LoginRequest authRequest)
    {
        var now = DateTime.UtcNow;

        var tokens = (await _dbUserAutentication
            .FindAsync(t => t.UserName == authRequest.Username))
            .ToList();

        foreach (var token in tokens)
        {
            if (token.DateExpired > now)
            {
                token.DateExpired = now;
                await _dbUserAutentication.UpdateAsync(token);
            }
        }
    }
    #endregion
}
