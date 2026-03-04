using FluentValidation.Results;
using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Dominio.Exception;
using Laboratory.Umbrella.Dominio.Helpers;
using Laboratory.Umbrella.Services.Interfaces;
using Laboratory.Umbrella.Services.Services;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Laboratory.Umbrella.Api.Controllers;

public class BaseController : ControllerBase
{
    #region MyRegion
    private readonly ILogger _logger;
    private readonly ISecurityService _securityService;
    #endregion

    #region Constructors
    public BaseController(ILogger logger, ISecurityService securityService)
    {
        _logger = logger;
        _securityService = securityService;
    }
    #endregion

    #region Methods
    protected async Task<CurrentParametersHelpers> Init()
    {
        var token = HttpContext.Request.Headers.Authorization.FirstOrDefault() ?? string.Empty;
        token = token.Replace("Bearer ", "");

        var providerToken = await _securityService.ValidateToken(token);

        if (string.IsNullOrEmpty(providerToken.Token) || string.IsNullOrWhiteSpace(providerToken.Token))
            throw new CustomException(Constants.Code.Error.InvalidToken, Constants.Message.Error.InvalidToken);

        var helper = new CurrentParametersHelpers();

        helper.SetAuthenticatedParameters(providerToken);

        return helper;
    }
    protected Exception GetInnerException(Exception ex)
    {
        if (ex.InnerException == null)
            return ex;

        return ex.InnerException;
    }
    protected UnprocessableEntityObjectResult UnprocessableEntity(List<ValidationFailure> errors)
        => UnprocessableEntity(errors.FirstOrDefault()?.ErrorMessage ?? "Invalid request body");
    protected UnprocessableEntityObjectResult UnprocessableEntity(string error)
        => UnprocessableEntity(new CustomResponse<bool>(error, Types.ResponseCodes.InvalidRequestBody));
    protected BadRequestObjectResult BadRequest(string? error = null)
        => BadRequest(new CustomResponse<bool>(error ?? "Internal error. Try again", Types.ResponseCodes.ServerException));
    protected void LogException(Exception ex, [CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
        => _logger.LogError("SERVER_EXEC - {MESSAGE}. Method: {METHOD} | Controller: {CONTROLLER}", ex.Message, memberName, filePath);
    #endregion
}
