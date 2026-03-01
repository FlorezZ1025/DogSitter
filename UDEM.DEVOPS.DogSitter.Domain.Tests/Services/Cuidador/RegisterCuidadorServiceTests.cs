using NSubstitute;
using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Cuidador;
using Entities = UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Domain.Tests.Services.Cuidador;

public class RegisterCuidadorServiceTests
{
    readonly ICuidadorRepository _repository;
    readonly RegisterCuidadorService _service;

    public RegisterCuidadorServiceTests()
    {
        _repository = Substitute.For<ICuidadorRepository>();
        _service = new RegisterCuidadorService(_repository);
    }

    [Fact]
    public async Task RegisterCuidadorAsync_WhenCuidadorIsNew_ShouldReturnGuid()
    {
        //Arrange
        var cuidador = new Entities.Cuidador
        {
            Id = Guid.NewGuid(),
            nombre = "Juan",
            telefono = "3001234567",
            email = "juan@test.com",
            fechaInicioExperiencia = DateTime.UtcNow.AddYears(-2),
            direccion = "Calle 1",
            activo = true
        };
        _repository.GetCuidadorAsync(cuidador.Id).Returns((Entities.Cuidador?)null);
        _repository.GetAllCuidadoresAsync().Returns(new List<Entities.Cuidador>());
        _repository.SaveCuidadorAsync(Arg.Any<Entities.Cuidador>()).Returns(cuidador);

        //Act
        var result = await _service.RegisterCuidadorAsync(cuidador);

        //Assert
        Assert.Equal(cuidador.Id, result);
        await _repository.Received(1).SaveCuidadorAsync(cuidador);
    }

    [Fact]
    public async Task RegisterCuidadorAsync_WhenCuidadorAlreadyExists_ThrowsDuplicatedEntityException()
    {
        //Arrange
        var cuidador = new Entities.Cuidador
        {
            Id = Guid.NewGuid(),
            nombre = "Juan",
            telefono = "3001234567",
            email = "juan@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1",
            activo = true
        };
        _repository.GetCuidadorAsync(cuidador.Id).Returns(cuidador);

        //Act
        var exception = await Assert.ThrowsAsync<DuplicatedEntityException>(
            async () => await _service.RegisterCuidadorAsync(cuidador));

        //Assert
        Assert.Equal("El cuidador ya se encuentra registrado", exception.Message);
        await _repository.Received(0).SaveCuidadorAsync(Arg.Any<Entities.Cuidador>());
    }

    [Fact]
    public async Task RegisterCuidadorAsync_WhenEmailAlreadyRegistered_ThrowsDuplicatedEmailException()
    {
        //Arrange
        var existingCuidador = new Entities.Cuidador
        {
            Id = Guid.NewGuid(),
            nombre = "Pedro",
            telefono = "3009876543",
            email = "duplicado@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 2",
            activo = true
        };
        var newCuidador = new Entities.Cuidador
        {
            Id = Guid.NewGuid(),
            nombre = "Juan",
            telefono = "3001234567",
            email = "duplicado@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1",
            activo = true
        };
        _repository.GetCuidadorAsync(newCuidador.Id).Returns((Entities.Cuidador?)null);
        _repository.GetAllCuidadoresAsync().Returns(new List<Entities.Cuidador> { existingCuidador });

        //Act
        var exception = await Assert.ThrowsAsync<DuplicatedEmailException>(
            async () => await _service.RegisterCuidadorAsync(newCuidador));

        //Assert
        Assert.Equal($"El email {newCuidador.email} se encuentra ya registrado", exception.Message);
        await _repository.Received(0).SaveCuidadorAsync(Arg.Any<Entities.Cuidador>());
    }

    [Fact]
    public async Task RegisterCuidadorAsync_WhenEmailIsUnique_ShouldRegisterSuccessfully()
    {
        //Arrange
        var existingCuidador = new Entities.Cuidador
        {
            Id = Guid.NewGuid(),
            nombre = "Pedro",
            telefono = "3009876543",
            email = "pedro@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 2",
            activo = true
        };
        var newCuidador = new Entities.Cuidador
        {
            Id = Guid.NewGuid(),
            nombre = "Juan",
            telefono = "3001234567",
            email = "juan@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1",
            activo = true
        };
        _repository.GetCuidadorAsync(newCuidador.Id).Returns((Entities.Cuidador?)null);
        _repository.GetAllCuidadoresAsync().Returns(new List<Entities.Cuidador> { existingCuidador });
        _repository.SaveCuidadorAsync(Arg.Any<Entities.Cuidador>()).Returns(newCuidador);

        //Act
        var result = await _service.RegisterCuidadorAsync(newCuidador);

        //Assert
        Assert.Equal(newCuidador.Id, result);
        await _repository.Received(1).SaveCuidadorAsync(newCuidador);
    }
}
