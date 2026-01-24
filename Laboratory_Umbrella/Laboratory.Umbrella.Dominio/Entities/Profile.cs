using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Dominio.Entities;

public class Profile : IEntity
{
    [Key]
    [MaxLength(36)]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public int Status { get; set; }

    public ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();

    public ICollection<ProfileOptionPermission> OptionPermissions { get; set; }
        = new List<ProfileOptionPermission>();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? AnulledAt { get; set; }
}
