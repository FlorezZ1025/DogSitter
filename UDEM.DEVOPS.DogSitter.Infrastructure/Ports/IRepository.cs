using System.Linq.Expressions;
using UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Infrastructure.Ports;

public interface IRepository<T> where T : DomainEntity
{
    Task<T> GetOneAsync(Guid id);
    Task<IEnumerable<T>> GetManyAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeStringProperties = "",
        bool isTracking = false);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(Guid id);

}
