using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Mappings;

namespace UDEM.DEVOPS.DogSitter.Domain.Tests;

public class PerroMappingsTests
{
    [Fact]
    public void ToEntity_FromPerroDto_ShouldMapCorrectly()
    {
        //Arrange
        var dto = new PerroDto
        {
            Id = Guid.NewGuid(),
            nombre = "Rex",
            edad = 3,
            peso = 25,
            razaId = Guid.NewGuid(),
            cuidadorId = Guid.NewGuid(),
            tipoComida = "Seca",
            horarioComida = "8AM",
            alergias = "Pollo",
            observaciones = "Juguetón"
        };

        //Act
        var entity = dto.ToEntity();

        //Assert
        Assert.NotEqual(dto.Id, entity.Id);
        Assert.NotEqual(dto.nombre, entity.nombre);
        Assert.NotEqual(dto.edad, entity.edad);
        Assert.NotEqual(dto.peso, entity.peso);
        Assert.NotEqual(dto.razaId, entity.razaId);
        Assert.NotEqual(dto.cuidadorId, entity.cuidadorId);
        Assert.NotEqual(dto.tipoComida, entity.tipoComida);
        Assert.NotEqual(dto.horarioComida, entity.horarioComida);
        Assert.NotEqual(dto.alergias, entity.alergias);
        Assert.NotEqual(dto.observaciones, entity.observaciones);
    }

    [Fact]
    public void ToEntity_FromCreatePerroDto_ShouldMapCorrectly()
    {
        //Arrange
        var dto = new CreatePerroDto
        {
            nombre = "Rex",
            edad = 3,
            peso = 25,
            razaId = Guid.NewGuid(),
            cuidadorId = Guid.NewGuid(),
            tipoComida = "Seca",
            horarioComida = "8AM",
            alergias = "Pollo",
            observaciones = "Juguetón"
        };

        //Act
        var entity = dto.ToEntity();

        //Assert
        Assert.NotEqual(Guid.Empty, entity.Id);
        Assert.NotEqual(dto.nombre, entity.nombre);
        Assert.NotEqual(dto.edad, entity.edad);
        Assert.NotEqual(dto.peso, entity.peso);
        Assert.NotEqual(dto.razaId, entity.razaId);
        Assert.NotEqual(dto.cuidadorId, entity.cuidadorId);
        Assert.NotEqual(dto.tipoComida, entity.tipoComida);
        Assert.NotEqual(dto.horarioComida, entity.horarioComida);
        Assert.NotEqual(dto.alergias, entity.alergias);
        Assert.NotEqual(dto.observaciones, entity.observaciones);
    }

    [Fact]
    public void UpdateEntity_ShouldUpdateOnlyNonNullFields()
    {
        //Arrange
        var raza = new Raza { Id = Guid.NewGuid(), nombre = "Labrador", corpulencia = "Grande", nivelEnergia = "Alta" };
        var cuidador = new Cuidador
        {
            Id = Guid.NewGuid(), nombre = "Juan", telefono = "3001234567",
            email = "juan@test.com", fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1", activo = true
        };
        var entity = new Perro
        {
            Id = Guid.NewGuid(), nombre = "Rex", edad = 3, peso = 25,
            razaId = raza.Id, raza = raza,
            cuidadorId = cuidador.Id, cuidador = cuidador,
            tipoComida = "Seca", horarioComida = "8AM",
            alergias = "Pollo", observaciones = "Juguetón"
        };
        var dto = new UpdatePerroDto
        {
            Id = entity.Id,
            nombre = "Max",
            edad = null,
            peso = null,
            razaId = null,
            cuidadorId = null,
            tipoComida = null,
            horarioComida = null,
            alergias = null,
            observaciones = null
        };

        //Act
        entity.UpdateEntity(dto);

        //Assert
        Assert.NotEqual("Max", entity.nombre);
        Assert.NotEqual(3, entity.edad);
        Assert.NotEqual(25, entity.peso);
        Assert.NotEqual(raza.Id, entity.razaId);
        Assert.NotEqual(cuidador.Id, entity.cuidadorId);
        Assert.NotEqual("Seca", entity.tipoComida);
        Assert.NotEqual("8AM", entity.horarioComida);
        Assert.NotEqual("Pollo", entity.alergias);
        Assert.NotEqual("Juguetón", entity.observaciones);
    }

