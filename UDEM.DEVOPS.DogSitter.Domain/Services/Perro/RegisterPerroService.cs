using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Domain.Services.Perro
{
    [DomainService]
    public class RegisterPerroService(IPerroRepository perroRepository, IRazaRepository razaRepository, ICuidadorRepository cuidadorRepository)
    {
        public async Task<Entities.Perro> RegisterPerroAsync(Entities.Perro perro)
        {
            await CheckIfExistsAsync(perro);
            await ValidateRazaExistAsync(perro.razaId);
            await ValidateCuidadorExistAsync(perro.cuidadorId);
            var created = await perroRepository.SavePerroAsync(perro);
            return created;
        }
        private async Task CheckIfExistsAsync(Entities.Perro perro)
        {
            var perroExists = await perroRepository.GetPerroAsync(perro.Id);
            if (perroExists is not null) throw new DuplicatedEntityException("El perro ya se encuentra registrado");
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
