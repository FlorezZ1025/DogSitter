using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDEM.DEVOPS.DogSitter.Domain.Entities
{
    public class Cuidador : DomainEntity
    {
        public required string nombre { get; set; }
        public required string telefono { get; set; }
        public required string email { get; set; }
        public DateTime fechaInicioExperiencia { get; set; }
        public required string direccion {  get; set; }
        public required bool activo { get; set; }
        public ICollection<Perro> perros { get; set; } = [];
    }
}
