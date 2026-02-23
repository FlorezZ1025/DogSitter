using DGW.DogSitter.DogWalker.Domain.Entities;

namespace DGW.DogSitter.DogWalker.Domain.Ports
{
    public interface IPerroRepository
    {
        Task<Perro?> GetPerroAsync(Guid id);
        Task<Perro> SavePerroAsync(Perro c);
        Task<Perro> EditPerroAsync(Perro c);
        Task<Perro> PatchPerroAsync(Perro c);
        Task<bool> DeletePerroAsync(Guid id);
    }
}
