using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Dominio.Response;

public class UsuariosResponse
{
    public string Id { get; set; } = string.Empty;
    public string fullName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime LastLogin { get; set; }
    public AuditResponse Audit { get; set; } = new();
    public List<UserProfileResponse> Profiles { get; set; } = new();
    public List<UserPermissionResponse> Permissions { get; set; } = new();

    public class UserProfileResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class UserPermissionResponse
    {
        public string ProfileId { get; set; } = string.Empty;
        public string OptionId { get; set; } = string.Empty;
        public string OptionDescription { get; set; } = string.Empty;
        public string OptionType { get; set; } = string.Empty;
        public string? OptionUrl { get; set; }
        public int OptionOrder { get; set; }
        public string SectionId { get; set; } = string.Empty;
        public string SectionDescription { get; set; } = string.Empty;
        public int PermissionId { get; set; }
        public bool Granted { get; set; }
    }
}
