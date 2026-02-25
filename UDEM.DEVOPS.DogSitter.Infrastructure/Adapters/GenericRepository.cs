using UDEM.DEVOPS.DogSitter.Infrastructure.DataSource;
using UDEM.DEVOPS.DogSitter.Infrastructure.Ports;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Infrastructure.Adapters;

public class GenericRepository<T> : IRepository<T> where T : DomainEntity
{
    readonly DataContext Context;
    readonly DbSet<T> Dataset;

    public GenericRepository(DataContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        Dataset = Context.Set<T>();
    }


    public async Task<IEnumerable<T>> GetManyAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeStringProperties = "", bool isTracking = false)
    {
        IQueryable<T> query = Context.Set<T>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeStringProperties))
        {
            foreach (var includeProperty in includeStringProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync().ConfigureAwait(false);
        }

        return !isTracking ? await query.AsNoTracking().ToListAsync() : await query.ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        _ = entity ?? throw new ArgumentNullException(nameof(entity), "Entity can not be null");
        await Dataset.AddAsync(entity);
        return entity;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        //_ = entity ?? throw new ArgumentNullException(nameof(entity), "Entity can not be null");
        var affected = await Dataset.Where(e => e.Id == id).ExecuteDeleteAsync();
        return affected > 0;
    }

    public async Task<T> GetOneAsync(Guid id)
    {
        return await Dataset.FindAsync(id) ?? throw new ArgumentNullException(nameof(id));

    }

    public async Task<T> UpdateAsync(T entity)
    {
        Context.Set<T>();
        Dataset.Update(entity);
        return await Task.FromResult(entity);
    }

}
