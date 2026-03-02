
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Domain.Services.Cuidador
{
    [DomainService]
    public class DeleteCuidadorService
    {
        private readonly ICuidadorRepository _cuidadorRepository;
        public DeleteCuidadorService(ICuidadorRepository cuidadorRepository)
        {
            _cuidadorRepository = cuidadorRepository;
        }

        public async Task DeleteCuidadorAsync(Guid id)
        {
            var cuidador = await CheckIfCuidadorExists(id);
            await CheckIfCuidadorHasPerrosAlready(cuidador);
            await _cuidadorRepository.DeleteCuidadorAsync(id);
        }
        public async Task<Entities.Cuidador> CheckIfCuidadorExists(Guid id)
        {
            var cuidador = await _cuidadorRepository.GetCuidadorAsync(id)
                ?? throw new NotFoundEntityException("No se encontró el cuidador a eliminar");
            return cuidador;
        }
        public async Task CheckIfCuidadorHasPerrosAlready(Entities.Cuidador cuidador)
        {
            if (cuidador.perros != null && cuidador.perros.Any())
            {
                throw new DeleteRestrictionException("No se puede eliminar el cuidador porque tiene perros asociados");
            }
        }
    }
}
