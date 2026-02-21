using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Entities;
using Laboratory.Umbrella.Dominio.Exception;
using Laboratory.Umbrella.Dominio.Helpers;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;
using Laboratory.Umbrella.Services.Interfaces;
using System.Linq.Expressions;

namespace Laboratory.Umbrella.Services.Services;

public class UserService : BaseService, IUserService
{
    #region properties
    private readonly IRepository<Usuarios> _dbUser;
    private readonly IRepository<UserProfile> _dbUserProfile;
    private readonly IRepository<Profile> _dbProfile;
    private readonly IRepository<Opciones> _dbOpciones;
    private readonly IRepository<Secciones> _dbSecciones;
    private readonly IRepository<ProfileOptionPermission> _dbProfileOptions;
    #endregion

    #region constructor
    public UserService(IRepository<Usuarios> dbUser,
                       IRepository<UserProfile> dbUserProfile,
                       IRepository<Profile> dbProfile,
                       IRepository<Opciones> dbOpciones,
                       IRepository<Secciones> dbSecciones,
                       IRepository<ProfileOptionPermission> dbProfileOptions)
    {
        _dbUser = dbUser;
        _dbUserProfile = dbUserProfile;
        _dbProfile = dbProfile;
        _dbOpciones = dbOpciones;
        _dbSecciones = dbSecciones;
        _dbProfileOptions = dbProfileOptions;
    }
    #endregion

    #region methods
    public async Task<MetaDataResponse<List<UsuariosResponse>>> GetByParameters(UserByParameters request)
    {
        var (users, totalRecords) = await GetUsersByParametersAsync(request);
        var userIds = users.Select(u => u.Id).ToList();
        var userProfiles = await GetUserProfilesByUserIds(userIds);
        var profiles = await GetProfilesByIds(userProfiles.Select(p => p.ProfileId).Distinct().ToList());
        var profilePermissions = await GetProfilePermissionsByProfileIds(profiles.Select(p => p.Id).Distinct().ToList());
        var options = await GetOptionsByIds(profilePermissions.Select(p => p.OptionId).Distinct().ToList());
        var sections = await GetSectionsByIds(options.Select(o => o.SectionId).Distinct().ToList());

        var userResponses = users.Select(u => new UsuariosResponse
        {
            Id = u.Id.ToString(),
            fullName = u.fullName,
            UserName = u.UserName,
            Status = Enum.GetName(typeof(GeneralStatus.ClientStatus.StatusClient), u.Status) ?? string.Empty,
            LastLogin = u.LastLogin,
            Audit = new AuditResponse
            {
                CreatedAt = DateToString(u.CreatedAt),
                UserCreated = u.UserCreated,
                UpdatedAt = DateToString(u.UpdatedAt),
                UserUpdated = u.UserUpdated,
                AnulledAt = DateToString(u.AnulledAt),
                UserAnulled = u.UserAnulled
            },
            Profiles = BuildProfiles(u.Id, userProfiles, profiles),
            Permissions = BuildPermissions(u.Id, userProfiles, profiles, profilePermissions, options, sections)
        }).ToList();

        return new(userResponses, BuildMeta(request.Page, request.PageSize, totalRecords));
    }

    public async Task<MetaDataResponse<UsuariosResponse>> GetById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new CustomException(Constants.Code.Error.UserNoEncontrado, Constants.Message.Error.UserNoEncontrado);

        var user = await _dbUser.GetByIdAsync(id);

        if (user == null)
            throw new CustomException(Constants.Code.Error.UserNoEncontrado, Constants.Message.Error.UserNoEncontrado);

        var userProfiles = await GetUserProfilesByUserIds([user.Id]);
        var profiles = await GetProfilesByIds(userProfiles.Select(p => p.ProfileId).Distinct().ToList());
        var profilePermissions = await GetProfilePermissionsByProfileIds(profiles.Select(p => p.Id).Distinct().ToList());
        var options = await GetOptionsByIds(profilePermissions.Select(p => p.OptionId).Distinct().ToList());
        var sections = await GetSectionsByIds(options.Select(o => o.SectionId).Distinct().ToList());

