using DGW.DogSitter.DogWalker.Domain.Entities;
using DGW.DogSitter.DogWalker.Domain.Ports;
using DGW.DogSitter.DogWalker.Infrastructure.DataSource;
using DGW.DogSitter.DogWalker.Infrastructure.Ports;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;

using System.Text.Json;

namespace DGW.DogSitter.DogWalker.Api.Tests;

public class ApiApp : WebApplicationFactory<Program>, IAsyncLifetime
{
    const string ENCABEZADO_AUTHORIZATION = "Authorization";
    const string BEARER = "Bearer";
    public const string TOKEN_VALIDO_PERMISOS = "eyJhbGciOiJSUzI1NiIsImtpZCI6ImEyWm9IcVVBYkU2cHl6enR5RTZka1JGQmVoRTd2RFJERUEyRWloUGVaUWsiLCJ0eXAiOiJKV1QifQ.eyJleHAiOjE3MTk5NTI4NTcsIm5iZiI6MTcxOTk0OTI1NywidmVyIjoiMS4wIiwiaXNzIjoiaHR0cHM6Ly9iMmNiaWJjb21wdHJhbnNzZ2RsbC5iMmNsb2dpbi5jb20vYTE4MjRiMzEtZWQ5My00NTRiLTkyY2EtZjYxOTMwNGQ4ODZkL3YyLjAvIiwic3ViIjoiMTRjZDZhMjEtZWI4My00MjRiLTkxMGQtNTM4ZThlNzRhNTQ4IiwiYXVkIjoiODdlYWJlYzAtN2MxNi00OWJlLWE2NzctMTJkNzI2YWVlYjQ4IiwiYWNyIjoiYjJjXzFhX3NpZ251cF9zaWduaW4iLCJub25jZSI6ImRlZmF1bHROb25jZSIsImlhdCI6MTcxOTk0OTI1NywiYXV0aF90aW1lIjoxNzE5OTQ5MjU3LCJlbWFpbCI6InJkemFsZWphbmRybzMxNkBnbWFpbC5jb20iLCJuYW1lIjoiam9yZ2UgYWxlamFuZHJvIGh1ZXJmYW5vIHJvZHJpZ3VleiIsImdpdmVuX25hbWUiOiJqb3JnZSBhbGVqYW5kcm8iLCJmYW1pbHlfbmFtZSI6Imh1ZXJmYW5vIHJvZHJpZ3VleiIsInRpZCI6ImExODI0YjMxLWVkOTMtNDU0Yi05MmNhLWY2MTkzMDRkODg2ZCJ9.naFzIpEYz0fetDc1MMTnTZJiSXEczbxaQCt-h2HueAqxdMwxWzkiHfLmdh4kO_BAzVt29JDwk9Ks69aLePS31Sn9P59mKVCW6_Elbo7zX8idedItJjqseuBq3ENw3cOP_33-Bgm8YMWCWVyPyq1XY8TKQY4cytNAsbfj443cH44pVc1xvnRJtP7Xm6lHMiANady1bxlHHFf8eQRR496AyPU8sqqdKIlSU8dNrFX5wKASeW4DfswK4cgo2bwcRJHr-HBAjpH6bmQGrq3G9JwhDmm61j9es9dkiS3wJTXr5d78O1CKhUSrUpgVLpamJDtC8IOQf4arjdCQSNBr6DBi_w";

    readonly Guid _id;
    public Guid UserId => _id;
    public Voter _voter;


    public ApiApp()
    {
        _id = Guid.NewGuid();
        _voter = default!;
    }

    public async Task<IServiceProvider> GetServiceCollectionAsync()
    {
        using var scope = Services.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<Voter>>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        _voter = await repository.AddAsync(new Voter("1234567890", DateTime.Now.AddYears(-18), "Colombia"));
        await unitOfWork.SaveAsync(new CancellationTokenSource().Token);

        return Services;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(svc =>
        {
            svc.RemoveAll(typeof(DbContextOptions<DataContext>));
            svc.AddDbContext<DataContext>(opt =>
            {
                opt.UseInMemoryDatabase("testdb");
            });

            svc.RemoveAll(typeof(ILoggerProvider));
            svc.AddSingleton(sp => CrearLoggerProviderTest());        

        });

        return base.CreateHost(builder);
    }

    /// <summary>
    /// Metodo para crear mock de ILoggerProvider
    /// </summary>
    /// <returns>ILoggerProvider</returns>
    private static ILoggerProvider CrearLoggerProviderTest()
    {
        var loggerProviderMock = Substitute.For<ILoggerProvider>();
        loggerProviderMock.CreateLogger(Arg.Any<string>());
        return loggerProviderMock;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public static void AgregarTokenASolicitud(HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Add(ENCABEZADO_AUTHORIZATION, $"{BEARER} {token}");
    }
}