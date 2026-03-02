namespace UDEM.DEVOPS.DogSitter.Domain.Dtos
{
    public record CuidadorDto
    {
        public Guid Id { get; set; }
        public required string nombre { get; set; }
        public required string telefono { get; set; }
        public required string email { get; set; }
        public DateTime fechaInicioExperiencia { get; set; }
        public required string direccion { get; set; }
        public required bool activo { get; set; }
    }

    public record CreateCuidadorDto
    {
        public required string nombre { get; set; }
        public required string telefono { get; set; }
        public required string email { get; set; }
        public DateTime fechaInicioExperiencia { get; set; }
        public required string direccion { get; set; }
        public required bool activo { get; set; }
    };

    public record UpdateCuidadorDto
    {
        public Guid Id { get; set; }
        public string? nombre { get; set; }
        public string? telefono { get; set; }
        public string? email { get; set; }
        public DateTime? fechaInicioExperiencia { get; set; }
        public string? direccion { get; set; }
        public bool? activo { get; set; }
    };

}
