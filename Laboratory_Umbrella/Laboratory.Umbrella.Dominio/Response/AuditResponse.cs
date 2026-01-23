using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Dominio.Response;

public record AuditResponse
{
    public string UserCreated { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
    public string? UserUpdated { get; set; } = string.Empty;
    public string? UpdatedAt { get; set; } = string.Empty;
    public string? UserAnulled { get; set; } = string.Empty;
    public string? AnulledAt { get; set; } = string.Empty;
}
