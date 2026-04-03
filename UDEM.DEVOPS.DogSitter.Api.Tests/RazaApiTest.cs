using System.Net;
using System.Net.Http.Json;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;

namespace UDEM.DEVOPS.DogSitter.Api.Tests;

[Collection("RazaApiCollection")]
public class RazaApiTest
{
    private readonly HttpClient _client;
    private const string BasePath = "/api/v1/raza";

    public RazaApiTest(DogSitterApiApp app)
    {
        _client = app.CreateClient();
    }

    private CreateRazaDto BuildCreateDto(string? nombreOverride = null) => new()
    {
        nombre = nombreOverride ?? $"Raza-{Guid.NewGuid()}",
        corpulencia = "Grande",
        nivelEnergia = "Alta",
        observacionesGenerales = "Raza de prueba"
    };

    private async Task<RazaDto> CreateRazaViaApiAsync(string? nombre = null)
    {
        var dto = BuildCreateDto(nombre);
        var response = await _client.PostAsJsonAsync(BasePath, dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<RazaDto>())!;
    }

    [Fact]
    public async Task PostRaza_WhenValid_ShouldReturn201()
    {
        //Arrange
        var dto = BuildCreateDto();

        //Act
        var response = await _client.PostAsJsonAsync(BasePath, dto);

        //Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<RazaDto>();
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(dto.nombre, result.nombre);
        Assert.Equal(dto.corpulencia, result.corpulencia);
        Assert.Equal(dto.nivelEnergia, result.nivelEnergia);
        Assert.Equal(dto.observacionesGenerales, result.observacionesGenerales);
    }

    [Fact]
    public async Task GetRazaById_WhenExists_ShouldReturn200()
    {
        //Arrange
        var created = await CreateRazaViaApiAsync();

        //Act
        var result = await _client.GetFromJsonAsync<RazaDto>($"{BasePath}/{created.Id}");

        //Assert
        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal(created.nombre, result.nombre);
    }

    [Fact]
    public async Task GetAllRazas_ShouldReturn200()
    {
        //Arrange
        await CreateRazaViaApiAsync();

        //Act
        var response = await _client.GetAsync(BasePath);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<RazaDto>>();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task PutRaza_WhenValid_ShouldReturn200()
    {
        //Arrange
        var created = await CreateRazaViaApiAsync();
        var updateDto = new UpdateRazaDto
        {
            Id = created.Id,
            nombre = $"Raza-Editada-{Guid.NewGuid()}",
            corpulencia = "Mediana",
            nivelEnergia = created.nivelEnergia,
            observacionesGenerales = created.observacionesGenerales
        };

        //Act
        var response = await _client.PutAsJsonAsync(BasePath, updateDto);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<RazaDto>();
        Assert.NotNull(result);
        Assert.Equal(updateDto.nombre, result.nombre);
        Assert.Equal("Mediana", result.corpulencia);
    }

    [Fact]
    public async Task PatchRaza_WhenValid_ShouldReturn200()
    {
        //Arrange
        var created = await CreateRazaViaApiAsync();
        var patchDto = new UpdateRazaDto
        {
            Id = created.Id,
            nivelEnergia = "Baja"
        };

        //Act
        var response = await _client.PatchAsJsonAsync(BasePath, patchDto);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<RazaDto>();
        Assert.NotNull(result);
        Assert.Equal("Baja", result.nivelEnergia);
        Assert.Equal(created.nombre, result.nombre);
    }

    [Fact]
    public async Task DeleteRaza_WhenExists_ShouldReturn200()
    {
        //Arrange
        var created = await CreateRazaViaApiAsync();

        //Act
        var response = await _client.DeleteAsync($"{BasePath}/{created.Id}");

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostRaza_WhenDuplicateName_ShouldReturn400()
    {
        //Arrange
        var nombre = $"Raza-Duplicada-{Guid.NewGuid()}";
        await CreateRazaViaApiAsync(nombre);
        var dto = BuildCreateDto(nombre);

        //Act
        var response = await _client.PostAsJsonAsync(BasePath, dto);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains(nombre, body);
    }

    [Fact]
    public async Task GetRazaById_WhenNotFound_ShouldReturn404()
    {
        //Act
        var response = await _client.GetAsync($"{BasePath}/{Guid.NewGuid()}");

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
