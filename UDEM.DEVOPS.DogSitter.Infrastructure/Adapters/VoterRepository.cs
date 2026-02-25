using UDEM.DEVOPS.DogSitter.Infrastructure.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Ports;

namespace UDEM.DEVOPS.DogSitter.Infrastructure.Adapters
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