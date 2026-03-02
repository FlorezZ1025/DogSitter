using UDEM.DEVOPS.DogSitter.Domain.Entities;
using UDEM.DEVOPS.DogSitter.Domain.Exceptions;
using UDEM.DEVOPS.DogSitter.Domain.Ports;
using UDEM.DEVOPS.DogSitter.Domain.Services;
using NSubstitute;

namespace UDEM.DEVOPS.DogSitter.Domain.Tests;

public class RecordVoterTests
{
    readonly IVoterRepository _repository;
    readonly RecordVoterService _service;

    public RecordVoterTests()
    {
        _repository = Substitute.For<IVoterRepository>();
        _service = new RecordVoterService(_repository);
    }

    [Fact]
    public void RecordVoterAsync_WithWrongDocument_ThrowsUnderAgeException()
    {
        //Arrange
        const string MESSAGE_EXCEPTION = "the document requires at least 8 chars";

        //Act
        CoreBusinessException exception = Assert.Throws<CoreBusinessException>(() => new Voter("123456", DateTime.Now.AddYears(-17), "COLOMBIA"));

        //Assert
        Assert.Equal(MESSAGE_EXCEPTION, exception.Message);
    }

    [Fact]
    public async Task RecordVoterAsync_WhenVoterIsUnderAge_ThrowsUnderAgeException()
    {
        //Arrange
        const string MESSAGE_EXCEPTION = "The voter is underaged";
        Voter voter = new("12345678", DateTime.Now.AddYears(-17), "COLOMBIA");

        //Act
        UnderAgeException exception = await Assert.ThrowsAsync<UnderAgeException>(async () => await _service.RecordVoterAsync(voter));

        //Assert
        Assert.Equal(MESSAGE_EXCEPTION, exception.Message);
        Assert.True(_repository.Received(requiredNumberOfCalls: 0).SaveVoterAsync(voter).IsCompleted);
    }

    [Fact]
    public async Task RecordVoterAsync_WhenVoterIsFromWrongCountry_ThrowsLocationNotAllowedException()
    {
        //Arrange
        const string VOTER_ORIGIN = "USA";
        const string MESSAGE_EXCEPTION = $"The voter is not allowed to vote in this location {VOTER_ORIGIN}";
        Voter voter = new("12345678", DateTime.Now.AddYears(-18), VOTER_ORIGIN);

        //Act
        LocationNotAllowedException exception = await Assert.ThrowsAsync<LocationNotAllowedException>(async () => await _service.RecordVoterAsync(voter));

        //Assert
        Assert.Equal(MESSAGE_EXCEPTION, exception.Message);
        Assert.True(_repository.Received(requiredNumberOfCalls: 0).SaveVoterAsync(voter).IsCompleted);
    }

    [Fact]
    public async Task RecordVoterAsync_WhenVoterIsOver18AndCorrectCountry_ShouldRecordvoter()
    {
        //Arrange
        Voter voter = new("12345678", DateTime.Now.AddYears(-18), "Colombia");
        _repository.SaveVoterAsync(Arg.Any<Voter>()).Returns(voter);

        //Act
        await _service.RecordVoterAsync(voter);

        //Assert
        Assert.IsType<Guid>(voter.Id);
        Assert.True(_repository.Received(requiredNumberOfCalls: 1).SaveVoterAsync(voter).IsCompleted);
        Assert.True(_repository.Received(requiredNumberOfCalls: 1).ExistsAsync(Arg.Any<Guid>()).IsCompleted);
    }

    [Fact]
    public async Task RecordVoterAsync_WhenVoterExist()
    {

        //Arrange
        string MESSAGE_EXCEPTION = "The voter already exists";
        Voter voter = new("12345678", DateTime.Now.AddYears(-18), "Colombia");
        _repository.ExistsAsync(Arg.Any<Guid>()).Returns(voter);

        //Act
        DuplicatedEntityException exception = await Assert.ThrowsAsync<DuplicatedEntityException>(async () => await _service.RecordVoterAsync(voter));

        //Assert
        await _repository.Received(requiredNumberOfCalls: 1).ExistsAsync(Arg.Any<Guid>());
        Assert.Equal(MESSAGE_EXCEPTION, exception.Message);
    }
}
