using System.Net;
using System.Net.Http.Json;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;

namespace UDEM.DEVOPS.DogSitter.Api.Tests;

[Collection("CuidadorApiCollection")]
public class CuidadorApiTest
{
    private readonly HttpClient _client;

    public CuidadorApiTest(DogSitterApiApp app)
    {
        _client = app.CreateClient();
    }

    private CreateCuidadorDto BuildCreateDto(string? emailOverride = null) => new()
    {
        nombre = "Test Cuidador",
        telefono = "3001234567",
        email = emailOverride ?? $"test-{Guid.NewGuid()}@example.com",
        fechaInicioExperiencia = DateTime.UtcNow.AddYears(-2),
        direccion = "Calle Test #1-2",
        activo = true
    };

    private async Task<CuidadorDto> CreateCuidadorViaApiAsync(string? email = null)
    {
        var dto = BuildCreateDto(email);
        var response = await _client.PostAsJsonAsync("/api/cuidador", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CuidadorDto>())!;
    }

    [Fact]
    public async Task PostCuidador_WhenValid_ShouldReturn201()
    {
        //Arrange
        var dto = BuildCreateDto();

        //Act
        var response = await _client.PostAsJsonAsync("/api/cuidador", dto);

        //Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<CuidadorDto>();
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(dto.nombre, result.nombre);
        Assert.Equal(dto.email, result.email);
        Assert.Equal(dto.telefono, result.telefono);
        Assert.Equal(dto.direccion, result.direccion);
        Assert.Equal(dto.activo, result.activo);
    }

    [Fact]
    public async Task GetCuidadorById_WhenExists_ShouldReturn200()
    {
        //Arrange
        var created = await CreateCuidadorViaApiAsync();

        //Act
        var result = await _client.GetFromJsonAsync<CuidadorDto>($"/api/cuidador/{created.Id}");

        //Assert
        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal(created.nombre, result.nombre);
        Assert.Equal(created.email, result.email);
    }

    [Fact]
    public async Task GetAllCuidadores_ShouldReturn200()
    {
        //Arrange
        await CreateCuidadorViaApiAsync();

        //Act
        var response = await _client.GetAsync("/api/cuidador");

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<CuidadorDto>>();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task PutCuidador_WhenValid_ShouldReturn200()
    {
        //Arrange
        var created = await CreateCuidadorViaApiAsync();
        var updateDto = new UpdateCuidadorDto
        {
            Id = created.Id,
            nombre = "Nombre Actualizado",
            email = created.email,
            telefono = created.telefono,
            direccion = created.direccion,
            activo = created.activo,
            fechaInicioExperiencia = created.fechaInicioExperiencia
        };

        //Act
        var response = await _client.PutAsJsonAsync("/api/cuidador", updateDto);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<CuidadorDto>();
        Assert.NotNull(result);
        Assert.Equal("Nombre Actualizado", result.nombre);
        Assert.Equal(created.email, result.email);
    }

    [Fact]
    public async Task PatchCuidador_WhenValid_ShouldReturn200()
    {
        //Arrange
        var created = await CreateCuidadorViaApiAsync();
        var patchDto = new UpdateCuidadorDto
        {
            Id = created.Id,
            telefono = "3109876543"
        };

        //Act
        var response = await _client.PatchAsJsonAsync("/api/cuidador", patchDto);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<CuidadorDto>();
        Assert.NotNull(result);
        Assert.Equal("3109876543", result.telefono);
        Assert.Equal(created.nombre, result.nombre);
    }

    [Fact]
    public async Task DeleteCuidador_WhenExists_ShouldReturn200()
    {
        //Arrange
        var created = await CreateCuidadorViaApiAsync();

        //Act
        var response = await _client.DeleteAsync($"/api/cuidador/{created.Id}");

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostCuidador_WhenDuplicateEmail_ShouldReturn400()
    {
        //Arrange
        var email = $"duplicate-{Guid.NewGuid()}@example.com";
        await CreateCuidadorViaApiAsync(email);
        var dto = BuildCreateDto(email);

        //Act
        var response = await _client.PostAsJsonAsync("/api/cuidador", dto);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains(email, body);
    }

    [Fact]
    public async Task GetCuidadorById_WhenNotFound_ShouldReturn404()
    {
        //Act
        var response = await _client.GetAsync($"/api/cuidador/{Guid.NewGuid()}");

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteCuidador_WhenNotFound_ShouldReturn404()
    {
        //Act
        var response = await _client.DeleteAsync($"/api/cuidador/{Guid.NewGuid()}");

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
