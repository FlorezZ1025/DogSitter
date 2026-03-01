using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Mappings;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Domain.Services.Perro
{
    [DomainService]
    public class EditPerroService(
        IPerroRepository perroRepository,
        IRazaRepository razaRepository,
        ICuidadorRepository cuidadorRepository)
    {
        public async Task<PerroDto> EditPerroAsync(UpdatePerroDto perroDto)
        {
            var entity = await perroRepository.GetPerroAsync(perroDto.Id);
            if (entity is null) throw new NotFoundEntityException("No se encontró el perro a editar");
            entity.UpdateEntity(perroDto);
            await ValidateRazaExistAsync(entity.razaId);
            await ValidateCuidadorExistAsync(entity.cuidadorId);
            await perroRepository.EditPerroAsync(entity);
            return entity.ToResponseDto();
        }
        private async Task ValidateRazaExistAsync(Guid razaId)
        {
            var raza = await razaRepository.GetRazaAsync(razaId);
            if (raza is null) throw new NotFoundEntityException($"La raza con id {razaId} no existe en el sistema");
        }
        private async Task ValidateCuidadorExistAsync(Guid cuidadorId)
        {
            var cuidador = await cuidadorRepository.GetCuidadorAsync(cuidadorId);
            if (cuidador is null) throw new NotFoundEntityException($"El cuidador con id {cuidadorId} no existe en el sistema");
        }
    }
}
