using UDEM.DEVOPS.DogSitter.Application.Voters;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;
using AutoFixture;
using AutoFixture.AutoMoq;
using Xunit;

namespace UDEM.DEVOPS.DogSitter.Api.Tests;

[Collection("VoterV1Collection collection")]
public class VoterApiTest
{
    readonly ApiApp _builder;
    readonly IFixture _fixture;
    readonly HttpClient _client;

    public VoterApiTest(ApiApp apiApp)
    {
        _builder = apiApp;
        _ = _builder.GetServiceCollectionAsync();
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _client = _builder.CreateClient();
    }

    [Fact]
    public async Task GetSingleVoterTestSuccess()
    {
        var idVoter = _builder._voter.Id;

        ApiApp.AgregarTokenASolicitud(_client, ApiApp.TOKEN_VALIDO_PERMISOS);
        var singleVoter = await _client.GetFromJsonAsync<VoterDto>($"/api/voter/{idVoter}");

        Assert.NotNull(singleVoter);
        Assert.Equal(singleVoter.Id, idVoter);
    }


    [Fact]
    public async Task PostVoterTestSuccess()
    {
        var voterCommand = _fixture.Build<VoterRegisterCommand>()
                                      .With(voter => voter.Nid, _builder._voter.Nid)
                                      .With(voter => voter.Origin, _builder._voter.Origin)
                                      .With(voter => voter.Dob, _builder._voter.DateOfBirth)
                                      .Create();
        ApiApp.AgregarTokenASolicitud(_client, ApiApp.TOKEN_VALIDO_PERMISOS);
        var request = await _client.PostAsJsonAsync("/api/voter/", voterCommand);
        request.EnsureSuccessStatusCode();

        var responseData = System.Text.Json.JsonSerializer.Deserialize<Guid>(await request.Content.ReadAsStringAsync());
        Assert.IsType<Guid>(responseData);
    }

    [Fact]
    public async Task PostClientsFailureByAge()
    {
        HttpResponseMessage request = default!;
        try
        {
            var voterCommand = _fixture.Build<VoterRegisterCommand>()
                                     .With(voter => voter.Nid, _builder._voter.Nid)
                                     .With(voter => voter.Origin, _builder._voter.Origin)
                                     .With(voter => voter.Dob, DateTime.Now.AddYears(-16))
                                     .Create();
            ApiApp.AgregarTokenASolicitud(_client, ApiApp.TOKEN_VALIDO_PERMISOS);

            request = await _client.PostAsJsonAsync("/api/voter/", voterCommand);
            request.EnsureSuccessStatusCode();
            Assert.Fail("There's no way to get here if voter is underage");
        }
        catch (Exception)
        {
            var responseMessage = await request.Content.ReadAsStringAsync();
            Assert.True(request.StatusCode is HttpStatusCode.BadRequest && responseMessage.Contains("The voter is underaged"));
        }
    }
}