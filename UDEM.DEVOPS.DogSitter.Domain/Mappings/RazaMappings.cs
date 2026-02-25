using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Domain.Mappings
{
    public static class RazaMappings
    {
        public static Raza ToEntity(this CreateRazaDto dto)
        {
            return new Raza
            {
                Id = Guid.NewGuid(),
                nombre = dto.nombre,
                corpulencia = dto.corpulencia,
                nivelEnergia = dto.nivelEnergia,
                observacionesGenerales = dto.observacionesGenerales
            };
        }

        public static void UpdateEntity(this Raza raza, UpdateRazaDto dto)
        {
            if (dto.nombre is not null) raza.nombre = dto.nombre;
            if (dto.corpulencia is not null) raza.corpulencia = dto.corpulencia;
            if (dto.nivelEnergia is not null) raza.nivelEnergia = dto.nivelEnergia;
            if (dto.observacionesGenerales is not null) raza.observacionesGenerales = dto.observacionesGenerales;
        }

        public static RazaDto ToResponseDto(this Raza raza)
        {
            return new RazaDto
            {
                Id = raza.Id,
                nombre = raza.nombre,
                corpulencia = raza.corpulencia,
                nivelEnergia = raza.nivelEnergia,
                observacionesGenerales = raza.observacionesGenerales
            };
        }
    }
}
