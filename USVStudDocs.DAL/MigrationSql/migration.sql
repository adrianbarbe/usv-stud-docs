START TRANSACTION;

ALTER TABLE "Certificates" DROP COLUMN "OrderNumber";

ALTER TABLE "CommonRegistrationNumber" ADD "OrderNumber" integer NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20230608114333_Initial5', '6.0.10');

COMMIT;

