using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDEM.DEVOPS.DogSitter.Infrastructure.Health
{
    public static class VersionInfo
    {
        public static string Version =  "1.0.0";
        public static string Release = "stable";
        public static readonly DateTime DeployedAt = DateTime.UtcNow;
    }
}