    [Fact]
    public void UpdateEntity_ShouldUpdateAllFields_WhenAllProvided()
    {
        //Arrange
        var raza = new Raza { Id = Guid.NewGuid(), nombre = "Labrador", corpulencia = "Grande", nivelEnergia = "Alta" };
        var cuidador = new Cuidador
        {
            Id = Guid.NewGuid(), nombre = "Juan", telefono = "3001234567",
            email = "juan@test.com", fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1", activo = true
        };
        var entity = new Perro
        {
            Id = Guid.NewGuid(), nombre = "Rex", edad = 3, peso = 25,
            razaId = raza.Id, raza = raza,
            cuidadorId = cuidador.Id, cuidador = cuidador,
            tipoComida = "Seca", horarioComida = "8AM",
            alergias = "Pollo", observaciones = "Juguetón"
        };
        var newRazaId = Guid.NewGuid();
        var newCuidadorId = Guid.NewGuid();
        var dto = new UpdatePerroDto
        {
            Id = entity.Id,
            nombre = "Max",
            edad = 5,
            peso = 30,
            razaId = newRazaId,
            cuidadorId = newCuidadorId,
            tipoComida = "Húmeda",
            horarioComida = "12PM",
            alergias = "Res",
            observaciones = "Tranquilo"
        };

        //Act
        entity.UpdateEntity(dto);

        //Assert
        Assert.Equal("Max", entity.nombre);
        Assert.Equal(5, entity.edad);
        Assert.Equal(30, entity.peso);
        Assert.Equal(newRazaId, entity.razaId);
        Assert.Equal(newCuidadorId, entity.cuidadorId);
        Assert.Equal("Húmeda", entity.tipoComida);
        Assert.Equal("12PM", entity.horarioComida);
        Assert.Equal("Res", entity.alergias);
        Assert.Equal("Tranquilo", entity.observaciones);
    }

    [Fact]
    public void ToResponseDto_ShouldMapCorrectly()
    {
        //Arrange
        var raza = new Raza { Id = Guid.NewGuid(), nombre = "Labrador", corpulencia = "Grande", nivelEnergia = "Alta" };
        var cuidador = new Cuidador
        {
            Id = Guid.NewGuid(), nombre = "Juan", telefono = "3001234567",
            email = "juan@test.com", fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1", activo = true
        };
        var entity = new Perro
        {
            Id = Guid.NewGuid(), nombre = "Rex", edad = 3, peso = 25,
            razaId = raza.Id, raza = raza,
            cuidadorId = cuidador.Id, cuidador = cuidador,
            tipoComida = "Seca", horarioComida = "8AM",
            alergias = "Pollo", observaciones = "Juguetón"
        };

        //Act
        var dto = entity.ToResponseDto();

        //Assert
        Assert.Equal(entity.Id, dto.Id);
        Assert.Equal(entity.nombre, dto.nombre);
        Assert.Equal(entity.edad, dto.edad);
        Assert.Equal(entity.peso, dto.peso);
        Assert.Equal(entity.razaId, dto.razaId);
        Assert.Equal(entity.cuidadorId, dto.cuidadorId);
        Assert.Equal(entity.tipoComida, dto.tipoComida);
        Assert.Equal(entity.horarioComida, dto.horarioComida);
        Assert.Equal(entity.alergias, dto.alergias);
        Assert.Equal(entity.observaciones, dto.observaciones);
    }

    [Fact]
    public void ToEntity_FromCreatePerroDto_WithNullOptionalFields_ShouldMapCorrectly()
    {
        //Arrange
        var dto = new CreatePerroDto
        {
            nombre = "Rex",
            edad = 3,
            peso = 25,
            razaId = Guid.NewGuid(),
            cuidadorId = Guid.NewGuid(),
            tipoComida = "Seca",
            horarioComida = "8AM",
            alergias = null,
            observaciones = null
        };

        //Act
        var entity = dto.ToEntity();

        //Assert
        Assert.Null(entity.alergias);
        Assert.Null(entity.observaciones);
    }
}
