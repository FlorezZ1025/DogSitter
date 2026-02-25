using UDEM.DEVOPS.DogSitter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UDEM.DEVOPS.DogSitter.Infrastructure.DataSource.ModelConfig;

public class CuidadorEntityTypeConfiguration : IEntityTypeConfiguration<Cuidador>
{
    public void Configure(EntityTypeBuilder<Cuidador> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.nombre).HasMaxLength(150);
        builder.Property(c => c.telefono).HasMaxLength(20);
        builder.Property(c => c.email).HasMaxLength(150);
        builder.Property(c => c.direccion).HasMaxLength(250);

        builder.HasIndex(c => c.email).IsUnique();
    }
}


