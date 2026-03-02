using UDEM.DEVOPS.DogSitter.Infrastructure.DataSource;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;

namespace UDEM.DEVOPS.DogSitter.Infrastructure.Adapters;

[Repository]
public class VoterSimpleRepository(DataContext dc) : IVoterSimpleQueryRepository
{
    private readonly IDbConnection _dbConn = dc.Database.GetDbConnection();

    public async Task<VoterDto> Single(Guid id)
    {
        return await _dbConn.QuerySingleOrDefaultAsync<VoterDto>(@"select id , dateOfBirth, origin from Voter where Id = @Id",
                             new
                             {
                                 Id = id
                             }) ?? throw new NotFoundEntityException("The voter does not exist");
    }
}
