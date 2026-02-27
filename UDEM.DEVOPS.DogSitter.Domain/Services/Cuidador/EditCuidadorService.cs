using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Mappings;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Domain.Services.Cuidador
{
    [DomainService]
    public class EditCuidadorService
    {
        private readonly ICuidadorRepository _cuidadorRepository;
        public EditCuidadorService(ICuidadorRepository cuidadorRepository)
        {
            _cuidadorRepository = cuidadorRepository;
        }
        public async Task<CuidadorDto> EditCuidadorAsync(UpdateCuidadorDto cuidadorDto)
        {
            var entity = await _cuidadorRepository.GetCuidadorAsync(cuidadorDto.Id);
            if (entity is null) throw new NotFoundEntityException("No se encontró el cuidador a editar");
            await CheckIfNewEmailExists(cuidadorDto);
            entity.UpdateEntity(cuidadorDto);
            await _cuidadorRepository.EditCuidadorAsync(entity);
            return entity.ToResponseDto();
        }
        private async Task CheckIfNewEmailExists(UpdateCuidadorDto cuidador)
        {
            if(cuidador.email != null)
            {
                var cuidadores = await _cuidadorRepository.GetAllCuidadoresAync();
                foreach (var c in cuidadores)
                {
                    if (c.email == cuidador.email && c.Id != cuidador.Id) throw new DuplicatedEmailException($"El email {cuidador.email} ya se encuentra registrado, debe registrar otro");
                }
            }
        }
    }
}
