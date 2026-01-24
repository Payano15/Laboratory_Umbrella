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
public class ClientController : BaseController
{
    private readonly IClienteService _clienteService;

    public ClientController(IClienteService clienteService,
                            ISecurityService securityService,
                            ILogger<ClientController> logger) : base(logger, securityService)
    {
        _clienteService = clienteService;

    }

    [HttpPost("V1/HABGSGTO")]
    public async Task<IActionResult> GetClienteAsync(ClientRequest request)
    {
        try
        {
            await Init((BaseService)_clienteService);
            var response = await _clienteService.GetClient(request);
            return Ok(new Response<MetaDataResponse<List<ClientResponse>>>(response));
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

    [HttpGet("V1/HANDLUY")]
    public async Task<IActionResult> GetClienteByIdAsync(string Id)
    {
        try
        {
            await Init((BaseService)_clienteService);
            var response = await _clienteService.GetClientById(Id);
            return Ok(new Response<MetaDataResponse<ClientResponse>>(response));
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

    [HttpPost("V1/HGATROB")]
    public async Task<IActionResult> CreateClientAsync(CreateClientRequest request)
    {
        try
        {
            await Init((BaseService)_clienteService);
            var response = await _clienteService.CreateOrUpdateClient(request);
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
