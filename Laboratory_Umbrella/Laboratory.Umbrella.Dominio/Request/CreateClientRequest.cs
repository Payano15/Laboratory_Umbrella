using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Dominio.Request;

public record CreateClientRequest
{
    public string Id { get; set; } = null!;
    public string fullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string gender { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Status { get; set; }
    public decimal CreditLimit { get; set; } = decimal.Zero;
    public decimal discount { get; set; } = decimal.Zero;
    public DateTime bornDate { get; set; }
    
}
