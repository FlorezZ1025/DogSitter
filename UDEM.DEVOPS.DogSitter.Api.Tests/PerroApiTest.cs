using System.Net;
using System.Net.Http.Json;
using UDEM.DEVOPS.DogSitter.Domain.Dtos;

namespace UDEM.DEVOPS.DogSitter.Api.Tests;

[Collection("PerroApiCollection")]
public class PerroApiTest
{
    private readonly HttpClient _client;
    private const string PerroBasePath = "/api/v1/perro";
    private const string CuidadorBasePath = "/api/v1/cuidador";
    private const string RazaBasePath = "/api/v1/raza";

    public PerroApiTest(DogSitterApiApp app)
    {
        _client = app.CreateClient();
    }

    private async Task<CuidadorDto> CreateCuidadorViaApiAsync()
    {
        var dto = new CreateCuidadorDto
        {
            nombre = "Cuidador Perro Test",
            telefono = "3001234567",
            email = $"cuidador-{Guid.NewGuid()}@example.com",
            fechaInicioExperiencia = DateTime.UtcNow.AddYears(-2),
            direccion = "Calle Test #1-2",
            activo = true
        };
        var response = await _client.PostAsJsonAsync(CuidadorBasePath, dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<CuidadorDto>())!;
    }

    private async Task<RazaDto> CreateRazaViaApiAsync()
    {
        var dto = new CreateRazaDto
        {
            nombre = $"Raza-{Guid.NewGuid()}",
            corpulencia = "Grande",
            nivelEnergia = "Alta",
            observacionesGenerales = "Raza de prueba"
        };
        var response = await _client.PostAsJsonAsync(RazaBasePath, dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<RazaDto>())!;
    }

    private async Task<(RazaDto raza, CuidadorDto cuidador)> CreateDependenciesAsync()
    {
        var raza = await CreateRazaViaApiAsync();
        var cuidador = await CreateCuidadorViaApiAsync();
        return (raza, cuidador);
    }

    private CreatePerroDto BuildCreateDto(Guid razaId, Guid cuidadorId) => new()
    {
        nombre = "Rex Test",
        edad = 3,
        peso = 25.5m,
        razaId = razaId,
        cuidadorId = cuidadorId,
        tipoComida = "Concentrado Premium",
        horarioComida = "8:00 AM - 6:00 PM",
        alergias = null,
        observaciones = "Perro de prueba"
    };

    private async Task<PerroDto> CreatePerroViaApiAsync()
    {
        var (raza, cuidador) = await CreateDependenciesAsync();
        var dto = BuildCreateDto(raza.Id, cuidador.Id);
        var response = await _client.PostAsJsonAsync(PerroBasePath, dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<PerroDto>())!;
    }

    [Fact]
    public async Task PostPerro_WhenValid_ShouldReturn201()
    {
        //Arrange
        var (raza, cuidador) = await CreateDependenciesAsync();
        var dto = BuildCreateDto(raza.Id, cuidador.Id);

        //Act
        var response = await _client.PostAsJsonAsync(PerroBasePath, dto);

        //Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<PerroDto>();
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(dto.nombre, result.nombre);
        Assert.Equal(dto.edad, result.edad);
        Assert.Equal(dto.peso, result.peso);
        Assert.Equal(raza.Id, result.razaId);
        Assert.Equal(cuidador.Id, result.cuidadorId);
        Assert.Equal(dto.tipoComida, result.tipoComida);
        Assert.Equal(dto.horarioComida, result.horarioComida);
    }

    [Fact]
    public async Task GetPerroById_WhenExists_ShouldReturn200()
    {
        //Arrange
        var created = await CreatePerroViaApiAsync();

        //Act
        var result = await _client.GetFromJsonAsync<PerroDto>($"{PerroBasePath}/{created.Id}");

        //Assert
        Assert.NotNull(result);
        Assert.Equal(created.Id, result.Id);
        Assert.Equal(created.nombre, result.nombre);
        Assert.NotNull(result.raza);
        Assert.NotNull(result.cuidador);
    }

    [Fact]
    public async Task GetAllPerros_ShouldReturn200()
    {
        //Arrange
        await CreatePerroViaApiAsync();

        //Act
        var response = await _client.GetAsync(PerroBasePath);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<IEnumerable<PerroDto>>();
        Assert.NotNull(result);
        Assert.NotEmpty(result);
    }

    [Fact]
    public async Task PutPerro_WhenValid_ShouldReturn200()
    {
        //Arrange
        var created = await CreatePerroViaApiAsync();
        var updateDto = new UpdatePerroDto
        {
            Id = created.Id,
            nombre = "Rex Actualizado",
            edad = 5,
            peso = created.peso,
            horarioComida = created.horarioComida,
            tipoComida = created.tipoComida
        };

        //Act
        var response = await _client.PutAsJsonAsync(PerroBasePath, updateDto);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<PerroDto>();
        Assert.NotNull(result);
        Assert.Equal("Rex Actualizado", result.nombre);
        Assert.Equal(5, result.edad);
    }

    [Fact]
    public async Task PatchPerro_WhenValid_ShouldReturn200()
    {
        //Arrange
        var created = await CreatePerroViaApiAsync();
        var patchDto = new UpdatePerroDto
        {
            Id = created.Id,
            tipoComida = "Comida Húmeda"
        };

        //Act
        var response = await _client.PatchAsJsonAsync(PerroBasePath, patchDto);

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<PerroDto>();
        Assert.NotNull(result);
        Assert.Equal("Comida Húmeda", result.tipoComida);
        Assert.Equal(created.nombre, result.nombre);
    }

    [Fact]
    public async Task DeletePerro_WhenExists_ShouldReturn200()
    {
        //Arrange
        var created = await CreatePerroViaApiAsync();

        //Act
        var response = await _client.DeleteAsync($"{PerroBasePath}/{created.Id}");

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostPerro_WhenRazaNotFound_ShouldReturn404()
    {
        //Arrange
        var cuidador = await CreateCuidadorViaApiAsync();
        var dto = BuildCreateDto(Guid.NewGuid(), cuidador.Id);

        //Act
        var response = await _client.PostAsJsonAsync(PerroBasePath, dto);

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostPerro_WhenCuidadorNotFound_ShouldReturn404()
    {
        //Arrange
        var raza = await CreateRazaViaApiAsync();
        var dto = BuildCreateDto(raza.Id, Guid.NewGuid());

        //Act
        var response = await _client.PostAsJsonAsync(PerroBasePath, dto);

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetPerroById_WhenNotFound_ShouldReturn404()
    {
        //Act
        var response = await _client.GetAsync($"{PerroBasePath}/{Guid.NewGuid()}");

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
