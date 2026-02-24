using DGW.DogSitter.DogWalker.Domain.Dtos;
using DGW.DogSitter.DogWalker.Domain.Entities;

namespace DGW.DogSitter.DogWalker.Domain.Ports
{
    public interface ICuidadorRepository
    {
        Task<Cuidador?> GetCuidadorAsync(Guid id);
        Task<Cuidador> SaveCuidadorAsync(Cuidador c);
        Task<Cuidador> EditCuidadorAsync(Cuidador c);   
        Task<Cuidador> PatchCuidadorAsync(Guid id,UpdateCuidadorDto dto);
        Task<bool> DeleteCuidadorAsync(Guid id);
    }
}
