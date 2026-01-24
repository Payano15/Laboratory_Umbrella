using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Dominio.Entities;

public class Usuarios : IEntity
{
    [Key]
    [MaxLength(36)]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [MaxLength(200)]
    public string fullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [MaxLength(300)]
    public string PasswordHash { get; set; } = string.Empty;
    
    [Required]
    public int Status { get; set; }

    [MaxLength(200)]
    public string hashSalt { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Required]
    public string UserCreated { get; set; } = string.Empty;

    [Required]
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;

    [Required]
    public string UserUpdated { get; set; } = string.Empty;

    [Required]
    public DateTime LastLogin { get; set; } = DateTime.Now;

    public ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
    public DateTime? AnulledAt { get; set; }
    public string UserAnulled { get; set; } = string.Empty;
}
