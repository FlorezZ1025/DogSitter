using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Mappings;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Infrastructure.Ports;

namespace UDEM.DEVOPS.DogSitter.Infrastructure.Adapters
{
    [Repository]
    public class PerroRepository(IRepository<Perro> dataSource) : IPerroRepository
    {
        readonly IRepository<Perro> _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));

        public async Task<IEnumerable<Perro>> GetAllAsync() => await _dataSource.GetManyAsync();
        public async Task<Perro?> GetPerroAsync(Guid id)
        {
            var perros = await _dataSource.GetManyAsync(filter: p => p.Id == id);
            return perros.FirstOrDefault();
        }
        public async Task<Perro> SavePerroAsync(Perro p) => await _dataSource.AddAsync(p);
        public async Task<Perro> EditPerroAsync(Perro p)
        {
            if(p is null) throw new ArgumentNullException(nameof(p));
            return await _dataSource.UpdateAsync(p);
        }

        public async Task<Perro> PatchPerroAsync(Guid id, UpdatePerroDto dto)
        {
            var entity = await _dataSource.GetOneAsync(id)
                ?? throw new NotFoundEntityException($"No se encontró un perro con el id {id}");
            entity.UpdateEntity(dto);
            return await _dataSource.UpdateAsync(entity);
        }
        public async Task<bool> DeletePerroAsync(Guid id) => await _dataSource.DeleteAsync(id);

    }
}
