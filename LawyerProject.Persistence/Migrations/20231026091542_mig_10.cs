using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LawyerProject.Persistence.Migrations
{
    public partial class mig_10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Files",
                table: "Cases");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Files",
                table: "Cases",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
