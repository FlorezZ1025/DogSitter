using UDEM.DEVOPS.DogSitter.Domain.Exceptions;

namespace UDEM.DEVOPS.DogSitter.Domain.Tests;

public class ExceptionsTests
{
    [Fact]
    public void CoreBusinessException_WithMessage_ShouldSetMessage()
    {
        //Arrange & Act
        var exception = new CoreBusinessException("Test message");

        //Assert
        Assert.Equal("Test message", exception.Message);
    }

    [Fact]
    public void CoreBusinessException_WithMessageAndInner_ShouldSetBoth()
    {
        //Arrange
        var inner = new Exception("Inner exception");

        //Act
        var exception = new CoreBusinessException("Test message", inner);

        //Assert
        Assert.Equal("Test message", exception.Message);
        Assert.Equal(inner, exception.InnerException);
    }

    [Fact]
    public void UnderAgeException_WithMessage_ShouldSetMessage()
    {
        //Arrange & Act
        var exception = new UnderAgeException("Under age");

        //Assert
        Assert.Equal("Under age", exception.Message);
        Assert.IsAssignableFrom<CoreBusinessException>(exception);
    }

    [Fact]
    public void UnderAgeException_WithMessageAndInner_ShouldSetBoth()
    {
        //Arrange
        var inner = new Exception("Inner");

        //Act
        var exception = new UnderAgeException("Under age", inner);

        //Assert
        Assert.Equal("Under age", exception.Message);
        Assert.Equal(inner, exception.InnerException);
    }

    [Fact]
    public void DuplicatedEntityException_WithMessage_ShouldSetMessage()
    {
        //Arrange & Act
        var exception = new DuplicatedEntityException("Duplicated");

        //Assert
        Assert.Equal("Duplicated", exception.Message);
        Assert.IsAssignableFrom<CoreBusinessException>(exception);
    }

    [Fact]
    public void DuplicatedEntityException_WithMessageAndInner_ShouldSetBoth()
    {
        //Arrange
        var inner = new Exception("Inner");

        //Act
        var exception = new DuplicatedEntityException("Duplicated", inner);

        //Assert
        Assert.Equal("Duplicated", exception.Message);
        Assert.Equal(inner, exception.InnerException);
    }

    [Fact]
    public void DuplicatedEmailException_WithMessage_ShouldSetMessage()
    {
        //Arrange & Act
        var exception = new DuplicatedEmailException("Email duplicado");

        //Assert
        Assert.Equal("Email duplicado", exception.Message);
        Assert.IsAssignableFrom<CoreBusinessException>(exception);
    }

    [Fact]
    public void DuplicatedEmailException_WithMessageAndInner_ShouldSetBoth()
    {
        //Arrange
        var inner = new Exception("Inner");

        //Act
        var exception = new DuplicatedEmailException("Email duplicado", inner);

        //Assert
        Assert.Equal("Email duplicado", exception.Message);
        Assert.Equal(inner, exception.InnerException);
    }

    [Fact]
    public void LocationNotAllowedException_WithMessage_ShouldSetMessage()
    {
        //Arrange & Act
        var exception = new LocationNotAllowedException("Location not allowed");

        //Assert
        Assert.Equal("Location not allowed", exception.Message);
        Assert.IsAssignableFrom<CoreBusinessException>(exception);
    }

    [Fact]
    public void LocationNotAllowedException_WithMessageAndInner_ShouldSetBoth()
    {
        //Arrange
        var inner = new Exception("Inner");

        //Act
        var exception = new LocationNotAllowedException("Location not allowed", inner);

        //Assert
        Assert.Equal("Location not allowed", exception.Message);
        Assert.Equal(inner, exception.InnerException);
    }

    [Fact]
    public void NotFoundEntityException_WithMessage_ShouldSetMessage()
    {
        //Arrange & Act
        var exception = new NotFoundEntityException("Not found");

        //Assert
        Assert.Equal("Not found", exception.Message);
        Assert.IsAssignableFrom<CoreBusinessException>(exception);
    }

    [Fact]
    public void NotFoundEntityException_WithMessageAndInner_ShouldSetBoth()
    {
        //Arrange
        var inner = new Exception("Inner");

        //Act
        var exception = new NotFoundEntityException("Not found", inner);

        //Assert
        Assert.Equal("Not found", exception.Message);
        Assert.Equal(inner, exception.InnerException);
    }

    [Fact]
    public void DeleteRestrictionException_WithMessage_ShouldSetMessage()
    {
        //Arrange & Act
        var exception = new DeleteRestrictionException("Cannot delete");

        //Assert
        Assert.Equal("Cannot delete", exception.Message);
        Assert.IsAssignableFrom<CoreBusinessException>(exception);
    }

    [Fact]
    public void DeleteRestrictionException_WithMessageAndInner_ShouldSetBoth()
    {
        //Arrange
        var inner = new Exception("Inner");

        //Act
        var exception = new DeleteRestrictionException("Cannot delete", inner);

        //Assert
        Assert.Equal("Cannot delete", exception.Message);
        Assert.Equal(inner, exception.InnerException);
    }

    [Fact]
    public void DuplicatedNombreRazaException_WithMessage_ShouldSetMessage()
    {
        //Arrange & Act
        var exception = new DuplicatedNombreRazaException("Nombre duplicado");

        //Assert
        Assert.Equal("Nombre duplicado", exception.Message);
        Assert.IsAssignableFrom<CoreBusinessException>(exception);
    }

    [Fact]
    public void DuplicatedNombreRazaException_WithMessageAndInner_ShouldSetBoth()
    {
        //Arrange
        var inner = new Exception("Inner");

        //Act
        var exception = new DuplicatedNombreRazaException("Nombre duplicado", inner);

        //Assert
        Assert.Equal("Nombre duplicado", exception.Message);
        Assert.Equal(inner, exception.InnerException);
    }
}
