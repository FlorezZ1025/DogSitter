using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DGW.DogSitter.DogWalker.Domain.Entities
{
    public class Perro : DomainEntity
    {
        public required string nombre { get; set; }
        public required short edad { get; set; }
        public required decimal peso { get; set; }
        public Guid razaId {  get; set; }
        public required Raza raza { get; set; }
        public Guid cuidadorId { get; set; }
        public required Cuidador cuidador { get; set; }
        public required string tipoComida { get; set; } 
        public required string horarioComida { get; set; }
        public required string alergias { get; set;  }
        public string? observaciones { get; set; } = null;
    }
}
