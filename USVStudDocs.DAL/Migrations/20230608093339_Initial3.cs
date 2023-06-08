using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace USVStudDocs.DAL.Migrations
{
    public partial class Initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedDate",
                table: "Certificates",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "Certificates",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CommonRegistrationNumber",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RegistrationNumber = table.Column<string>(type: "text", nullable: false),
                    FacultyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonRegistrationNumber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommonRegistrationNumber_Faculty_FacultyId",
                        column: x => x.FacultyId,
                        principalTable: "Faculty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommonRegistrationNumber_FacultyId",
                table: "CommonRegistrationNumber",
                column: "FacultyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommonRegistrationNumber");

            migrationBuilder.DropColumn(
                name: "ApprovedDate",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Certificates");
        }
    }
}
