using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LawyerProject.Persistence.Migrations
{
    public partial class mig_13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_Users_IdUserFK",
                table: "Adverts");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Users_IdUserFK",
                table: "Cases");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "IdUserFK",
                table: "Cases",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "IdUserFK",
                table: "Adverts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_AspNetUsers_IdUserFK",
                table: "Adverts",
                column: "IdUserFK",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_AspNetUsers_IdUserFK",
                table: "Cases",
                column: "IdUserFK",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adverts_AspNetUsers_IdUserFK",
                table: "Adverts");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_AspNetUsers_IdUserFK",
                table: "Cases");

            migrationBuilder.AlterColumn<int>(
                name: "IdUserFK",
                table: "Cases",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "IdUserFK",
                table: "Adverts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ObjectId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataState = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ProfileImage = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ObjectId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Adverts_Users_IdUserFK",
                table: "Adverts",
                column: "IdUserFK",
                principalTable: "Users",
                principalColumn: "ObjectId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Users_IdUserFK",
                table: "Cases",
                column: "IdUserFK",
                principalTable: "Users",
                principalColumn: "ObjectId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
