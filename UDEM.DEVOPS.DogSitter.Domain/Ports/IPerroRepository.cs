using UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Domain.Ports
{
    public interface IPerroRepository
    {
        Task<Perro?> GetPerroAsync(Guid id);
        Task<IEnumerable<Perro>> GetAllAsync();
        Task<Perro> SavePerroAsync(Perro c);
        Task<Perro> EditPerroAsync(Perro c);
        Task<Perro> PatchPerroAsync(Perro c);
        Task<bool> DeletePerroAsync(Guid id);
    }
}
