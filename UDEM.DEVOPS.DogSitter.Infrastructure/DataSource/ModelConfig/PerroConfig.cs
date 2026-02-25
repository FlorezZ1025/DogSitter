using UDEM.DEVOPS.DogSitter.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UDEM.DEVOPS.DogSitter.Infrastructure.DataSource.ModelConfig;

public class PerroEntityTypeConfiguration : IEntityTypeConfiguration<Perro>
{
    public void Configure(EntityTypeBuilder<Perro> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.nombre).HasMaxLength(100);
        builder.Property(p => p.peso).HasPrecision(5, 2);
        builder.Property(p => p.tipoComida).HasMaxLength(100);
        builder.Property(p => p.horarioComida).HasMaxLength(100);
        builder.Property(p => p.alergias).HasMaxLength(250);

        builder.HasOne(p => p.raza)
               .WithMany(r => r.perros)
               .HasForeignKey(p => p.razaId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.cuidador)
               .WithMany(c => c.perros)
               .HasForeignKey(p => p.cuidadorId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}


