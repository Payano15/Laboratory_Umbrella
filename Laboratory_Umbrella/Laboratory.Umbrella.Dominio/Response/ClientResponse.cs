using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Dominio.Response;

public record ClientResponse
{
    public string Id { get; set; } = null!;
    public string fullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string gender { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string bornDate { get; set; } = string.Empty;
    public string TypeClient { get; set; } = string.Empty;
    public decimal CreditLimit { get; set; } = decimal.Zero;
    public decimal discount { get; set; } = decimal.Zero;
    public AuditResponse Audit  { get; set; } = new();
}
