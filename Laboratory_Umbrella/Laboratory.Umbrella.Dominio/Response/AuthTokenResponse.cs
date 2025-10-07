using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Dominio.Response;

public record AuthTokenResponse
{
    public string Token { get; set; } = string.Empty;
}
