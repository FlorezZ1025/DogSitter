using UDEM.DEVOPS.DogSitter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UDEM.DEVOPS.DogSitter.Infrastructure.DataSource.ModelConfig;

public class RazaEntityTypeConfiguration : IEntityTypeConfiguration<Raza>
{
    public void Configure(EntityTypeBuilder<Raza> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.nombre).HasMaxLength(100);
        builder.Property(r => r.corpulencia).HasMaxLength(20);
        builder.Property(r => r.nivelEnergia).HasMaxLength(20);
        builder.Property(r => r.observacionesGenerales).HasMaxLength(1000);
    }
}


