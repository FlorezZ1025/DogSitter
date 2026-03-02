using NSubstitute;
using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Perro;
using Entities = UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Domain.Tests.Services.Perro;

public class RegisterPerroServiceTests
{
    readonly IPerroRepository _perroRepository;
    readonly IRazaRepository _razaRepository;
    readonly ICuidadorRepository _cuidadorRepository;
    readonly RegisterPerroService _service;

    public RegisterPerroServiceTests()
    {
        _perroRepository = Substitute.For<IPerroRepository>();
        _razaRepository = Substitute.For<IRazaRepository>();
        _cuidadorRepository = Substitute.For<ICuidadorRepository>();
        _service = new RegisterPerroService(_perroRepository, _razaRepository, _cuidadorRepository);
    }

    [Fact]
    public async Task RegisterPerroAsync_WhenPerroIsNew_ShouldReturnPerro()
    {
        //Arrange
        var razaId = Guid.NewGuid();
        var cuidadorId = Guid.NewGuid();
        var raza = new Entities.Raza { Id = razaId, nombre = "Labrador", corpulencia = "Grande", nivelEnergia = "Alta" };
        var cuidador = new Entities.Cuidador
        {
            Id = cuidadorId, nombre = "Juan", telefono = "3001234567",
            email = "juan@test.com", fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1", activo = true
        };
        var perro = new Entities.Perro
        {
            Id = Guid.NewGuid(), nombre = "Rex", edad = 3, peso = 25,
            razaId = razaId, raza = raza,
            cuidadorId = cuidadorId, cuidador = cuidador,
            tipoComida = "Seca", horarioComida = "8AM"
        };
        _perroRepository.GetPerroAsync(perro.Id).Returns((Entities.Perro?)null);
        _razaRepository.GetRazaAsync(razaId).Returns(raza);
        _cuidadorRepository.GetCuidadorAsync(cuidadorId).Returns(cuidador);
        _perroRepository.SavePerroAsync(Arg.Any<Entities.Perro>()).Returns(perro);

        //Act
        var result = await _service.RegisterPerroAsync(perro);

        //Assert
        Assert.Equal(perro.Id, result.Id);
        await _perroRepository.Received(1).SavePerroAsync(perro);
    }

    [Fact]
    public async Task RegisterPerroAsync_WhenPerroAlreadyExists_ThrowsDuplicatedEntityException()
    {
        //Arrange
        var raza = new Entities.Raza { Id = Guid.NewGuid(), nombre = "Labrador", corpulencia = "Grande", nivelEnergia = "Alta" };
        var cuidador = new Entities.Cuidador
        {
            Id = Guid.NewGuid(), nombre = "Juan", telefono = "3001234567",
            email = "juan@test.com", fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1", activo = true
        };
        var perro = new Entities.Perro
        {
            Id = Guid.NewGuid(), nombre = "Rex", edad = 3, peso = 25,
            razaId = raza.Id, raza = raza,
            cuidadorId = cuidador.Id, cuidador = cuidador,
            tipoComida = "Seca", horarioComida = "8AM"
        };
        _perroRepository.GetPerroAsync(perro.Id).Returns(perro);

        //Act
        var exception = await Assert.ThrowsAsync<DuplicatedEntityException>(
            async () => await _service.RegisterPerroAsync(perro));

        //Assert
        Assert.Equal("El perro ya se encuentra registrado", exception.Message);
        await _perroRepository.Received(0).SavePerroAsync(Arg.Any<Entities.Perro>());
    }

    [Fact]
    public async Task RegisterPerroAsync_WhenRazaNotFound_ThrowsNotFoundEntityException()
    {
        //Arrange
        var razaId = Guid.NewGuid();
        var cuidadorId = Guid.NewGuid();
        var raza = new Entities.Raza { Id = razaId, nombre = "Labrador", corpulencia = "Grande", nivelEnergia = "Alta" };
        var cuidador = new Entities.Cuidador
        {
            Id = cuidadorId, nombre = "Juan", telefono = "3001234567",
            email = "juan@test.com", fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1", activo = true
        };
        var perro = new Entities.Perro
        {
            Id = Guid.NewGuid(), nombre = "Rex", edad = 3, peso = 25,
            razaId = razaId, raza = raza,
            cuidadorId = cuidadorId, cuidador = cuidador,
            tipoComida = "Seca", horarioComida = "8AM"
        };
        _perroRepository.GetPerroAsync(perro.Id).Returns((Entities.Perro?)null);
        _razaRepository.GetRazaAsync(razaId).Returns((Entities.Raza?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityException>(
            async () => await _service.RegisterPerroAsync(perro));

        //Assert
        Assert.Equal($"La raza con id {razaId} no existe en el sistema", exception.Message);
    }

    [Fact]
    public async Task RegisterPerroAsync_WhenCuidadorNotFound_ThrowsNotFoundEntityException()
    {
        //Arrange
        var razaId = Guid.NewGuid();
        var cuidadorId = Guid.NewGuid();
        var raza = new Entities.Raza { Id = razaId, nombre = "Labrador", corpulencia = "Grande", nivelEnergia = "Alta" };
        var cuidador = new Entities.Cuidador
        {
            Id = cuidadorId, nombre = "Juan", telefono = "3001234567",
            email = "juan@test.com", fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1", activo = true
        };
        var perro = new Entities.Perro
        {
            Id = Guid.NewGuid(), nombre = "Rex", edad = 3, peso = 25,
            razaId = razaId, raza = raza,
            cuidadorId = cuidadorId, cuidador = cuidador,
            tipoComida = "Seca", horarioComida = "8AM"
        };
        _perroRepository.GetPerroAsync(perro.Id).Returns((Entities.Perro?)null);
        _razaRepository.GetRazaAsync(razaId).Returns(raza);
        _cuidadorRepository.GetCuidadorAsync(cuidadorId).Returns((Entities.Cuidador?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityException>(
            async () => await _service.RegisterPerroAsync(perro));

        //Assert
        Assert.Equal($"El cuidador con id {cuidadorId} no existe en el sistema", exception.Message);
    }
}
