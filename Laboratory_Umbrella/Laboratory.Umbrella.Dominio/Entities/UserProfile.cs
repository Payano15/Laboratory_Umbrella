using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Laboratory.Umbrella.Dominio.Entities;

public class UserProfile : IEntity
{
    [Key]
    [MaxLength(36)]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [MaxLength(36)]
    public string UserId { get; set; } = string.Empty;

    public Usuarios User { get; set; } = null!;

    [Required]
    [MaxLength(36)]
    public string ProfileId { get; set; } = string.Empty;

    public Profile Profile { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? AnulledAt { get; set; }
}
