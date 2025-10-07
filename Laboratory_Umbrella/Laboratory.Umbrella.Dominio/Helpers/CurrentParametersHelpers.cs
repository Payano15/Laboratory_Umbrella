using Laboratory.Umbrella.Dominio.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratory.Umbrella.Dominio.Helpers;

public class CurrentParametersHelpers
{
    public string Token { get; set; } = string.Empty;

    public void SetAuthenticatedParameters(AuthTokenResponse TokenResponse)
    {
        Token = TokenResponse.Token;
    }
}
