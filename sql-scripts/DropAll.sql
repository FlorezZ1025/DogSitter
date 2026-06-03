START TRANSACTION;

UPDATE "Perros" SET alergias = '' WHERE alergias IS NULL;
ALTER TABLE "Perros" ALTER COLUMN alergias SET NOT NULL;
ALTER TABLE "Perros" ALTER COLUMN alergias SET DEFAULT '';

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20260301025922_CampoAlergiasNullableEnPerro';

COMMIT;

START TRANSACTION;

CREATE TABLE "Voter" (
    "Id" uuid NOT NULL,
    "CreatedOn" timestamp with time zone NOT NULL,
    "DateOfBirth" timestamp with time zone NOT NULL,
    "LastModifiedOn" timestamp with time zone NOT NULL,
    "Nid" text NOT NULL,
    "Origin" text NOT NULL,
    CONSTRAINT "PK_Voter" PRIMARY KEY ("Id")
);

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20260222202748_reomvingVoter';

COMMIT;

START TRANSACTION;

DROP TABLE "Perros";

DROP TABLE "Voter";

DROP TABLE "Cuidadores";

DROP TABLE "Razas";

DELETE FROM "__EFMigrationsHistory"
WHERE "MigrationId" = '20260222201258_initialMigration';

COMMIT;

