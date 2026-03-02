using NSubstitute;
using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Raza;
using Entities = UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Domain.Tests.Services.Raza;

public class RegisterRazaServiceTests
{
    readonly IRazaRepository _repository;
    readonly RegisterRazaService _service;

    public RegisterRazaServiceTests()
    {
        _repository = Substitute.For<IRazaRepository>();
        _service = new RegisterRazaService(_repository);
    }

    [Fact]
    public async Task RegisterRazaAsync_WhenRazaIsNew_ShouldReturnGuid()
    {
        //Arrange
        var raza = new Entities.Raza
        {
            Id = Guid.NewGuid(),
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta"
        };
        _repository.GetRazaAsync(raza.Id).Returns((Entities.Raza?)null);
        _repository.GetAllRazasAync().Returns(new List<Entities.Raza>());
        _repository.SaveRazaAsync(Arg.Any<Entities.Raza>()).Returns(raza);

        //Act
        var result = await _service.RegisterRazaAsync(raza);

        //Assert
        Assert.Equal(raza.Id, result);
        await _repository.Received(1).SaveRazaAsync(raza);
    }

    [Fact]
    public async Task RegisterRazaAsync_WhenRazaAlreadyExists_ThrowsDuplicatedEntityException()
    {
        //Arrange
        var raza = new Entities.Raza
        {
            Id = Guid.NewGuid(),
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta"
        };
        _repository.GetRazaAsync(raza.Id).Returns(raza);

        //Act
        var exception = await Assert.ThrowsAsync<DuplicatedEntityException>(
            async () => await _service.RegisterRazaAsync(raza));

        //Assert
        Assert.Equal("La raza ya se encuentra registrada", exception.Message);
        await _repository.Received(0).SaveRazaAsync(Arg.Any<Entities.Raza>());
    }

    [Fact]
    public async Task RegisterRazaAsync_WhenNameAlreadyRegistered_ThrowsDuplicatedEntityException()
    {
        //Arrange
        var existingRaza = new Entities.Raza
        {
            Id = Guid.NewGuid(),
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta"
        };
        var newRaza = new Entities.Raza
        {
            Id = Guid.NewGuid(),
            nombre = "Labrador",
            corpulencia = "Mediana",
            nivelEnergia = "Media"
        };
        _repository.GetRazaAsync(newRaza.Id).Returns((Entities.Raza?)null);
        _repository.GetAllRazasAync().Returns(new List<Entities.Raza> { existingRaza });

        //Act
        var exception = await Assert.ThrowsAsync<DuplicatedEntityException>(
            async () => await _service.RegisterRazaAsync(newRaza));

        //Assert
        Assert.Equal($"El nombre de raza {newRaza.nombre} se encuentra ya registrado", exception.Message);
        await _repository.Received(0).SaveRazaAsync(Arg.Any<Entities.Raza>());
    }

    [Fact]
    public async Task RegisterRazaAsync_WhenNameIsUnique_ShouldRegister()
    {
        //Arrange
        var existingRaza = new Entities.Raza
        {
            Id = Guid.NewGuid(),
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta"
        };
        var newRaza = new Entities.Raza
        {
            Id = Guid.NewGuid(),
            nombre = "Pastor Aleman",
            corpulencia = "Grande",
            nivelEnergia = "Alta"
        };
        _repository.GetRazaAsync(newRaza.Id).Returns((Entities.Raza?)null);
        _repository.GetAllRazasAync().Returns(new List<Entities.Raza> { existingRaza });
        _repository.SaveRazaAsync(Arg.Any<Entities.Raza>()).Returns(newRaza);

        //Act
        var result = await _service.RegisterRazaAsync(newRaza);

        //Assert
        Assert.Equal(newRaza.Id, result);
        await _repository.Received(1).SaveRazaAsync(newRaza);
    }
}
