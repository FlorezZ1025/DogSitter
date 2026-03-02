using NSubstitute;
using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Perro;
using Entities = UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Domain.Tests.Services.Perro;

public class DeletePerroServiceTests
{
    readonly IPerroRepository _perroRepository;
    readonly DeletePerroService _service;

    public DeletePerroServiceTests()
    {
        _perroRepository = Substitute.For<IPerroRepository>();
        _service = new DeletePerroService(_perroRepository);
    }

    [Fact]
    public async Task DeletePerroAsync_WhenPerroNotFound_ThrowsNotFoundEntityException()
    {
        //Arrange
        var id = Guid.NewGuid();
        _perroRepository.GetPerroAsync(id).Returns((Entities.Perro?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityException>(
            async () => await _service.DeletePerroAsync(id));

        //Assert
        Assert.Equal("No se encontró el perro a eliminar", exception.Message);
        await _perroRepository.Received(0).DeletePerroAsync(Arg.Any<Guid>());
    }

    [Fact]
    public async Task DeletePerroAsync_WhenPerroExists_ShouldDelete()
    {
        //Arrange
        var id = Guid.NewGuid();
        var raza = new Entities.Raza { Id = Guid.NewGuid(), nombre = "Labrador", corpulencia = "Grande", nivelEnergia = "Alta" };
        var cuidador = new Entities.Cuidador
        {
            Id = Guid.NewGuid(), nombre = "Juan", telefono = "3001234567",
            email = "juan@test.com", fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1", activo = true
        };
        var perro = new Entities.Perro
        {
            Id = id, nombre = "Rex", edad = 3, peso = 25,
            razaId = raza.Id, raza = raza,
            cuidadorId = cuidador.Id, cuidador = cuidador,
            tipoComida = "Seca", horarioComida = "8AM"
        };
        _perroRepository.GetPerroAsync(id).Returns(perro);
        _perroRepository.DeletePerroAsync(id).Returns(true);

        //Act
        await _service.DeletePerroAsync(id);

        //Assert
        await _perroRepository.Received(1).DeletePerroAsync(id);
    }
}
