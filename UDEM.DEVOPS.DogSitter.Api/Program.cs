using Asp.Versioning;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;
using Serilog;
using Serilog.Sinks.Grafana.Loki;
using System.Reflection;
using UDEM.DEVOPS.DogSitter.Api.ApiHandlers;
using UDEM.DEVOPS.DogSitter.Api.Filters;
using OpenTelemetry.Resources;
using UDEM.DEVOPS.DogSitter.Api.Middleware; 
using UDEM.DEVOPS.DogSitter.Infrastructure.DataSource;
using UDEM.DEVOPS.DogSitter.Infrastructure.Extensions;

using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using Serilog;
using Serilog.Sinks.Grafana.Loki;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.GrafanaLoki(
        config["Grafana:LokiEndpoint"] ?? Environment.GetEnvironmentVariable("GRAFANA_LOKI_ENDPOINT") ?? "",
        credentials: new LokiCredentials
        {
            Login = config["Grafana:InstanceId"] ?? Environment.GetEnvironmentVariable("GRAFANA_INSTANCE_ID") ?? "",
            Password = config["Grafana:ApiToken"] ?? Environment.GetEnvironmentVariable("GRAFANA_API_TOKEN") ?? ""
        },
        labels: new[]
        {
            new LokiLabel { Key = "app", Value = "api-dogsitter" },
            new LokiLabel { Key = "env", Value = "production" }
        })
    .CreateLogger();

builder.Host.UseSerilog();

// Configurar OpenTelemetry para métricas y trazas
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource
        .AddService(config["Grafana:ServiceName"] ?? Environment.GetEnvironmentVariable("GRAFANA_SERVICE_NAME") ?? "api-dogsitter"))
    .WithTracing(tracing => tracing
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter(otlp =>
        {
            otlp.Endpoint = new Uri(
                config["Grafana:OtlpEndpoint"] ?? Environment.GetEnvironmentVariable("GRAFANA_OTLP_ENDPOINT") ?? "");
            otlp.Headers = $"Authorization=Basic {Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(
                    $"{config["Grafana:InstanceId"] ?? Environment.GetEnvironmentVariable("GRAFANA_INSTANCE_ID") ?? ""}:{config["Grafana:ApiToken"] ?? Environment.GetEnvironmentVariable("GRAFANA_API_TOKEN") ?? ""}"))}";
        }))
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter(otlp =>
        {
            otlp.Endpoint = new Uri(
                config["Grafana:OtlpEndpoint"] ?? Environment.GetEnvironmentVariable("GRAFANA_OTLP_ENDPOINT") ?? "");
            otlp.Headers = $"Authorization=Basic {Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes(
                    $"{config["Grafana:InstanceId"] ?? Environment.GetEnvironmentVariable("GRAFANA_INSTANCE_ID") ?? ""}:{config["Grafana:ApiToken"] ?? Environment.GetEnvironmentVariable("GRAFANA_API_TOKEN") ?? ""}"))}";
        }));


builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
//builder.Logging.ClearProviders();
builder.Logging.AddConsole();

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
builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(2, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DogSitter API",
        Version = "v1"
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "DogSitter API",
        Version = "v2"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Ingrese un token válido",
        Name = "Autorización",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.Load("UDEM.DEVOPS.DogSitter.Application")));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "DogSitter API V2");
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "DogSitter API V1");
    options.RoutePrefix = string.Empty;
});

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
var versionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .HasApiVersion(new ApiVersion(2,0))
    .ReportApiVersions()
    .Build();

var v1 = app.MapGroup("/api/v{version:apiVersion}")
    .WithApiVersionSet(versionSet)
    .HasApiVersion(new ApiVersion(1, 0));

v1.MapCuidador();
v1.MapRaza();
v1.MapPerro();

var v2 = app.MapGroup("/api/v{version:apiVersion}")
    .WithApiVersionSet(versionSet)
    .HasApiVersion(new ApiVersion(2,0));
v2.MapCuidador();
v2.MapRaza();
v2.MapPerro();
v2.MapMensaje();

await app.RunAsync();

public partial class Program
{
    private Program() { }
}