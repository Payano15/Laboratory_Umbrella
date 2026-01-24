using Laboratory.Umbrella.Api;
using Laboratory.Umbrella.Api.Middlewares;
using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Configure<Config>(builder.Configuration.GetSection(nameof(Config)));
builder.Services.AddHttpClient();
builder.Services.AddConfiguredDatabase(builder.Configuration);

builder.Services.AddApplicationDependencies();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<SecureHeaderMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
