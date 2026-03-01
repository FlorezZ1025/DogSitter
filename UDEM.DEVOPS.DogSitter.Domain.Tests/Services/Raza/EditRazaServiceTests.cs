using NSubstitute;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services.Raza;
using Entities = UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Domain.Tests.Services.Raza;

public class EditRazaServiceTests
{
    readonly IRazaRepository _repository;
    readonly EditRazaService _service;

    public EditRazaServiceTests()
    {
        _repository = Substitute.For<IRazaRepository>();
        _service = new EditRazaService(_repository);
    }

    [Fact]
    public async Task EditRazaAsync_WhenRazaNotFound_ThrowsNotFoundEntityException()
    {
        //Arrange
        var dto = new UpdateRazaDto { Id = Guid.NewGuid(), nombre = "Nuevo Nombre" };
        _repository.GetRazaAsync(dto.Id).Returns((Entities.Raza?)null);

        //Act
        var exception = await Assert.ThrowsAsync<NotFoundEntityException>(
            async () => await _service.EditRazaAsync(dto));

        //Assert
        Assert.Equal("No se encontró la raza a editar", exception.Message);
    }

    [Fact]
    public async Task EditRazaAsync_WhenNameDuplicated_ThrowsDuplicatedNombreRazaException()
    {
        //Arrange
        var existingId = Guid.NewGuid();
        var otherId = Guid.NewGuid();
        var entity = new Entities.Raza
        {
            Id = existingId,
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta"
        };
        var otherRaza = new Entities.Raza
        {
            Id = otherId,
            nombre = "Pastor Aleman",
            corpulencia = "Grande",
            nivelEnergia = "Alta"
        };
        var dto = new UpdateRazaDto { Id = existingId, nombre = "pastor aleman" };
        _repository.GetRazaAsync(existingId).Returns(entity);
        _repository.GetAllRazasAync().Returns(new List<Entities.Raza> { entity, otherRaza });

        //Act
        var exception = await Assert.ThrowsAsync<DuplicatedNombreRazaException>(
            async () => await _service.EditRazaAsync(dto));

        //Assert
        Assert.Equal("Ya existe una raza con el mismo nombre", exception.Message);
    }

    [Fact]
    public async Task EditRazaAsync_WhenValid_ShouldEditAndReturnDto()
    {
        //Arrange
        var id = Guid.NewGuid();
        var entity = new Entities.Raza
        {
            Id = id,
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta"
        };
        var dto = new UpdateRazaDto { Id = id, nombre = "Labrador Retriever" };
        _repository.GetRazaAsync(id).Returns(entity);
        _repository.GetAllRazasAync().Returns(new List<Entities.Raza> { entity });
        _repository.EditRazaAsync(Arg.Any<Entities.Raza>()).Returns(entity);

        //Act
        var result = await _service.EditRazaAsync(dto);

        //Assert
        Assert.Equal(id, result.Id);
        Assert.Equal("Labrador Retriever", result.nombre);
        await _repository.Received(1).EditRazaAsync(Arg.Any<Entities.Raza>());
    }

    [Fact]
    public async Task EditRazaAsync_WhenSameNameSameId_ShouldNotThrow()
    {
        //Arrange
        var id = Guid.NewGuid();
        var entity = new Entities.Raza
        {
            Id = id,
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta"
        };
        var dto = new UpdateRazaDto { Id = id, nombre = "labrador" };
        _repository.GetRazaAsync(id).Returns(entity);
        _repository.GetAllRazasAync().Returns(new List<Entities.Raza> { entity });
        _repository.EditRazaAsync(Arg.Any<Entities.Raza>()).Returns(entity);

        //Act
        var result = await _service.EditRazaAsync(dto);

        //Assert
        Assert.NotNull(result);
        await _repository.Received(1).EditRazaAsync(Arg.Any<Entities.Raza>());
    }
}
