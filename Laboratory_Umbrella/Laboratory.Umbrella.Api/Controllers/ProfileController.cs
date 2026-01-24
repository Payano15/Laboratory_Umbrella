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
public class ProfileController : BaseController
{
    private readonly IProfileService _service;

    public ProfileController(IProfileService service,
                             ISecurityService securityService,
                             ILogger<ProfileController> logger) : base(logger, securityService)
    {
        _service = service;
    }

    [HttpPost("V1/HPROFLST")]
    public async Task<IActionResult> GetByParameters(ProfileByParameters request)
    {
        try
        {
            await Init((BaseService)_service);
            var response = await _service.GetByParameters(request);
            return Ok(new Response<MetaDataResponse<List<ProfileResponse>>>(response));
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

    [HttpGet("V1/HPROFID")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            await Init((BaseService)_service);
            var response = await _service.GetById(id);
            return Ok(new Response<MetaDataResponse<ProfileResponse>>(response));
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

    [HttpPost("V1/HPROFSV")]
    public async Task<IActionResult> Save(SaveProfileRequest request)
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

    [HttpDelete("V1/HPROFDEL")]
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
