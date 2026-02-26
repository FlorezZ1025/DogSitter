using UDEM.DEVOPS.DogSitter.Infrastructure.DataSource;
using Microsoft.EntityFrameworkCore;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using Microsoft.Extensions.Logging;

namespace UDEM.DEVOPS.DogSitter.Infrastructure.Adapters;

public class UnitOfWork(DataContext context, ILogger<UnitOfWork> _logger) : IUnitOfWork
{
    private readonly DataContext _context = context;

    public async Task SaveAsync(CancellationToken? cancellationToken = null)
    {
        var token = cancellationToken ?? new CancellationTokenSource().Token;

        _context.ChangeTracker.DetectChanges();

        var entryStatus = new Dictionary<EntityState, string> {
            {EntityState.Added, "CreatedOn"},
            {EntityState.Modified, "LastModifiedOn"}
        };

        _context.ChangeTracker.Entries().Where(e => entryStatus.ContainsKey(e.State)).ToList().ForEach(e =>
        {
            e.Property(entryStatus[e.State]).CurrentValue = DateTime.UtcNow;
        });
        try
        {
            await _context.SaveChangesAsync(token);
        }catch(Exception ex)
        {
            _logger.LogError(ex, "An error occurred while saving changes to the database.");
            throw new Exception($"Error en unit of work {ex.InnerException}", ex.InnerException);
        }

    }
}
