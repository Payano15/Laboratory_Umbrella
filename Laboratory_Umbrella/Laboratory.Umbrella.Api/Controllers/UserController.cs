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
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService,
                          ISecurityService securityService,
                          ILogger<UserController> logger) : base(logger, securityService)
    {
        _userService = userService;
    }

    [HttpPost("V1/HUSRPAR")]
    public async Task<IActionResult> GetUsersByParametersAsync(UserByParameters request)
    {
        try
        {
            await Init((BaseService)_userService);
            var response = await _userService.GetByParameters(request);
            return Ok(new Response<MetaDataResponse<List<UsuariosResponse>>>(response));
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

    [HttpGet("V1/HUSRBYID")]
    public async Task<IActionResult> GetUserByIdAsync(string id)
    {
        try
        {
            await Init((BaseService)_userService);
            var response = await _userService.GetById(id);
            return Ok(new Response<MetaDataResponse<UsuariosResponse>>(response));
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

    [HttpPost("V1/HUSRSAVE")]
    public async Task<IActionResult> SaveUserAsync(SaveUserRequest request)
    {
        try
        {
            await Init((BaseService)_userService);
            var response = await _userService.Save(request);
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

    [HttpPost("V1/HUSRCHPASS")]
    public async Task<IActionResult> ChangePasswordAsync(string id, ChangePasswordRequest request)
    {
        try
        {
            await Init((BaseService)_userService);
            var response = await _userService.ChangePassword(id, request);
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

    [HttpPost("V1/HUSRVALPASS")]
    public async Task<IActionResult> ValidatePasswordAsync(ValidatePasswordRequest request)
    {
        try
        {
            await Init((BaseService)_userService);
            var response = await _userService.ValidatePassword(request);
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
