using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Laboratory.Umbrella.Dominio.Response;

namespace Laboratory.Umbrella.Services.Interfaces;

public interface ITokenService
{
    Task<AuthTokenResponse> ValidateToken(string token);
}
