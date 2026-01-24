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
public class OpcionesController : BaseController
{
    private readonly IOpcionesService _service;

    public OpcionesController(IOpcionesService service,
                              ISecurityService securityService,
                              ILogger<OpcionesController> logger) : base(logger, securityService)
    {
        _service = service;
    }

    [HttpPost("V1/HOPTLST")]
    public async Task<IActionResult> GetByParameters(OpcionesByParameters request)
    {
        try
        {
            await Init((BaseService)_service);
            var response = await _service.GetByParameters(request);
            return Ok(new Response<MetaDataResponse<List<OpcionesResponse>>>(response));
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

    [HttpGet("V1/HOPTID")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            await Init((BaseService)_service);
            var response = await _service.GetById(id);
            return Ok(new Response<MetaDataResponse<OpcionesResponse>>(response));
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

    [HttpPost("V1/HOPTSV")]
    public async Task<IActionResult> Save(SaveOpcionesRequest request)
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

    [HttpDelete("V1/HOPTDEL")]
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
