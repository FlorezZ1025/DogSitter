CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Cuidadores" (
    "Id" uuid NOT NULL,
    nombre character varying(150) NOT NULL,
    telefono character varying(20) NOT NULL,
    email character varying(150) NOT NULL,
    "fechaInicioExperiencia" timestamp with time zone NOT NULL,
    direccion character varying(250) NOT NULL,
    activo boolean NOT NULL,
    "CreatedOn" timestamp with time zone NOT NULL,
    "LastModifiedOn" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Cuidadores" PRIMARY KEY ("Id")
);

CREATE TABLE "Razas" (
    "Id" uuid NOT NULL,
    nombre character varying(100) NOT NULL,
    corpulencia character varying(20) NOT NULL,
    "nivelEnergia" character varying(20) NOT NULL,
    "observacionesGenerales" character varying(1000),
    "CreatedOn" timestamp with time zone NOT NULL,
    "LastModifiedOn" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Razas" PRIMARY KEY ("Id")
);

CREATE TABLE "Voter" (
    "Id" uuid NOT NULL,
    "Nid" text NOT NULL,
    "DateOfBirth" timestamp with time zone NOT NULL,
    "Origin" text NOT NULL,
    "CreatedOn" timestamp with time zone NOT NULL,
    "LastModifiedOn" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Voter" PRIMARY KEY ("Id")
);

CREATE TABLE "Perros" (
    "Id" uuid NOT NULL,
    nombre character varying(100) NOT NULL,
    edad smallint NOT NULL,
    peso numeric(5,2) NOT NULL,
    "razaId" uuid NOT NULL,
    "cuidadorId" uuid NOT NULL,
    "tipoComida" character varying(100) NOT NULL,
    "horarioComida" character varying(100) NOT NULL,
    alergias character varying(250) NOT NULL,
    observaciones text,
    "CreatedOn" timestamp with time zone NOT NULL,
    "LastModifiedOn" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Perros" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Perros_Cuidadores_cuidadorId" FOREIGN KEY ("cuidadorId") REFERENCES "Cuidadores" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Perros_Razas_razaId" FOREIGN KEY ("razaId") REFERENCES "Razas" ("Id") ON DELETE RESTRICT
);

CREATE UNIQUE INDEX "IX_Cuidadores_email" ON "Cuidadores" (email);

CREATE INDEX "IX_Perros_cuidadorId" ON "Perros" ("cuidadorId");

CREATE INDEX "IX_Perros_razaId" ON "Perros" ("razaId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260222201258_initialMigration', '8.0.11');

COMMIT;

START TRANSACTION;

DROP TABLE "Voter";

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260222202748_reomvingVoter', '8.0.11');

COMMIT;

START TRANSACTION;

ALTER TABLE "Perros" ALTER COLUMN alergias DROP NOT NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260301025922_CampoAlergiasNullableEnPerro', '8.0.11');

COMMIT;

