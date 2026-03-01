using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Infrastructure.Ports;

namespace UDEM.DEVOPS.DogSitter.Infrastructure.Adapters
{
    [Repository]
    public class PerroRepository(IRepository<Perro> dataSource) : IPerroRepository
    {
        public Task<IEnumerable<Perro>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Perro?> GetPerroAsync(Guid id)
        {
            throw new NotImplementedException();
        }
        public Task<Perro> SavePerroAsync(Perro c)
        {
            throw new NotImplementedException();
        }
        public Task<Perro> EditPerroAsync(Perro c)
        {
            throw new NotImplementedException();
        }


        public Task<Perro> PatchPerroAsync(Perro c)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeletePerroAsync(Guid id)
        {
            throw new NotImplementedException();
        }

    }
}
