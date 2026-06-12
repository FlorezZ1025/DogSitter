using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Simmy;
using Polly.Simmy.Fault;
using System.Net;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services;
using UDEM.DEVOPS.DogSitter.Infrastructure.Adapters;
using UDEM.DEVOPS.DogSitter.Infrastructure.Configuration;
using UDEM.DEVOPS.DogSitter.Infrastructure.Ports;



namespace UDEM.DEVOPS.DogSitter.Infrastructure.Extensions;

public static class AutoLoadServices
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        // generic repository
        services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));
        // unit of work
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<IMessageService, MessageService>();

        var chaosSettings = new ChaosSettings();

        services.AddSingleton(chaosSettings);

        services.AddHttpClient<IRickAndMortyChaosService, RickAndMortyChaosService>()
            .AddResilienceHandler("rick-and-morty-chaos", (builder, context) =>
            {
                var settings = context.ServiceProvider.GetRequiredService<ChaosSettings>();
                var logger = context.ServiceProvider.GetRequiredService<ILogger<RickAndMortyChaosService>>();
                // SIMMY: Fault injection — HTTP 500 intermitente
                builder.AddChaosFault(new ChaosFaultStrategyOptions
                {
                    InjectionRate = settings.InjectionRate,
                    EnabledGenerator = _ => ValueTask.FromResult(settings.Enabled),
                    FaultGenerator = new FaultGenerator()
                        .AddException(() => new HttpRequestException(
                            "💥 [CHAOS INJECTED] Rick and Morty API simulated failure",
                            null,
                            HttpStatusCode.InternalServerError))
                });
            });
        services.AddHttpClient<IRickAndMortyService, RickAndMortyService>()
            .AddResilienceHandler("rick-and-morty-chaos-only", (builder, context) =>
            {
                var settings = context.ServiceProvider.GetRequiredService<ChaosSettings>();
                var logger = context.ServiceProvider.GetRequiredService<ILogger<RickAndMortyService>>();

                builder.AddRetry(new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = 4,
                    Delay = TimeSpan.FromMilliseconds(300),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,

                    ShouldHandle = args =>
                    {
                        var shouldRetry = 
                            args.Outcome.Exception is HttpRequestException ||
                            args.Outcome.Result?.StatusCode == HttpStatusCode.InternalServerError;
                        return ValueTask.FromResult(shouldRetry);
                    },

                    OnRetry = args =>
                    {
                        logger.LogWarning(
                            """⚠️ [RETRY] Attempt {RetryAttempt} due to: {ExceptionMessage}. Delay: {RetryDelay}ms""",
                            args.AttemptNumber + 1,
                            args.Outcome.Exception?.Message
                            ?? args.Outcome.Result?.StatusCode.ToString(),
                            args.RetryDelay.TotalMilliseconds
                            );
                        return ValueTask.CompletedTask;
                    }
                });

                // SIMMY: Fault injection — HTTP 500 intermitente
                builder.AddChaosFault(new ChaosFaultStrategyOptions
                {
                    InjectionRate = settings.InjectionRate,
                    EnabledGenerator = _ => ValueTask.FromResult(settings.Enabled),
                    FaultGenerator = new FaultGenerator()
                        .AddException(() => new HttpRequestException(
                            "💥 [CHAOS INJECTED] Rick and Morty API simulated failure",
                            null,
                            HttpStatusCode.InternalServerError))
                });
            });

        // all services with domain service attribute, we can also do this "by convention",
        // naming services with suffix "Service" if decide to remove the domain service decorator
        var _services = AppDomain.CurrentDomain.GetAssemblies()
              .Where(assembly =>
              {
                  return assembly.FullName is null || assembly.FullName.Contains("Domain", StringComparison.InvariantCulture);
              })
              .SelectMany(s => s.GetTypes())
              .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(DomainServiceAttribute)));

                // Ditto, but repositories
                var _repositories = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(assembly =>
                    {
                        return assembly.FullName is null || assembly.FullName.Contains("Infrastructure", StringComparison.InvariantCulture);
                    })
                    .SelectMany(s => s.GetTypes())
                    .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(RepositoryAttribute)));

                // svc
                foreach (var service in _services)
                {
                    services.AddTransient(service);
                }

                // repos
                foreach (var repo in _repositories)
                {
                    Type iface = repo.GetInterfaces().Single();
                    services.AddTransient(iface, repo);
                }

                return services;
            }
}
