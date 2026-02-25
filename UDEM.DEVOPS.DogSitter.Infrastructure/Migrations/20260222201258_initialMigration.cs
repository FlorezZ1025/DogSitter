using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UDEM.DEVOPS.DogSitter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cuidadores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    fechaInicioExperiencia = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    direccion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cuidadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Razas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    corpulencia = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    nivelEnergia = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    observacionesGenerales = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Razas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Voter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nid = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Origin = table.Column<string>(type: "text", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Perros",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    edad = table.Column<short>(type: "smallint", nullable: false),
                    peso = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    razaId = table.Column<Guid>(type: "uuid", nullable: false),
                    cuidadorId = table.Column<Guid>(type: "uuid", nullable: false),
                    tipoComida = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    horarioComida = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    alergias = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    observaciones = table.Column<string>(type: "text", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastModifiedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Perros_Cuidadores_cuidadorId",
                        column: x => x.cuidadorId,
                        principalTable: "Cuidadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Perros_Razas_razaId",
                        column: x => x.razaId,
                        principalTable: "Razas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cuidadores_email",
                table: "Cuidadores",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Perros_cuidadorId",
                table: "Perros",
                column: "cuidadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Perros_razaId",
                table: "Perros",
                column: "razaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Perros");

            migrationBuilder.DropTable(
                name: "Voter");

            migrationBuilder.DropTable(
                name: "Cuidadores");

            migrationBuilder.DropTable(
                name: "Razas");
        }
    }
}
