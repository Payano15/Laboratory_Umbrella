using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Exception;
using Laboratory.Umbrella.Dominio.Request;
using Laboratory.Umbrella.Dominio.Response;
using Laboratory.Umbrella.Services.Interfaces;
using Laboratory.Umbrella.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace Laboratory.Umbrella.Api.Controllers;

[ApiController]
[Route("api/SMALPHY")]
public class UserProfileController : BaseController
{
    private readonly IUserProfileService _service;

    public UserProfileController(IUserProfileService service,
                                 ISecurityService securityService,
                                 ILogger<UserProfileController> logger) : base(logger, securityService)
    {
        _service = service;
    }

    [HttpPost("V1/HUSRPRLST")]
    public async Task<IActionResult> GetByParameters(UserProfileByParameters request)
    {
        try
        {
            await Init((BaseService)_service);
            var response = await _service.GetByParameters(request);
            return Ok(new Response<MetaDataResponse<List<UserProfileResponse>>>(response));
        }
        catch (CustomException ex)
        {
            return Ok(new Response<object>(ex.Code, ex.Message));
        }
        catch (Exception ex)
        {
            return Ok(new Response<object>("GCO001", ex.Message));
        }
    }

    [HttpGet("V1/HUSRPRID")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            await Init((BaseService)_service);
            var response = await _service.GetById(id);
            return Ok(new Response<MetaDataResponse<UserProfileResponse>>(response));
        }
        catch (CustomException ex)
        {
            return Ok(new Response<object>(ex.Code, ex.Message));
        }
        catch (Exception ex)
        {
            return Ok(new Response<object>("GCO001", ex.Message));
        }
    }

    [HttpPost("V1/HUSRPRSV")]
    public async Task<IActionResult> Save(SaveUserProfileRequest request)
    {
        try
        {
            await Init((BaseService)_service);
            var response = await _service.Save(request);
            return Ok(new Response<MetaDataResponse<bool>>(response));
        }
        catch (CustomException ex)
        {
            return Ok(new Response<object>(ex.Code, ex.Message));
        }
        catch (Exception ex)
        {
            return Ok(new Response<object>("GCO001", ex.Message));
        }
    }

    [HttpDelete("V1/HUSRPRDEL")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await Init((BaseService)_service);
            var response = await _service.Delete(id);
            return Ok(new Response<MetaDataResponse<bool>>(response));
        }
        catch (CustomException ex)
        {
            return Ok(new Response<object>(ex.Code, ex.Message));
        }
        catch (Exception ex)
        {
            return Ok(new Response<object>("GCO001", ex.Message));
        }
    }
}
