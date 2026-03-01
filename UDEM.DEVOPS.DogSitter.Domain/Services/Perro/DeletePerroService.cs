using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Domain.Services.Perro
{
    [DomainService]
    public class DeletePerroService(IPerroRepository perroRepository)
    {
        public async Task DeletePerroAsync(Guid id)
        {
            var perro = await CheckIfPerroExists(id);
            await perroRepository.DeletePerroAsync(id);
        }
        private async Task<Entities.Perro> CheckIfPerroExists(Guid id)
        {
            var perro = await perroRepository.GetPerroAsync(id)
                ?? throw new NotFoundEntityException("No se encontró el perro a eliminar");
            return perro;
        }

    }
}
