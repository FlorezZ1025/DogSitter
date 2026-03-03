using UDEM.DEVOPS.DogSitter.Infrastructure.DataSource;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace UDEM.DEVOPS.DogSitter.Api.Tests;

public class DogSitterApiApp : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly string _dbName = $"dogsitter-testdb-{Guid.NewGuid()}";

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(svc =>
        {
            svc.RemoveAll(typeof(DbContextOptions<DataContext>));
            svc.AddDbContext<DataContext>(opt =>
            {
                opt.UseInMemoryDatabase(_dbName);
            });

            svc.RemoveAll(typeof(ILoggerProvider));
            svc.AddSingleton(sp =>
            {
                var mock = Substitute.For<ILoggerProvider>();
                mock.CreateLogger(Arg.Any<string>());
                return mock;
            });
        });

        return base.CreateHost(builder);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    Task IAsyncLifetime.DisposeAsync() => Task.CompletedTask;
}
