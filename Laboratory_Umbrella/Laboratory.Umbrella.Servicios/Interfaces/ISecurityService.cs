using Laboratory.Umbrella.Dominio.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Services.Interfaces;

public interface ISecurityService
{
    Task<AuthTokenResponse> ValidateToken(string token);
}
