using UDEM.DEVOPS.DogSitter.Api.ApiHandlers;
using UDEM.DEVOPS.DogSitter.Api.Filters;
using UDEM.DEVOPS.DogSitter.Api.Middleware;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Infrastructure.Adapters;
using UDEM.DEVOPS.DogSitter.Infrastructure.DataSource;
using UDEM.DEVOPS.DogSitter.Infrastructure.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Prometheus;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Logging.ClearProviders();

builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseNpgsql(config.GetConnectionString("db"));
});

builder.Services.AddHealthChecks()
    .AddDbContextCheck<DataContext>()
    .ForwardToPrometheus();
builder.Services.AddDomainServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Ingrese un token válido",
        Name = "Autorización",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }   
    });
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.Load("UDEM.DEVOPS.DogSitter.Application")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHttpMetrics();

app.UseMiddleware<AppExceptionHandlerMiddleware>();

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    }
});

app.UseRouting().UseEndpoints(endpoint =>
{
    endpoint.MapMetrics();
});

//app.MapGroup("/api/voter").MapVoter().AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);
app.MapGroup("").MapCuidador().AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);
app.MapGroup("").MapRaza().AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory);
await app.RunAsync();

public partial class Program
{
    private Program() { }
}