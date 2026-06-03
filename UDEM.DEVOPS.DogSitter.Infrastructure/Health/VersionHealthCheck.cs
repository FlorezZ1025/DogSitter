using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDEM.DEVOPS.DogSitter.Infrastructure.Health
{
    public class VersionHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            var data = new Dictionary<string, object>
        {
            { "version", VersionInfo.Version },
            { "release", VersionInfo.Release },
            { "deployedAt", VersionInfo.DeployedAt },
            { "status", "healthy" }
        };

            return Task.FromResult(
                HealthCheckResult.Healthy("API is running", data));
        }
    }
}
