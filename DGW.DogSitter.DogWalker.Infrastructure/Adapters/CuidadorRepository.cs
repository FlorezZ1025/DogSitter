using DGW.DogSitter.DogWalker.Domain.Dtos;
using DGW.DogSitter.DogWalker.Domain.Entities;
using DGW.DogSitter.DogWalker.Domain.Exceptions;
using DGW.DogSitter.DogWalker.Domain.Mappings;
using DGW.DogSitter.DogWalker.Domain.Ports;
using DGW.DogSitter.DogWalker.Infrastructure.Ports;
using System;

namespace DGW.DogSitter.DogWalker.Infrastructure.Adapters
{
    [Repository]
    public class CuidadorRepository(IRepository<Cuidador> dataSource) : ICuidadorRepository
    {
        readonly IRepository<Cuidador> _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        public async Task<Cuidador?> GetCuidadorAsync(Guid id) => await _dataSource.GetOneAsync(id);
        public async Task<Cuidador> SaveCuidadorAsync(Cuidador c) => await _dataSource.AddAsync(c);
        public async Task<Cuidador> EditCuidadorAsync(Cuidador c) 
        {
            if(c is null) throw new ArgumentNullException(nameof(c));
            return await _dataSource.UpdateAsync(c); 
        }
        public async Task<Cuidador> PatchCuidadorAsync(Guid id, UpdateCuidadorDto dto)
        {
            var entity = await _dataSource.GetOneAsync(id)
                ?? throw new NotFoundCuidadorException("Cuidador no existe");
            entity.UpdateEntity(dto);
            return await dataSource.UpdateAsync(entity);
        }
        public async Task<bool> DeleteCuidadorAsync(Guid id) => await dataSource.DeleteAsync(id); 
    }
}
