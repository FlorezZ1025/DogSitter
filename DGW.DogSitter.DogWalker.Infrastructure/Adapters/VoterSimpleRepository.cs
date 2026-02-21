using DGW.DogSitter.DogWalker.Infrastructure.DataSource;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;
using DGW.DogSitter.DogWalker.Domain.Exceptions;
using DGW.DogSitter.DogWalker.Domain.Ports;
using DGW.DogSitter.DogWalker.Domain.Dtos;

namespace DGW.DogSitter.DogWalker.Infrastructure.Adapters;

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
                             }) ?? throw new NotFoundVoterException("The voter does not exist");
    }
}
