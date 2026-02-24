using DGW.DogSitter.DogWalker.Infrastructure.Ports;
using DGW.DogSitter.DogWalker.Domain.Entities;
using DGW.DogSitter.DogWalker.Domain.Ports;

namespace DGW.DogSitter.DogWalker.Infrastructure.Adapters
{
    [Repository]
    public class VoterRepository(IRepository<Voter> dataSource) : IVoterRepository
    {
        readonly IRepository<Voter> _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));

        public async Task<Voter> SaveVoterAsync(Voter v) => await _dataSource.AddAsync(v);

        public async Task<Voter?> ExistsAsync(Guid uid)
        {
            var voters = await _dataSource.GetManyAsync(filter: p => p.Id == uid);
            return voters.FirstOrDefault();
        }
    }
}