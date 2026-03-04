using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;
using Laboratory.Umbrella.Services.Interfaces;
using Laboratory.Umbrella.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Laboratory.Umbrella.Api.Controllers;

[ApiController]
[Route("api/WFETGEFEJE")]
public class AuthenticationService: BaseController
{
    #region Properties
    private readonly IAuthenticationService _authenticationService;
    #endregion

    #region Constructor
    public AuthenticationService(IAuthenticationService authenticationService,
                                 ISecurityService securityService,
                                 ILogger<AuthenticationService> logger) : base(logger, securityService)
    {
        _authenticationService = authenticationService;
    }
    #endregion

    #region Methods
    [HttpGet("V1/RGWAFSEXTI")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            var response = await _authenticationService.Login(request);
            return Ok(new Response<LoginResponse>(response));
        }
        catch (Exception ex)
        {
            return Ok(new Response<object>("GCO001", ex.Message));
        }
    }
    #endregion
}
