using NSubstitute;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Perro;
using Entities = UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Domain.Tests.Services.Perro;

public class EditPerroServiceTests
{
    readonly IPerroRepository _perroRepository;
    readonly IRazaRepository _razaRepository;
    readonly ICuidadorRepository _cuidadorRepository;
    readonly EditPerroService _service;

    public EditPerroServiceTests()
    {
        _perroRepository = Substitute.For<IPerroRepository>();
        _razaRepository = Substitute.For<IRazaRepository>();
        _cuidadorRepository = Substitute.For<ICuidadorRepository>();
        _service = new EditPerroService(_perroRepository, _razaRepository, _cuidadorRepository);
    }

    [Fact]
    public async Task EditPerroAsync_WhenPerroNotFound_ThrowsNotFoundEntityException()
    {
        //Arrange
        var dto = new UpdatePerroDto { Id = Guid.NewGuid(), nombre = "Nuevo Nombre" };
        _perroRepository.GetPerroAsync(dto.Id).Returns((Entities.Perro?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityException>(
            async () => await _service.EditPerroAsync(dto));

        //Assert
        Assert.Equal("No se encontró el perro a editar", exception.Message);
    }

    [Fact]
    public async Task EditPerroAsync_WhenRazaNotFound_ThrowsNotFoundEntityException()
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
        var entity = new Entities.Perro
        {
            Id = Guid.NewGuid(), nombre = "Rex", edad = 3, peso = 25,
            razaId = razaId, raza = raza,
            cuidadorId = cuidadorId, cuidador = cuidador,
            tipoComida = "Seca", horarioComida = "8AM"
        };
        var newRazaId = Guid.NewGuid();
        var dto = new UpdatePerroDto { Id = entity.Id, razaId = newRazaId };
        _perroRepository.GetPerroAsync(entity.Id).Returns(entity);
        _razaRepository.GetRazaAsync(newRazaId).Returns((Entities.Raza?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityException>(
            async () => await _service.EditPerroAsync(dto));

        //Assert
        Assert.Equal($"La raza con id {newRazaId} no existe en el sistema", exception.Message);
    }

    [Fact]
    public async Task EditPerroAsync_WhenCuidadorNotFound_ThrowsNotFoundEntityException()
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
        var entity = new Entities.Perro
        {
            Id = Guid.NewGuid(), nombre = "Rex", edad = 3, peso = 25,
            razaId = razaId, raza = raza,
            cuidadorId = cuidadorId, cuidador = cuidador,
            tipoComida = "Seca", horarioComida = "8AM"
        };
        var newCuidadorId = Guid.NewGuid();
        var dto = new UpdatePerroDto { Id = entity.Id, cuidadorId = newCuidadorId };
        _perroRepository.GetPerroAsync(entity.Id).Returns(entity);
        _razaRepository.GetRazaAsync(razaId).Returns(raza);
        _cuidadorRepository.GetCuidadorAsync(newCuidadorId).Returns((Entities.Cuidador?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityException>(
            async () => await _service.EditPerroAsync(dto));

        //Assert
        Assert.Equal($"El cuidador con id {newCuidadorId} no existe en el sistema", exception.Message);
    }

    [Fact]
    public async Task EditPerroAsync_WhenValid_ShouldEditAndReturnDto()
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
        var entity = new Entities.Perro
        {
            Id = Guid.NewGuid(), nombre = "Rex", edad = 3, peso = 25,
            razaId = razaId, raza = raza,
            cuidadorId = cuidadorId, cuidador = cuidador,
            tipoComida = "Seca", horarioComida = "8AM"
        };
        var dto = new UpdatePerroDto { Id = entity.Id, nombre = "Rex Editado" };
        _perroRepository.GetPerroAsync(entity.Id).Returns(entity);
        _razaRepository.GetRazaAsync(razaId).Returns(raza);
        _cuidadorRepository.GetCuidadorAsync(cuidadorId).Returns(cuidador);
        _perroRepository.EditPerroAsync(Arg.Any<Entities.Perro>()).Returns(entity);

        //Act
        var result = await _service.EditPerroAsync(dto);

        //Assert
        Assert.Equal(entity.Id, result.Id);
        Assert.Equal("Rex Editado", result.nombre);
        await _perroRepository.Received(1).EditPerroAsync(Arg.Any<Entities.Perro>());
    }
}
