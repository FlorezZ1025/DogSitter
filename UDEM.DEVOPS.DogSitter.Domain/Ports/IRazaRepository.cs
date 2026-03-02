using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Domain.Ports
{
    public interface IRazaRepository
    {
        Task<Raza?> GetRazaAsync(Guid id);
        Task<IEnumerable<Raza>> GetAllRazasAync();
        Task<Raza> SaveRazaAsync(Raza c);
        Task<Raza> EditRazaAsync(Raza c);
        Task<Raza> PatchRazaAsync(Guid c, UpdateRazaDto dto);
        Task<bool> DeleteRazaAsync(Guid id);
    }
}
