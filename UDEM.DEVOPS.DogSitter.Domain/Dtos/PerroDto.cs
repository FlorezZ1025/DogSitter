namespace UDEM.DEVOPS.DogSitter.Domain.Dtos
{
    public record PerroDto
    {
        public Guid Id { get; set; }
        public required string nombre { get; set; }
        public required short edad { get; set; }
        public required decimal peso { get; set; }
        public Guid razaId { get; set; }
        public Guid cuidadorId { get; set; }
        public required string tipoComida { get; set; }
        public required string horarioComida { get; set; }
        public required string alergias { get; set; }
        public string? observaciones { get; set; } = null;
    }

    public record CreatePerroDto
    {
        public required string nombre { get; set; }
        public required short edad { get; set; }
        public required decimal peso { get; set; }
        public required Guid razaId { get; set; }
        public required Guid cuidadorId { get; set; }
        public required string tipoComida { get; set; }
        public required string horarioComida { get; set; }
        public required string alergias { get; set; }
        public string? observaciones { get; set; } = null;
    };

    public record UpdatePerroDto
    {
        public string? nombre { get; set; }
        public short? edad { get; set; }
        public decimal? peso { get; set; }
        public Guid? razaId { get; set; }
        public Guid? cuidadorId { get; set; }
        public string? tipoComida { get; set; }
        public string? horarioComida { get; set; }
        public string? alergias { get; set; }
        public string? observaciones { get; set; }
    };
}
