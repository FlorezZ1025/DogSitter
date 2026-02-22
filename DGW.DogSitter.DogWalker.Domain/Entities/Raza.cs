using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGW.DogSitter.DogWalker.Domain.Entities
{
    public class Raza : DomainEntity
    {
        public required string nombre { get; set; }
        public required string corpulencia { get; set; }
        public required string nivelEnergia { get; set; }
        public string? observacionesGenerales { get; set; } = null;

        public ICollection<Perro> perros { get; set; } = [];
    }
}
