using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDEM.DEVOPS.DogSitter.Domain.Mappings
{
    public static class PerroMappings
    {
        public static Perro ToEntity(this CreatePerroDto dto)
        {
            return new Perro
            {
                Id = Guid.NewGuid(),
                nombre = dto.nombre,
                edad = dto.edad,
                peso = dto.peso,
                razaId = dto.razaId,
                raza = null!,
                cuidadorId = dto.cuidadorId,
                cuidador = null!,
                tipoComida = dto.tipoComida,
                horarioComida = dto.horarioComida,
                alergias = dto.alergias,
                observaciones = dto.observaciones
            };
        }

        public static void UpdateEntity(this Perro perro, UpdatePerroDto dto)
        {
            if (dto.nombre is not null) perro.nombre = dto.nombre;
            if (dto.edad.HasValue) perro.edad = dto.edad.Value;
            if (dto.peso.HasValue) perro.peso = dto.peso.Value;
            if (dto.razaId.HasValue) perro.razaId = dto.razaId.Value;
            if (dto.cuidadorId.HasValue) perro.cuidadorId = dto.cuidadorId.Value;
            if (dto.tipoComida is not null) perro.tipoComida = dto.tipoComida;
            if (dto.horarioComida is not null) perro.horarioComida = dto.horarioComida;
            if (dto.alergias is not null) perro.alergias = dto.alergias;
            if (dto.observaciones is not null) perro.observaciones = dto.observaciones;
        }

        public static PerroDto ToResponseDto(this Perro perro)
        {
            return new PerroDto
            {
                Id = perro.Id,
                nombre = perro.nombre,
                edad = perro.edad,
                peso = perro.peso,
                razaId = perro.razaId,
                cuidadorId = perro.cuidadorId,
                tipoComida = perro.tipoComida,
                horarioComida = perro.horarioComida,
                alergias = perro.alergias,
                observaciones = perro.observaciones
            };
        }
    }
}
