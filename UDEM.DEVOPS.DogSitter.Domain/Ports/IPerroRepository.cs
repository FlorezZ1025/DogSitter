using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Domain.Ports
{
    public interface IPerroRepository
    {
        Task<Perro?> GetPerroAsync(Guid id);
        Task<IEnumerable<Perro>> GetAllAsync();
        Task<Perro> SavePerroAsync(Perro p);
        Task<Perro> EditPerroAsync(Perro p);
        Task<Perro> PatchPerroAsync(Guid id, UpdatePerroDto dto);
        Task<bool> DeletePerroAsync(Guid id);
    }
}
