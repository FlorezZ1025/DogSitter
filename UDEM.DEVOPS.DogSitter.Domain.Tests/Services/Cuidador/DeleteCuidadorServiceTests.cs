using NSubstitute;
using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Cuidador;
using Entities = UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Domain.Tests.Services.Cuidador;

public class DeleteCuidadorServiceTests
{
    readonly ICuidadorRepository _repository;
    readonly DeleteCuidadorService _service;

    public DeleteCuidadorServiceTests()
    {
        _repository = Substitute.For<ICuidadorRepository>();
        _service = new DeleteCuidadorService(_repository);
    }

    [Fact]
    public async Task DeleteCuidadorAsync_WhenCuidadorNotFound_ThrowsNotFoundEntityException()
    {
        //Arrange
        var id = Guid.NewGuid();
        _repository.GetCuidadorAsync(id).Returns((Entities.Cuidador?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityException>(
            async () => await _service.DeleteCuidadorAsync(id));

        //Assert
        Assert.Equal("No se encontró el cuidador a eliminar", exception.Message);
        await _repository.Received(0).DeleteCuidadorAsync(Arg.Any<Guid>());
    }

    [Fact]
    public async Task DeleteCuidadorAsync_WhenCuidadorHasPerros_ThrowsDeleteRestrictionException()
    {
        //Arrange
        var id = Guid.NewGuid();
        var raza = new Entities.Raza { Id = Guid.NewGuid(), nombre = "Labrador", corpulencia = "Grande", nivelEnergia = "Alta" };
        var cuidador = new Entities.Cuidador
        {
            Id = id,
            nombre = "Juan",
            telefono = "3001234567",
            email = "juan@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1",
            activo = true,
            perros = new List<Entities.Perro>
            {
                new Entities.Perro { Id = Guid.NewGuid(), nombre = "Rex", edad = 3, peso = 25, raza = raza, razaId = raza.Id, cuidador = null!, cuidadorId = id, tipoComida = "Seca", horarioComida = "8AM" }
            }
        };
        _repository.GetCuidadorAsync(id).Returns(cuidador);

        //Act
        var exception = await Assert.ThrowsAsync<DeleteRestrictionException>(
            async () => await _service.DeleteCuidadorAsync(id));

        //Assert
        //Fallo
        Assert.Equal("No se puede eliminar el cuidador porque tiene perros asociadoxxxxxxxx", exception.Message);
        await _repository.Received(0).DeleteCuidadorAsync(Arg.Any<Guid>());
    }

    [Fact]
    public async Task DeleteCuidadorAsync_WhenCuidadorHasNoPerros_ShouldDelete()
    {
        //Arrange
        var id = Guid.NewGuid();
        var cuidador = new Entities.Cuidador
        {
            Id = id,
            nombre = "Juan",
            telefono = "3001234567",
            email = "juan@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1",
            activo = true,
            perros = new List<Entities.Perro>()
        };
        _repository.GetCuidadorAsync(id).Returns(cuidador);
        
        
        //Fallo
        _repository.DeleteCuidadorAsync(id).Returns(false);

        //Act
        await _service.DeleteCuidadorAsync(id);

        //Assert
        await _repository.Received(1).DeleteCuidadorAsync(id);
    }

    [Fact]
    public async Task CheckIfCuidadorExists_WhenExists_ShouldReturnCuidador()
    {
        //Arrange
        var id = Guid.NewGuid();
        var cuidador = new Entities.Cuidador
        {
            Id = id,
            nombre = "Juan",
            telefono = "3001234567",
            email = "juan@test.com",
            fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1",
            activo = true
        };
        _repository.GetCuidadorAsync(id).Returns(cuidador);

        //Act
        var result = await _service.CheckIfCuidadorExists(id);

        //Assert
        //Fallo
        Assert.NotEqual(cuidador, result);
    }

    [Fact]
    public async Task CheckIfCuidadorHasPerrosAlready_WhenNullPerros_ShouldNotThrow()
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
            activo = true,
            perros = null!
        };

        //Act & Assert
        await _service.CheckIfCuidadorHasPerrosAlready(cuidador);
    }
}
