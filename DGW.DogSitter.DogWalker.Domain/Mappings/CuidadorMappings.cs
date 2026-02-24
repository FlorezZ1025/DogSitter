using DGW.DogSitter.DogWalker.Domain.Dtos;
using DGW.DogSitter.DogWalker.Domain.Entities;

namespace DGW.DogSitter.DogWalker.Domain.Mappings
{
    public static class CuidadorMappings
    {
        public static Cuidador ToEntity(this CuidadorDto dto)
        {
            return new Cuidador
            {
                Id = Guid.NewGuid(),
                nombre = dto.nombre,
                telefono = dto.telefono,
                email = dto.email,
                fechaInicioExperiencia = dto.fechaInicioExperiencia,
                direccion = dto.direccion,
                activo = dto.activo,
            };
        }

        public static void UpdateEntity(this Cuidador cuidador, UpdateCuidadorDto dto)
        {
            if (cuidador is null) throw new ArgumentNullException(nameof(cuidador));
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            if (dto.nombre is not null) cuidador.nombre = dto.nombre;
            if (dto.telefono is not null) cuidador.telefono = dto.telefono;
            if (dto.email is not null) cuidador.email = dto.email;
            if (dto.fechaInicioExperiencia.HasValue) cuidador.fechaInicioExperiencia = dto.fechaInicioExperiencia.Value;
            if (dto.direccion is not null) cuidador.direccion = dto.direccion;
            if (dto.activo.HasValue) cuidador.activo = dto.activo.Value;
        }

        public static CuidadorDto ToResponseDto(this Cuidador cuidador)
        {
            return new CuidadorDto
            {
                Id = cuidador.Id,
                nombre = cuidador.nombre,
                telefono = cuidador.telefono,
                email = cuidador.email,
                fechaInicioExperiencia = cuidador.fechaInicioExperiencia,
                direccion = cuidador.direccion,
                activo = cuidador.activo
            };
        }
    }
}
