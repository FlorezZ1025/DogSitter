using UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Domain.Tests;

public class DomainEntityTests
{
    [Fact]
    public void DomainEntity_Id_ShouldBeSettable()
    {
        //Arrange
        var entity = new DomainEntity();
        var id = Guid.NewGuid();

        //Act
        entity.Id = id;

        //Assert
        //Fallo
        Assert.Equal(id, Guid.Empty);
    }

    [Fact]
    public void DomainEntity_Id_DefaultShouldBeEmptyGuid()
    {
        //Arrange & Act
        var entity = new DomainEntity();

        //Assert
        //Fallo
        //Assert.Equal(Guid.Empty, entity.Id);
        Assert.NotEqual(Guid.Empty, entity.Id);
    }
}

public class CuidadorEntityTests
{
    [Fact]
    public void Cuidador_ShouldInheritFromDomainEntity()
    {
        //Arrange & Act
        var cuidador = new Cuidador
        {
            nombre = "Juan",
            telefono = "3001234567",
            email = "juan@test.com",
            direccion = "Calle 1",
            activo = true
        };

        //Assert
        Assert.IsAssignableFrom<DomainEntity>(cuidador);
    }

    [Fact]
    public void Cuidador_PerrosCollection_ShouldDefaultToEmpty()
    {
        //Arrange & Act
        var cuidador = new Cuidador
        {
            nombre = "Juan",
            telefono = "3001234567",
            email = "juan@test.com",
            direccion = "Calle 1",
            activo = true
        };

        //Assert
        Assert.Empty(cuidador.perros);
    }
}

public class PerroEntityTests
{
    [Fact]
    public void Perro_ShouldInheritFromDomainEntity()
    {
        //Arrange & Act
        var raza = new Raza { Id = Guid.NewGuid(), nombre = "Labrador", corpulencia = "Grande", nivelEnergia = "Alta" };
        var cuidador = new Cuidador
        {
            Id = Guid.NewGuid(), nombre = "Juan", telefono = "3001234567",
            email = "juan@test.com", fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1", activo = true
        };
        var perro = new Perro
        {
            nombre = "Rex", edad = 3, peso = 25,
            raza = raza, razaId = raza.Id,
            cuidador = cuidador, cuidadorId = cuidador.Id,
            tipoComida = "Seca", horarioComida = "8AM"
        };

        //Assert
        Assert.IsAssignableFrom<DomainEntity>(perro);
    }

    [Fact]
    public void Perro_OptionalFields_ShouldDefaultToNull()
    {
        //Arrange & Act
        var raza = new Raza { Id = Guid.NewGuid(), nombre = "Labrador", corpulencia = "Grande", nivelEnergia = "Alta" };
        var cuidador = new Cuidador
        {
            Id = Guid.NewGuid(), nombre = "Juan", telefono = "3001234567",
            email = "juan@test.com", fechaInicioExperiencia = DateTime.UtcNow,
            direccion = "Calle 1", activo = true
        };
        var perro = new Perro
        {
            nombre = "Rex", edad = 3, peso = 25,
            raza = raza, razaId = raza.Id,
            cuidador = cuidador, cuidadorId = cuidador.Id,
            tipoComida = "Seca", horarioComida = "8AM"
        };

        //Assert
        Assert.Null(perro.alergias);
        Assert.Null(perro.observaciones);
    }
}

public class RazaEntityTests
{
    [Fact]
    public void Raza_ShouldInheritFromDomainEntity()
    {
        //Arrange & Act
        var raza = new Raza
        {
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta"
        };

        //Assert
        Assert.IsAssignableFrom<DomainEntity>(raza);
    }

    [Fact]
    public void Raza_PerrosCollection_ShouldDefaultToEmpty()
    {
        //Arrange & Act
        var raza = new Raza
        {
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta"
        };

        //Assert
        Assert.Empty(raza.perros);
    }

    [Fact]
    public void Raza_ObservacionesGenerales_ShouldDefaultToNull()
    {
        //Arrange & Act
        var raza = new Raza
        {
            nombre = "Labrador",
            corpulencia = "Grande",
            nivelEnergia = "Alta"
        };

        //Assert
        Assert.Null(raza.observacionesGenerales);
    }
}
