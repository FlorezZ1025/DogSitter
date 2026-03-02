using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Domain.Services.Raza
{
    [DomainService]
    public class DeleteRazaService(IRazaRepository razaRepository)
    {
        public async Task DeleteRazaAsync(Guid id)
        {
            var raza = await CheckIfRazaExists(id);
            await CheckIfRazaHasPerrosAlready(raza);
            var deleted = await razaRepository.DeleteRazaAsync(id);
        }
        public async Task<Entities.Raza> CheckIfRazaExists(Guid id)
        {
            var raza = await razaRepository.GetRazaAsync(id)
                ?? throw new NotFoundEntityException("No se encontró la raza a eliminar");
            return raza;
        }
        public async Task CheckIfRazaHasPerrosAlready(Entities.Raza raza)
        {
            if (raza.perros != null && raza.perros.Any())
            {
                throw new DeleteRestrictionException("No se puede eliminar la raza porque tiene perros asociados");
            }
        }
    }
}
