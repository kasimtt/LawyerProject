using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LawyerProject.Persistence.Migrations
{
    public partial class mig_7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdUserFK",
                table: "Adverts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_IdUserFK",
                table: "Adverts",
                column: "IdUserFK");

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_Users_IdUserFK",
                table: "Adverts",
                column: "IdUserFK",
                principalTable: "Users",
                principalColumn: "ObjectId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_Users_IdUserFK",
                table: "Adverts");

            migrationBuilder.DropIndex(
                name: "IX_Adverts_IdUserFK",
                table: "Adverts");

            migrationBuilder.DropColumn(
                name: "IdUserFK",
                table: "Adverts");
        }
    }
}
