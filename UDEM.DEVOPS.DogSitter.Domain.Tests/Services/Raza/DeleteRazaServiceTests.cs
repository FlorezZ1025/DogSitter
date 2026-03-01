using NSubstitute;
using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Raza;
using Entities = UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Domain.Tests.Services.Raza;

public class DeleteRazaServiceTests
{
    readonly IRazaRepository _repository;
    readonly DeleteRazaService _service;

    public DeleteRazaServiceTests()
    {
        _repository = Substitute.For<IRazaRepository>();
        _service = new DeleteRazaService(_repository);
    }

    [Fact]
    public async Task DeleteRazaAsync_WhenRazaNotFound_ThrowsNotFoundEntityException()
    {
        //Arrange
        var id = Guid.NewGuid();
        _repository.GetRazaAsync(id).Returns((Entities.Raza?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityException>(
            async () => await _service.DeleteRazaAsync(id));

        //Assert
        Assert.Equal("No se encontró la raza a eliminar", exception.Message);
        await _repository.Received(0).DeleteRazaAsync(Arg.Any<Guid>());
    }

    [Fact]
    public async Task DeleteRazaAsync_WhenRazaHasPerros_ThrowsDeleteRestrictionException()
    {
        //Arrange
        var id = Guid.NewGuid();
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
        var raza = new Entities.Raza
        {
            Id = id,
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta",
            perros = new List<Entities.Perro>
            {
                new Entities.Perro { Id = Guid.NewGuid(), nombre = "Rex", edad = 3, peso = 25, raza = null!, razaId = id, cuidador = cuidador, cuidadorId = cuidador.Id, tipoComida = "Seca", horarioComida = "8AM" }
            }
        };
        _repository.GetRazaAsync(id).Returns(raza);

        //Act
        var exception = await Assert.ThrowsAsync<DeleteRestrictionException>(
            async () => await _service.DeleteRazaAsync(id));

        //Assert
        Assert.Equal("No se puede eliminar la raza porque tiene perros asociados", exception.Message);
        await _repository.Received(0).DeleteRazaAsync(Arg.Any<Guid>());
    }

    [Fact]
    public async Task DeleteRazaAsync_WhenRazaHasNoPerros_ShouldDelete()
    {
        //Arrange
        var id = Guid.NewGuid();
        var raza = new Entities.Raza
        {
            Id = id,
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta",
            perros = new List<Entities.Perro>()
        };
        _repository.GetRazaAsync(id).Returns(raza);
        _repository.DeleteRazaAsync(id).Returns(true);

        //Act
        await _service.DeleteRazaAsync(id);

        //Assert
        await _repository.Received(1).DeleteRazaAsync(id);
    }

    [Fact]
    public async Task CheckIfRazaExists_WhenExists_ShouldReturnRaza()
    {
        //Arrange
        var id = Guid.NewGuid();
        var raza = new Entities.Raza
        {
            Id = id,
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta"
        };
        _repository.GetRazaAsync(id).Returns(raza);

        //Act
        var result = await _service.CheckIfRazaExists(id);

        //Assert
        Assert.Equal(raza, result);
    }

    [Fact]
    public async Task CheckIfRazaHasPerrosAlready_WhenNullPerros_ShouldNotThrow()
    {
        //Arrange
        var raza = new Entities.Raza
        {
            Id = Guid.NewGuid(),
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta",
            perros = null!
        };

        //Act & Assert
        await _service.CheckIfRazaHasPerrosAlready(raza);
    }
}
