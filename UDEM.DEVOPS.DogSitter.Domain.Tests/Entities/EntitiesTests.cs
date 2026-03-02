using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;

namespace UDEM.DEVOPS.DogSitter.Domain.Tests;

public class VoterEntityTests
{
    [Fact]
    public void Voter_WithValidDocument_ShouldCreateSuccessfully()
    {
        //Arrange & Act
        var voter = new Voter("12345678", DateTime.Now.AddYears(-20), "COLOMBIA");

        //Assert
        Assert.Equal("12345678", voter.Nid);
        Assert.Equal("COLOMBIA", voter.Origin);
    }

    [Fact]
    public void Voter_WithShortDocument_ThrowsCoreBusinessException()
    {
        //Arrange & Act & Assert
        var exception = Assert.Throws<CoreBusinessException>(
            () => new Voter("1234567", DateTime.Now.AddYears(-20), "COLOMBIA"));
        Assert.Equal("the document requires at least 8 chars", exception.Message);
    }

    [Fact]
    public void Voter_IsUnderAge_WhenUnder18_ShouldReturnTrue()
    {
        //Arrange
        var voter = new Voter("12345678", DateTime.Now.AddYears(-17), "COLOMBIA");

        //Act & Assert
        Assert.True(voter.IsUnderAge);
    }

    [Fact]
    public void Voter_IsUnderAge_WhenExactly18_ShouldReturnFalse()
    {
        //Arrange
        var voter = new Voter("12345678", DateTime.Now.AddYears(-18), "COLOMBIA");

        //Act & Assert
        Assert.False(voter.IsUnderAge);
    }

    [Fact]
    public void Voter_IsUnderAge_WhenOver18_ShouldReturnFalse()
    {
        //Arrange
        var voter = new Voter("12345678", DateTime.Now.AddYears(-25), "COLOMBIA");

        //Act & Assert
        Assert.False(voter.IsUnderAge);
    }

    [Fact]
    public void Voter_CanVoteBasedOnLocation_WhenColombia_ShouldReturnTrue()
    {
        //Arrange
        var voter = new Voter("12345678", DateTime.Now.AddYears(-20), "COLOMBIA");

        //Act & Assert
        Assert.True(voter.CanVoteBasedOnLocation);
    }

    [Fact]
    public void Voter_CanVoteBasedOnLocation_WhenColombiaLowerCase_ShouldReturnTrue()
    {
        //Arrange
        var voter = new Voter("12345678", DateTime.Now.AddYears(-20), "colombia");

        //Act & Assert
        Assert.True(voter.CanVoteBasedOnLocation);
    }

    [Fact]
    public void Voter_CanVoteBasedOnLocation_WhenNotColombia_ShouldReturnFalse()
    {
        //Arrange
        var voter = new Voter("12345678", DateTime.Now.AddYears(-20), "USA");

        //Act & Assert
        Assert.False(voter.CanVoteBasedOnLocation);
    }

    [Fact]
    public void Voter_ShouldInheritFromDomainEntity()
    {
        //Arrange & Act
        var voter = new Voter("12345678", DateTime.Now.AddYears(-20), "COLOMBIA");

        //Assert
        Assert.IsAssignableFrom<DomainEntity>(voter);
        Assert.IsType<Guid>(voter.Id);
    }
}

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
        Assert.Equal(id, entity.Id);
    }

    [Fact]
    public void DomainEntity_Id_DefaultShouldBeEmptyGuid()
    {
        //Arrange & Act
        var entity = new DomainEntity();

        //Assert
        Assert.Equal(Guid.Empty, entity.Id);
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
