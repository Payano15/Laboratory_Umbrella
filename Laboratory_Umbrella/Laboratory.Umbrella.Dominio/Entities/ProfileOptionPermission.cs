using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Laboratory.Umbrella.Dominio.Common.Constants;

namespace Laboratory.Umbrella.Dominio.Entities;

public class ProfileOptionPermission : IEntity
{
    [Key]
    [MaxLength(36)]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [MaxLength(36)]
    public string ProfileId { get; set; } = string.Empty;

    public Profile Profile { get; set; } = null!;

    [Required]
    [MaxLength(36)]
    public string OptionId { get; set; } = string.Empty;

    public Opciones Option { get; set; } = null!;

    [Required]
    public int PermissionId { get; set; } 

    public Permission Permission { get; set; }

    public bool Granted { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? AnulledAt { get; set; }
}
