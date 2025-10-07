using Laboratory.Umbrella.Api;
using Laboratory.Umbrella.Api.Middlewares;
using Laboratory.Umbrella.Dominio.Common;
using Laboratory.Umbrella.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<Config>(builder.Configuration.GetSection(nameof(Config)));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddHttpClient();
builder.Services.AddConfiguredDatabase(builder.Configuration);

// Register Application Services
builder.Services.AddApplicationDependencies();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
