using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace USVStudDocs.DAL.Migrations
{
    public partial class Initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SecretaryId",
                table: "Certificates",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_SecretaryId",
                table: "Certificates",
                column: "SecretaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_FacultyPerson_SecretaryId",
                table: "Certificates",
                column: "SecretaryId",
                principalTable: "FacultyPerson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_FacultyPerson_SecretaryId",
                table: "Certificates");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_SecretaryId",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "SecretaryId",
                table: "Certificates");
        }
    }
}
