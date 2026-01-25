using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Entities;
using Laboratory.Umbrella.Dominio.Response;
using Laboratory.Umbrella.Services.Interfaces;

namespace Laboratory.Umbrella.Services.Services.Autentications;

public class TokenService : BaseService, ITokenService
{
    private readonly IRepository<UserAuthentication> _userAuth;
    private readonly IRepository<Usuarios> _users;
    private readonly IRepository<UserProfile> _userProfiles;
    private readonly IRepository<Profile> _profiles;
    private readonly IRepository<Opciones> _opciones;
    private readonly IRepository<Secciones> _secciones;
    private readonly IRepository<ProfileOptionPermission> _profilePermissions;

    public TokenService(
        IRepository<UserAuthentication> userAuth,
        IRepository<Usuarios> users,
        IRepository<UserProfile> userProfiles,
        IRepository<Profile> profiles,
        IRepository<Opciones> opciones,
        IRepository<Secciones> secciones,
        IRepository<ProfileOptionPermission> profilePermissions)
    {
        _userAuth = userAuth;
        _users = users;
        _userProfiles = userProfiles;
        _profiles = profiles;
        _opciones = opciones;
        _secciones = secciones;
        _profilePermissions = profilePermissions;
    }

    public async Task<AuthTokenResponse> ValidateToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return FailedToken();

        var auth = (await _userAuth.FindAsync(t => t.Token == token && t.DateExpired > DateTime.Now))
            .FirstOrDefault();

        if (auth is null)
            return FailedToken();

        var user = (await _users.FindAsync(u => u.UserName == auth.UserName &&
                                               u.Status == (int)GeneralStatus.GlobalStatus.Status.Activo))
            .FirstOrDefault();

        if (user is null)
            return FailedToken();

        var accessData = await LoadAccessData(user.Id);

        return new AuthTokenResponse
        {
            IsLoggedIn = true,
            DateExpired = DateToString(auth.DateExpired),
            Token = auth.Token,
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

    private AuthTokenResponse FailedToken() => new();

    private async Task<(List<Profile> Profiles, List<Opciones> Opciones, List<Secciones> Secciones)> LoadAccessData(string userId)
    {
        var userProfiles = (await _userProfiles.FindAsync(up => up.UserId == userId)).ToList();
        var profileIds = userProfiles.Select(up => up.ProfileId).Distinct().ToList();

        var profiles = (await _profiles
            .FindAsync(p => profileIds.Contains(p.Id) && p.Status == (int)GeneralStatus.GlobalStatus.Status.Activo))
            .ToList();

        var profilePermissions = (await _profilePermissions
            .FindAsync(pop => profileIds.Contains(pop.ProfileId) && pop.Granted))
            .ToList();

        var optionIds = profilePermissions.Select(pop => pop.OptionId).Distinct().ToList();
        var opciones = (await _opciones
            .FindAsync(o => optionIds.Contains(o.Id) && o.Status == (int)GeneralStatus.GlobalStatus.Status.Activo))
            .ToList();

        var seccionIds = opciones.Select(o => o.SectionId).Distinct().ToList();
        var secciones = (await _secciones
            .FindAsync(s => seccionIds.Contains(s.Id) && s.Status == (int)GeneralStatus.GlobalStatus.Status.Activo))
            .ToList();

        return (profiles, opciones, secciones);
    }
}
