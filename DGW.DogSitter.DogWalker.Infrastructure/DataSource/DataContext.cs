using Microsoft.EntityFrameworkCore;
using DGW.DogSitter.DogWalker.Domain.Entities;

namespace DGW.DogSitter.DogWalker.Infrastructure.DataSource;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Cuidador> Cuidadores { get; set; }
    public DbSet<Raza> Razas { get; set; }
    public DbSet<Perro> Perros { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder is null)
        {
            return;
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        modelBuilder.Ignore<Voter>();
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var t = entityType.ClrType;
            if (typeof(DomainEntity).IsAssignableFrom(t))
            {
                modelBuilder.Entity(entityType.Name).Property<DateTime>("CreatedOn");
                modelBuilder.Entity(entityType.Name).Property<DateTime>("LastModifiedOn");
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}