        var userResponse = new UsuariosResponse
        {
            Id = user.Id,
            fullName = user.fullName,
            UserName = user.UserName,
            Status = Enum.GetName(typeof(GeneralStatus.ClientStatus.StatusClient), user.Status) ?? string.Empty,
            LastLogin = user.LastLogin,
            Audit = new AuditResponse
            {
                CreatedAt = DateToString(user.CreatedAt),
                UserCreated = user.UserCreated,
                UpdatedAt = DateToString(user.UpdatedAt),
                UserUpdated = user.UserUpdated,
                AnulledAt = DateToString(user.AnulledAt),
                UserAnulled = user.UserAnulled
            },
            Profiles = BuildProfiles(user.Id, userProfiles, profiles),
            Permissions = BuildPermissions(user.Id, userProfiles, profiles, profilePermissions, options, sections)
        };

        return new(userResponse);
    }

    public async Task<MetaDataResponse<bool>> Save(SaveUserRequest request)
    {
        var isCreating = string.IsNullOrWhiteSpace(request.Id);

        return isCreating
            ? await CreateUser(request)
            : await UpdateUser(request);
    }

    public async Task<MetaDataResponse<bool>> ChangePassword(string id, ChangePasswordRequest request)
    {
        if (string.IsNullOrWhiteSpace(id))
            return new(false);

        var user = await _dbUser.GetByIdAsync(id);
        if (user == null)
            return new(false);

        var newSalt = Guid.NewGuid().ToString();
        var newHash = SecureChecksumHelper.ComputeHMACSHA256(request.NewPassword, newSalt);

        user.hashSalt = newSalt;
        user.PasswordHash = newHash;
        user.UpdatedAt = DateTime.Now;
        user.UserUpdated = UserLogged;

        await _dbUser.UpdateAsync(user);

        return new(true);
    }

    public async Task<MetaDataResponse<bool>> ValidatePassword(ValidatePasswordRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.PasswordHash))
            return new(false);

        var user = await _dbUser.GetByIdAsync(request.UserId);
        if (user == null)
            return new(false);

        var isValid = SecureChecksumHelper.Validate(request.PasswordHash, user.PasswordHash);

        return new(isValid);
    }
    #endregion

    #region auxiliary methods
    private MetaResponse BuildMeta(int page, int pageSize, long totalCount)
    {
        return new MetaResponse
        {
            TotalCount = (int)totalCount,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            HasNextPage = HasNextPage(page, pageSize, totalCount),
            HasPreviousPage = page > 1
        };
    }
    private async Task<(List<Usuarios> Users, int TotalRecords)> GetUsersByParametersAsync(UserByParameters parameters)
    {
        Expression<Func<Usuarios, bool>> filter = u =>
            (string.IsNullOrEmpty(parameters.FullName) || u.fullName.Contains(parameters.FullName)) &&
            (string.IsNullOrEmpty(parameters.UserName) || u.UserName.Contains(parameters.UserName));

        var pagedResult = await _dbUser.GetPagedAsync(
            parameters.Page,
            parameters.PageSize,
            filter,
            u => u.fullName,
            ascending: true);

        return (pagedResult.Items, pagedResult.TotalCount);
    }

    private async Task<MetaDataResponse<bool>> CreateUser(SaveUserRequest request)
    {
        var userId = Guid.NewGuid().ToString();
        var salt = Guid.NewGuid().ToString();
        var hash = SecureChecksumHelper.ComputeHMACSHA256(request.Password, salt);

        var newUser = new Usuarios
        {
            Id = userId,
            fullName = request.FullName,
            UserName = request.UserName,
            PasswordHash = hash,
            hashSalt = salt,
            Status = request.Status,
            CreatedAt = DateTime.Now,
            UserCreated = UserLogged
        };

        await _dbUser.AddAsync(newUser);

        await ReplaceUserProfiles(userId, request.UserProfileIds);

        return new(true);
    }

    private async Task<MetaDataResponse<bool>> UpdateUser(SaveUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Id))
            return new(false, null);

        var user = await _dbUser.GetByIdAsync(request.Id);
        if (user == null)
            return new(false, null);

        user.fullName = request.FullName;
        user.UserName = request.UserName;
        user.Status = request.Status;
        user.UpdatedAt = DateTime.Now;
        user.UserUpdated = UserLogged;

        await _dbUser.UpdateAsync(user);

        await ReplaceUserProfiles(user.Id, request.UserProfileIds);

        return new(true);
    }

    private async Task ReplaceUserProfiles(string userId, List<string> profileIds)
    {
        var existingProfiles = await _dbUserProfile.FindAsync(p => p.UserId == userId);
        var existingIds = existingProfiles.Select(p => p.Id).ToList();
        if (existingIds.Count > 0)
        {
            await _dbUserProfile.DeleteRangeAsync(existingIds);
        }

        if (profileIds == null || profileIds.Count == 0)
            return;

        var newProfiles = profileIds
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .Select(id => new UserProfile
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                ProfileId = id,
                CreatedAt = DateTime.Now
            })
            .ToList();

        if (newProfiles.Count > 0)
        {
            await _dbUserProfile.AddRangeAsync(newProfiles);
        }
    }
    private async Task<List<UserProfile>> GetUserProfilesByUserIds(List<string> userIds)
    {
        if (userIds == null || userIds.Count == 0)
            return [];

        return await _dbUserProfile.FindAsync(p => userIds.Contains(p.UserId));
    }

    private async Task<List<Profile>> GetProfilesByIds(List<string> profileIds)
    {
        if (profileIds == null || profileIds.Count == 0)
            return [];

        return await _dbProfile.FindAsync(p => profileIds.Contains(p.Id));
    }

    private async Task<List<ProfileOptionPermission>> GetProfilePermissionsByProfileIds(List<string> profileIds)
    {
        if (profileIds == null || profileIds.Count == 0)
            return [];

        return await _dbProfileOptions.FindAsync(p => profileIds.Contains(p.ProfileId));
    }

    private async Task<List<Opciones>> GetOptionsByIds(List<string> optionIds)
    {
        if (optionIds == null || optionIds.Count == 0)
            return [];

        return await _dbOpciones.FindAsync(o => optionIds.Contains(o.Id));
    }

    private async Task<List<Secciones>> GetSectionsByIds(List<string> sectionIds)
    {
        if (sectionIds == null || sectionIds.Count == 0)
            return [];

        return await _dbSecciones.FindAsync(s => sectionIds.Contains(s.Id));
    }

    private List<UsuariosResponse.UserProfileResponse> BuildProfiles(
        string userId,
        List<UserProfile> userProfiles,
        List<Profile> profiles)
    {
        var userProfileIds = userProfiles
            .Where(p => p.UserId == userId)
            .Select(p => p.ProfileId)
            .Distinct()
            .ToList();

        return profiles
            .Where(p => userProfileIds.Contains(p.Id))
            .Select(p => new UsuariosResponse.UserProfileResponse
            {
                Id = p.Id,
                Description = p.Description
            })
            .ToList();
    }

    private List<UsuariosResponse.UserPermissionResponse> BuildPermissions(
        string userId,
        List<UserProfile> userProfiles,
        List<Profile> profiles,
        List<ProfileOptionPermission> profilePermissions,
        List<Opciones> options,
        List<Secciones> sections)
    {
        var userProfileIds = userProfiles
            .Where(p => p.UserId == userId)
            .Select(p => p.ProfileId)
            .Distinct()
            .ToList();

        var permissions = profilePermissions
            .Where(pp => userProfileIds.Contains(pp.ProfileId))
            .ToList();

        var optionById = options.ToDictionary(o => o.Id, o => o);
        var sectionById = sections.ToDictionary(s => s.Id, s => s);

        return permissions
            .Select(pp =>
            {
                optionById.TryGetValue(pp.OptionId, out var option);
                var sectionId = option?.SectionId ?? string.Empty;
                sectionById.TryGetValue(sectionId, out var section);

                return new UsuariosResponse.UserPermissionResponse
                {
                    ProfileId = pp.ProfileId,
                    OptionId = pp.OptionId,
                    OptionDescription = option?.Description ?? string.Empty,
                    OptionType = option?.Type ?? string.Empty,
                    OptionUrl = option?.Url,
                    OptionOrder = option?.Order ?? 0,
                    SectionId = section?.Id ?? string.Empty,
                    SectionDescription = section?.Description ?? string.Empty,
                    PermissionId = pp.PermissionId,
                    Granted = pp.Granted
                };
            })
            .ToList();
    }
    #endregion
}