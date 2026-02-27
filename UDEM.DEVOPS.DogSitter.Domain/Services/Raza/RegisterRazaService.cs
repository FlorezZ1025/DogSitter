using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Domain.Services.Raza
{
    [DomainService]
    public class RegisterRazaService
    {
        private readonly IRazaRepository _razaRepository;
        public RegisterRazaService(IRazaRepository razaRepository)
        {
            _razaRepository = razaRepository;
        }

        public async Task<Guid> RegisterRazaAsync(Entities.Raza raza)
        {
            await CheckIfExistsAsync(raza);
            await _razaRepository.SaveRazaAsync(raza);
            return raza.Id;
        }

        public async Task CheckIfExistsAsync(Entities.Raza raza)
        {
            var razaExists = await _razaRepository.GetRazaAsync(raza.Id);
            if (razaExists is not null) throw new DuplicatedEntityException("La raza ya se encuentra registrada");
        }

        public async Task CheckIfNameIsRegistered(Entities.Raza raza)
        {
            var razas = await _razaRepository.GetAllRazasAync();
            foreach (var r in razas)
            {
                if (r.nombre == raza.nombre) throw new DuplicatedEntityException($"El nombre de raza {raza.nombre} se encuentra ya registrado");
            }
        }
    }
}
