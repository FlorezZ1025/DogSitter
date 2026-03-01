using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Mappings;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Domain.Services.Raza
{
    [DomainService]
    public class EditRazaService
    {
        private readonly IRazaRepository _razaRepository;
        public EditRazaService(IRazaRepository razaRepository) 
        { 
            _razaRepository = razaRepository;    
        }

        public async Task<RazaDto> EditRazaAsync(UpdateRazaDto razaDto)
        {
            var entity = await _razaRepository.GetRazaAsync(razaDto.Id);
            if (entity is null) throw new NotFoundEntityException("No se encontró la raza a editar");
            await CheckIfNameIsRegistered(razaDto);

            entity.UpdateEntity(razaDto);
            await _razaRepository.EditRazaAsync(entity);
            return entity.ToResponseDto();
        }

        private async Task CheckIfNameIsRegistered(UpdateRazaDto raza)
        {
            var razas = await _razaRepository.GetAllRazasAync();
            if (razas.Any(r => r.Id != raza.Id && r.nombre.ToLower() == raza?.nombre?.ToLower()))
                throw new DuplicatedNombreRazaException("Ya existe una raza con el mismo nombre");
        }
    }
}
