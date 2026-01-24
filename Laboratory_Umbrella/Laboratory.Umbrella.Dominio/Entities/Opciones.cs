using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Laboratory.Umbrella.Dominio.Entities;

public class Opciones : IEntity
{
    [Key]
    [MaxLength(36)]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Url { get; set; }

    [Required]
    [MaxLength(36)]
    public string SectionId { get; set; } = string.Empty;

    public Section Section { get; set; }

    [Required]
    public int Order { get; set; }

    [Required]
    public bool Visible { get; set; }

    [Required]
    public int Status { get; set; }

    public ICollection<ProfileOptionPermission> ProfileOptionPermissions { get; set; }
        = new List<ProfileOptionPermission>();

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? AnulledAt { get; set; }
}
