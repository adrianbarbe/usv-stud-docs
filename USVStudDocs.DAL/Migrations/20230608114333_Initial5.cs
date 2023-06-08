using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace USVStudDocs.DAL.Migrations
{
    public partial class Initial5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Certificates");

            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "CommonRegistrationNumber",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "CommonRegistrationNumber");

            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "Certificates",
                type: "integer",
                nullable: true);
        }
    }
}
