using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Dominio.Entities;

public class Cliente: IEntity
{
    public string Id { get; set; } = null!;
    public string fullName { get; set; }= string.Empty;
    public string Email { get; set; }= string.Empty;
    public string Phone { get; set; }= string.Empty;
    public string gender { get; set; }= string.Empty;
    public string Address { get; set; }= string.Empty;
    public int Status { get; set; }
    public DateTime bornDate { get; set; }
    public string UserCreated { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? UserUpdated { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string? UserAnulled { get; set; } = string.Empty;
    public DateTime? AnulledAt { get; set; } 

}
