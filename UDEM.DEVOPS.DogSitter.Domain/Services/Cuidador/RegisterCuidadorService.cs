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
            await CheckIfEmailIsRegistered(cuidador);
            await _cuidadorRepository.SaveCuidadorAsync(cuidador);
            return cuidador.Id;
        }
        private async Task CheckIfExistsAsync(Entities.Cuidador cuidador)
        {
            var cuidadorExists = await _cuidadorRepository.GetCuidadorAsync(cuidador.Id);
            if (cuidadorExists is not null) throw new DuplicatedEntityException("El cuidador ya se encuentra registrado");
            
        }
        private async Task CheckIfEmailIsRegistered(Entities.Cuidador cuidador)
        {
            var cuidadores = await _cuidadorRepository.GetAllCuidadoresAync();
            foreach (var c in cuidadores)
            {
                if (c.email == cuidador.email) throw new DuplicatedEmailException($"El email {cuidador.email} se encuentra ya registrado");
            }
        }
    }
}
