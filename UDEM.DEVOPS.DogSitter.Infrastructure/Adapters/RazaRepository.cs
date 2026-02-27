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
    public class RazaRepository(IRepository<Raza> dataSource) : IRazaRepository
    {
        public async Task<IEnumerable<Raza>> GetAllRazasAync() => await dataSource.GetManyAsync();

        public async Task<Raza?> GetRazaAsync(Guid id)
        {
            var razas = await dataSource.GetManyAsync(filter: p => p.Id == id);
            return razas.FirstOrDefault();
        }
        public async Task<Raza> SaveRazaAsync(Raza r) => await dataSource.AddAsync(r);

        public async Task<Raza> EditRazaAsync(Raza r)
        {
            if(r is null) throw new ArgumentNullException(nameof(r));
            return await dataSource.UpdateAsync(r);
        }

        public async Task<Raza> PatchRazaAsync(Guid id, UpdateRazaDto dto)
        {
            var entity = await dataSource.GetOneAsync(id)
                            ?? throw new NotFoundEntityException("Raza no existe");
            entity.UpdateEntity(dto);
            return await dataSource.UpdateAsync(entity);
        }

        public async Task<bool> DeleteRazaAsync(Guid id) => await dataSource.DeleteAsync(id);
    }
}
