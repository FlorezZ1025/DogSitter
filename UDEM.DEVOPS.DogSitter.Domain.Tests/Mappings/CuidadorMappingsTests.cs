using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Mappings;

namespace UDEM.DEVOPS.DogSitter.Domain.Tests;

public class CuidadorMappingsTests
{
    [Fact]
    public void ToEntity_FromCuidadorDto_ShouldMapCorrectly()
    {
        //Arrange
        var dto = new CuidadorDto
        {
            Id = Guid.NewGuid(),
            nombre = "Juan",
            telefono = "3001234567",
            email = "juan@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1",
            activo = true
        };

        //Act
        var entity = dto.ToEntity();

        //Assert
        //FAllo
        Assert.NotEqual(Guid.Empty, dto.Id);
        Assert.Equal(dto.Id, entity.Id);
        Assert.Equal(dto.nombre, entity.nombre);
        Assert.Equal(dto.telefono, entity.telefono);
        Assert.Equal(dto.email, entity.email);
        Assert.Equal(dto.direccion, entity.direccion);
        Assert.Equal(dto.activo, entity.activo);
        Assert.Equal(DateTimeKind.Utc, entity.fechaInicioExperiencia.Kind);
    }

    [Fact]
    public void ToEntity_FromCreateCuidadorDto_ShouldMapCorrectly()
    {
        //Arrange
        var dto = new CreateCuidadorDto
        {
            nombre = "Juan",
            telefono = "3001234567",
            email = "juan@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1",
            activo = true
        };

        //Act
        var entity = dto.ToEntity();

        //Assert
        Assert.NotEqual(Guid.Empty, entity.Id);
        Assert.Equal(dto.nombre, entity.nombre);
        Assert.Equal(dto.telefono, entity.telefono);
        Assert.Equal(dto.email, entity.email);
        Assert.Equal(dto.direccion, entity.direccion);
        Assert.Equal(dto.activo, entity.activo);
        Assert.Equal(DateTimeKind.Utc, entity.fechaInicioExperiencia.Kind);
    }

    [Fact]
    public void UpdateEntity_ShouldUpdateOnlyNonNullFields()
    {
        //Arrange
        var entity = new Cuidador
        {
            Id = Guid.NewGuid(),
            nombre = "Juan",
            telefono = "3001234567",
            email = "juan@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1",
            activo = true
        };
        var dto = new UpdateCuidadorDto
        {
            Id = entity.Id,
            nombre = "Juan Editado",
            telefono = null,
            email = null,
            fechaInicioExperiencia = null,
            direccion = null,
            activo = null
        };

        //Act
        entity.UpdateEntity(dto);

        //Assert
        Assert.Equal("Juan Editado", entity.nombre);
        Assert.Equal("3001234567", entity.telefono);
        Assert.Equal("juan@test.com", entity.email);
        Assert.Equal("Calle 1", entity.direccion);
        Assert.True(entity.activo);
    }

    [Fact]
    public void UpdateEntity_ShouldUpdateAllFields_WhenAllProvided()
    {
        //Arrange
        var entity = new Cuidador
        {
            Id = Guid.NewGuid(),
            nombre = "Juan",
            telefono = "3001234567",
            email = "juan@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1",
            activo = true
        };
        var newDate = DateTime.UtcNow.AddYears(-1);
        var dto = new UpdateCuidadorDto
        {
            Id = entity.Id,
            nombre = "Pedro",
            telefono = "3009876543",
            email = "pedro@test.com",
            fechaInicioExperiencia = newDate,
            direccion = "Calle 2",
            activo = false
        };

        //Act
        entity.UpdateEntity(dto);

        //Assert
        Assert.Equal("Pedro", entity.nombre);
        Assert.Equal("3009876543", entity.telefono);
        Assert.Equal("pedro@test.com", entity.email);
        Assert.Equal("Calle 2", entity.direccion);
        Assert.False(entity.activo);
        Assert.Equal(DateTimeKind.Utc, entity.fechaInicioExperiencia.Kind);
    }

    [Fact]
    public void UpdateEntity_WhenCuidadorIsNull_ThrowsArgumentNullException()
    {
        //Arrange
        Cuidador entity = null!;
        var dto = new UpdateCuidadorDto { Id = Guid.NewGuid() };

        //Act & Assert
        Assert.Throws<ArgumentNullException>(() => entity.UpdateEntity(dto));
    }

    [Fact]
    public void UpdateEntity_WhenDtoIsNull_ThrowsArgumentNullException()
    {
        //Arrange
        var entity = new Cuidador
        {
            Id = Guid.NewGuid(),
            nombre = "Juan",
            telefono = "3001234567",
            email = "juan@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1",
            activo = true
        };

        //Act & Assert
        Assert.Throws<ArgumentNullException>(() => entity.UpdateEntity(null!));
    }

    [Fact]
    public void ToResponseDto_ShouldMapCorrectly()
    {
        //Arrange
        var entity = new Cuidador
        {
            Id = Guid.NewGuid(),
            nombre = "Juan",
            telefono = "3001234567",
            email = "juan@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1",
            activo = true
        };

        //Act
        var dto = entity.ToResponseDto();

        //Assert
        Assert.Equal(entity.Id, dto.Id);
        Assert.Equal(entity.nombre, dto.nombre);
        Assert.Equal(entity.telefono, dto.telefono);
        Assert.Equal(entity.email, dto.email);
        Assert.Equal(entity.direccion, dto.direccion);
        Assert.Equal(entity.activo, dto.activo);
    }
}
