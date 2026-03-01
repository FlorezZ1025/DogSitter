using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Mappings;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Infrastructure.Ports;
using System;

namespace UDEM.DEVOPS.DogSitter.Infrastructure.Adapters
{
    [Repository]
    public class CuidadorRepository(IRepository<Cuidador> dataSource) : ICuidadorRepository
    {
        readonly IRepository<Cuidador> _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        public async Task<IEnumerable<Cuidador>> GetAllCuidadoresAsync() => await _dataSource.GetManyAsync();
        public async Task<Cuidador?> GetCuidadorAsync(Guid id)
        {
            var cuidadores = await _dataSource.GetManyAsync(filter: p => p.Id == id);
            return cuidadores.FirstOrDefault();
        }
        public async Task<Cuidador> SaveCuidadorAsync(Cuidador c) => await _dataSource.AddAsync(c);
        public async Task<Cuidador> EditCuidadorAsync(Cuidador c) 
        {
            if(c is null) throw new ArgumentNullException(nameof(c));
            return await _dataSource.UpdateAsync(c); 
        }
        public async Task<Cuidador> PatchCuidadorAsync(Guid id, UpdateCuidadorDto dto)
        {
            var entity = await _dataSource.GetOneAsync(id)
                ?? throw new NotFoundEntityException("Cuidador no existe");
            entity.UpdateEntity(dto);
            return await dataSource.UpdateAsync(entity);
        }
        public async Task<bool> DeleteCuidadorAsync(Guid id) => await dataSource.DeleteAsync(id);

      
    }
}
