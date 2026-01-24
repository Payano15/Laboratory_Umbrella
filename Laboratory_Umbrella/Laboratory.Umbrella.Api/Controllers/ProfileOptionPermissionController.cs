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
public class ProfileOptionPermissionController : BaseController
{
    private readonly IProfileOptionPermissionService _service;

    public ProfileOptionPermissionController(IProfileOptionPermissionService service,
                                             ISecurityService securityService,
                                             ILogger<ProfileOptionPermissionController> logger) : base(logger, securityService)
    {
        _service = service;
    }

    [HttpPost("V1/HPROPLST")]
    public async Task<IActionResult> GetByParameters(ProfileOptionPermissionByParameters request)
    {
        try
        {
            await Init((BaseService)_service);
            var response = await _service.GetByParameters(request);
            return Ok(new Response<MetaDataResponse<List<ProfileOptionPermissionResponse>>>(response));
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

    [HttpGet("V1/HPROPID")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            await Init((BaseService)_service);
            var response = await _service.GetById(id);
            return Ok(new Response<MetaDataResponse<ProfileOptionPermissionResponse>>(response));
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

    [HttpPost("V1/HPROPSV")]
    public async Task<IActionResult> Save(SaveProfileOptionPermissionRequest request)
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

    [HttpDelete("V1/HPROPDEL")]
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
