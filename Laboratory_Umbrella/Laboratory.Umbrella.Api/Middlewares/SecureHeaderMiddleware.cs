using Laboratory.Umbrella.Api.Attributes;
using Laboratory.Umbrella.Dominio.Comman;
using System.Net;

namespace Laboratory.Umbrella.Api.Middlewares;

public class SecureHeaderMiddleware
{
    private static Config? _config;
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public SecureHeaderMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            var attribute = endpoint.Metadata.GetMetadata<HeaderSecureAttribute>();
            if (attribute != null)
            {
                if (_config == null)
                {
                    var configuration = context.RequestServices.GetService<IConfiguration>();
                    _config = configuration?.GetSection(nameof(Config))?.Get<Config>() ?? new();
                }

                if (!context.Request.Headers.TryGetValue("RVHIVHY", out var header1) || header1 != _config.H1)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsJsonAsync(new Response<object>(((int)HttpStatusCode.Unauthorized).ToString(), "Unauthorized Request."));
                    return;
                }

                if (!context.Request.Headers.TryGetValue("KFJTYCC", out var header2) || header2 != _config.H2)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    await context.Response.WriteAsJsonAsync(new Response<object>(((int)HttpStatusCode.Unauthorized).ToString(), "Unauthorized Request."));
                    return;
                }
            }
        }

        await _next(context);
    }
}
