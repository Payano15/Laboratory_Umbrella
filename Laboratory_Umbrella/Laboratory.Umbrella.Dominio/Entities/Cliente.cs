using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Laboratory.Umbrella.Dominio.Entities;

public class Cliente : IEntity
{
    [Key]
    [MaxLength(36)]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [MaxLength(200)]
    public string fullName { get; set; } = string.Empty;

    [EmailAddress]
    [MaxLength(255)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(10)]
    public string? gender { get; set; }

    [MaxLength(500)]
    public string? Address { get; set; }

    public int Status { get; set; }

    public DateTime bornDate { get; set; }

    [MaxLength(100)]
    public string? UserCreated { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [MaxLength(100)]
    public string? UserUpdated { get; set; }

    public DateTime? UpdatedAt { get; set; }

    [MaxLength(100)]
    public string? UserAnulled { get; set; }

    public DateTime? AnulledAt { get; set; }
}
