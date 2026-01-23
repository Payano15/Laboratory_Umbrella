using Laboratory.Umbrella.Dominio.Response;
using Laboratory.Umbrella.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Services.Services;

public class SecurityService : ISecurityService
{
    public async Task<AuthTokenResponse> ValidateToken(string Token)
    {
        //return await _tokenService.ValidateToken(Token);
        Token = "ValidToken";
        return await Task.FromResult(new AuthTokenResponse { Token = Token });
    }
}
