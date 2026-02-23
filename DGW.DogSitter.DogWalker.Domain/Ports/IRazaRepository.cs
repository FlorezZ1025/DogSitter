using DGW.DogSitter.DogWalker.Domain.Entities;

namespace DGW.DogSitter.DogWalker.Domain.Ports
{
    public interface IRazaRepository
    {
        Task<Raza?> GetRazaAsync(Guid id);
        Task<Raza> SaveRazaAsync(Raza c);
        Task<Raza> EditRazaAsync(Raza c);
        Task<Raza> PatchRazaAsync(Raza c);
        Task<bool> DeleteRazaAsync(Guid id);
    }
}
