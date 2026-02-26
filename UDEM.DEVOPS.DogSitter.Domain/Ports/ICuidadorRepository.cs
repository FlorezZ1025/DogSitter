using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Domain.Ports
{
    public interface ICuidadorRepository
    {
        Task<Cuidador?> GetCuidadorAsync(Guid id);
        Task<IEnumerable<Cuidador>> GetAllCuidadoresAync();
        Task<Cuidador> SaveCuidadorAsync(Cuidador c);
        Task<Cuidador> EditCuidadorAsync(Cuidador c);   
        Task<Cuidador> PatchCuidadorAsync(Guid id,UpdateCuidadorDto dto);
        Task<bool> DeleteCuidadorAsync(Guid id);
    }
}
