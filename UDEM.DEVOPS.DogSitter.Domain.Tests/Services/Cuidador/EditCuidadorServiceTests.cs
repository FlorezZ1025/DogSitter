using NSubstitute;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Cuidador;
using Entities = UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Domain.Tests.Services.Cuidador;

public class EditCuidadorServiceTests
{
    readonly ICuidadorRepository _repository;
    readonly EditCuidadorService _service;

    public EditCuidadorServiceTests()
    {
        _repository = Substitute.For<ICuidadorRepository>();
        _service = new EditCuidadorService(_repository);
    }

    [Fact]
    public async Task EditCuidadorAsync_WhenCuidadorNotFound_ThrowsNotFoundEntityException()
    {
        //Arrange
        var dto = new UpdateCuidadorDto { Id = Guid.NewGuid(), nombre = "Nuevo Nombre" };
        _repository.GetCuidadorAsync(dto.Id).Returns((Entities.Cuidador?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityException>(
            async () => await _service.EditCuidadorAsync(dto));

        //Assert
        Assert.Equal("No se encontró el cuidador a editar", exception.Message);
    }

    [Fact]
    public async Task EditCuidadorAsync_WhenEmailDuplicated_ThrowsDuplicatedEmailException()
    {
        //Arrange
        var existingId = Guid.NewGuid();
        var otherId = Guid.NewGuid();
        var entity = new Entities.Cuidador
        {
            Id = existingId,
            nombre = "Juan",
            telefono = "3001234567",
            email = "juan@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1",
            activo = true
        };
        var otherCuidador = new Entities.Cuidador
        {
            Id = otherId,
            nombre = "Pedro",
            telefono = "3009876543",
            email = "pedro@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 2",
            activo = true
        };
        var dto = new UpdateCuidadorDto { Id = existingId, email = "pedro@test.com" };
        _repository.GetCuidadorAsync(existingId).Returns(entity);
        _repository.GetAllCuidadoresAsync().Returns(new List<Entities.Cuidador> { entity, otherCuidador });

        //Act
        var exception = await Assert.ThrowsAsync<DuplicatedEmailException>(
            async () => await _service.EditCuidadorAsync(dto));

        //Assert
        Assert.Equal($"El email {dto.email} ya se encuentra registrado, debe registrar otro", exception.Message);
    }

    [Fact]
    public async Task EditCuidadorAsync_WhenValid_ShouldEditAndReturnDto()
    {
        //Arrange
        var id = Guid.NewGuid();
        var entity = new Entities.Cuidador
        {
            Id = id,
            nombre = "Juan",
            telefono = "3001234567",
            email = "juan@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1",
            activo = true
        };
        var dto = new UpdateCuidadorDto { Id = id, nombre = "Juan Editado" };
        _repository.GetCuidadorAsync(id).Returns(entity);
        _repository.GetAllCuidadoresAsync().Returns(new List<Entities.Cuidador> { entity });
        _repository.EditCuidadorAsync(Arg.Any<Entities.Cuidador>()).Returns(entity);

        //Act
        var result = await _service.EditCuidadorAsync(dto);

        //Assert
        Assert.Equal(id, result.Id);
        Assert.Equal("Juan Editado", result.nombre);
        await _repository.Received(1).EditCuidadorAsync(Arg.Any<Entities.Cuidador>());
    }

    [Fact]
    public async Task EditCuidadorAsync_WhenEmailIsNull_ShouldSkipEmailCheck()
    {
        //Arrange
        var id = Guid.NewGuid();
        var entity = new Entities.Cuidador
        {
            Id = id,
            nombre = "Juan",
            telefono = "3001234567",
            email = "juan@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1",
            activo = true
        };
        var dto = new UpdateCuidadorDto { Id = id, nombre = "Nuevo Nombre" };
        _repository.GetCuidadorAsync(id).Returns(entity);
        _repository.EditCuidadorAsync(Arg.Any<Entities.Cuidador>()).Returns(entity);

        //Act
        var result = await _service.EditCuidadorAsync(dto);

        //Assert
        Assert.NotNull(result);
        await _repository.Received(0).GetAllCuidadoresAsync();
        await _repository.Received(1).EditCuidadorAsync(Arg.Any<Entities.Cuidador>());
    }

    [Fact]
    public async Task EditCuidadorAsync_WhenSameEmailSameId_ShouldNotThrow()
    {
        //Arrange
        var id = Guid.NewGuid();
        var entity = new Entities.Cuidador
        {
            Id = id,
            nombre = "Juan",
            telefono = "3001234567",
            email = "juan@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1",
            activo = true
        };
        var dto = new UpdateCuidadorDto { Id = id, email = "juan@test.com" };
        _repository.GetCuidadorAsync(id).Returns(entity);
        _repository.GetAllCuidadoresAsync().Returns(new List<Entities.Cuidador> { entity });
        _repository.EditCuidadorAsync(Arg.Any<Entities.Cuidador>()).Returns(entity);

        //Act
        var result = await _service.EditCuidadorAsync(dto);

        //Assert
        Assert.NotNull(result);
        await _repository.Received(1).EditCuidadorAsync(Arg.Any<Entities.Cuidador>());
    }
}
