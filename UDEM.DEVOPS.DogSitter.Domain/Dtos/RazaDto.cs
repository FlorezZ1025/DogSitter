using UDEM.DEVOPS.DogSitter.Domain.Entities;

namespace UDEM.DEVOPS.DogSitter.Domain.Dtos
{
    public record RazaDto
    {
        public Guid Id { get; set; }
        public required string nombre { get; set; }
        public required string corpulencia { get; set; }
        public required string nivelEnergia { get; set; }
        public string? observacionesGenerales { get; set; } = null;
    }

    public record CreateRazaDto
    {
        public required string nombre { get; set; }
        public required string corpulencia { get; set; }
        public required string nivelEnergia { get; set; }
        public string? observacionesGenerales { get; set; } = null;
    };

    public record UpdateRazaDto
    {
        public string? nombre { get; set; }
        public string? corpulencia { get; set; }
        public string? nivelEnergia { get; set; }
        public string? observacionesGenerales { get; set; }
    };
}
