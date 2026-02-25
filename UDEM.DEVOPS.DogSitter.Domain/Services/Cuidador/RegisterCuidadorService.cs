using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDEM.DEVOPS.DogSitter.Domain.Services.Cuidador
{
    [DomainService]
    public class RegisterCuidadorService
    {
        private readonly ICuidadorRepository _cuidadorRepository;
        public RegisterCuidadorService(ICuidadorRepository cuidadorRepository) => _cuidadorRepository = cuidadorRepository;

        public async Task<Guid> RegisterCuidadorAsync(Entities.Cuidador cuidador)
        {
            await CheckIfExistsAsync(cuidador);
            await _cuidadorRepository.SaveCuidadorAsync(cuidador);
            return cuidador.Id;
        }
        private async Task CheckIfExistsAsync(Entities.Cuidador cuidador)
        {
            var voterExists = await _cuidadorRepository.GetCuidadorAsync(cuidador.Id);
            if (voterExists is not null)
            {
                throw new DuplicatedCuidadorException("El cuidador ya se encuentra registrado");
            }
        }
    }
}
