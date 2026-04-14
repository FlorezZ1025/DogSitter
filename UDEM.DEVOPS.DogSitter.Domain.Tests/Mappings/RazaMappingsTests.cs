using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Mappings;

namespace UDEM.DEVOPS.DogSitter.Domain.Tests;

public class RazaMappingsTests
{
    [Fact]
    public void ToEntity_FromRazaDto_ShouldMapCorrectly()
    {
        //Arrange
        var dto = new RazaDto
        {
            Id = Guid.NewGuid(),
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta",
            observacionesGenerales = "Perro amigable"
        };

        //Act
        var entity = dto.ToEntity();

        //Assert
        Assert.Equal(dto.Id, entity.Id);
        Assert.Equal(dto.nombre, entity.nombre);
        Assert.Equal(dto.corpulencia, entity.corpulencia);
        Assert.Equal(dto.nivelEnergia, entity.nivelEnergia);
        Assert.Equal(dto.observacionesGenerales, entity.observacionesGenerales);
    }

    [Fact]
    public void ToEntity_FromCreateRazaDto_ShouldMapCorrectly()
    {
        //Arrange
        var dto = new CreateRazaDto
        {
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta",
            observacionesGenerales = "Perro amigable"
        };

        //Act
        var entity = dto.ToEntity();

        //Assert
        Assert.NotEqual(Guid.Empty, entity.Id);
        Assert.Equal(dto.nombre, entity.nombre);
        Assert.Equal(dto.corpulencia, entity.corpulencia);
        Assert.Equal(dto.nivelEnergia, entity.nivelEnergia);
        Assert.Equal(dto.observacionesGenerales, entity.observacionesGenerales);
    }

    [Fact]
    public void UpdateEntity_ShouldUpdateOnlyNonNullFields()
    {
        //Arrange
        var entity = new Raza
        {
            Id = Guid.NewGuid(),
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta",
            observacionesGenerales = "Perro amigable"
        };
        var dto = new UpdateRazaDto
        {
            Id = entity.Id,
            nombre = "Labrador Retriever",
            corpulencia = null,
            nivelEnergia = null,
            observacionesGenerales = null
        };

        //Act
        entity.UpdateEntity(dto);

        //Assert
        Assert.Equal("Labrador Retriever", entity.nombre);
        Assert.Equal("Grande", entity.corpulencia);
        Assert.Equal("Alta", entity.nivelEnergia);
        Assert.Equal("Perro amigable", entity.observacionesGenerales);
    }

    [Fact]
    public void UpdateEntity_ShouldUpdateAllFields_WhenAllProvided()
    {
        //Arrange
        var entity = new Raza
        {
            Id = Guid.NewGuid(),
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta",
            observacionesGenerales = "Perro amigable"
        };
        var dto = new UpdateRazaDto
        {
            Id = entity.Id,
            nombre = "Pastor Aleman",
            corpulencia = "Mediana",
            nivelEnergia = "Media",
            observacionesGenerales = "Perro guardian"
        };

        //Act
        entity.UpdateEntity(dto);

        //Assert
        Assert.Equal("Pastor Aleman", entity.nombre);
        Assert.Equal("Mediana", entity.corpulencia);
        Assert.Equal("Media", entity.nivelEnergia);
        Assert.Equal("Perro guardian", entity.observacionesGenerales);
    }

    [Fact]
    public void UpdateEntity_WhenRazaIsNull_ThrowsArgumentNullException()
    {
        //Arrange
        Raza entity = null!;
        var dto = new UpdateRazaDto { Id = Guid.NewGuid() };

        //Act & Assert
        Assert.Throws<ArgumentNullException>(() => entity.UpdateEntity(dto));
    }

    [Fact]
    public void UpdateEntity_WhenDtoIsNull_ThrowsArgumentNullException()
    {
        //Arrange
        var entity = new Raza
        {
            Id = Guid.NewGuid(),
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta"
        };

        //Act & Assert
        Assert.Throws<ArgumentNullException>(() => entity.UpdateEntity(null!));
    }

    [Fact]
    public void ToResponseDto_ShouldMapCorrectly()
    {
        //Arrange
        var entity = new Raza
        {
            Id = Guid.NewGuid(),
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta",
            observacionesGenerales = "Perro amigable"
        };

        //Act
        var dto = entity.ToResponseDto();

        //Assert
        Assert.Equal(entity.Id, dto.Id);
        Assert.Equal(entity.nombre, dto.nombre);
        Assert.Equal(entity.corpulencia, dto.corpulencia);
        Assert.Equal(entity.nivelEnergia, dto.nivelEnergia);
        Assert.Equal(entity.observacionesGenerales, dto.observacionesGenerales);
    }

    [Fact]
    public void ToEntity_FromCreateRazaDto_WithNullObservaciones_ShouldMapCorrectly()
    {
        //Arrange
        var dto = new CreateRazaDto
        {
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta",
            observacionesGenerales = null
        };

        //Act
        var entity = dto.ToEntity();

        //Assert
        Assert.Null(entity.observacionesGenerales);
    }
}
