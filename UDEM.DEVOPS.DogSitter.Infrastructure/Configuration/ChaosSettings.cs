using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDEM.DEVOPS.DogSitter.Infrastructure.Configuration
{
    public sealed class ChaosSettings
    {
        public bool Enabled { get; set; } = true;
        public double InjectionRate { get; set; } = 0.6; // 60% de fallos
    }
}
