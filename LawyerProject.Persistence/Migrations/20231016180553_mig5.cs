using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LawyerProject.Persistence.Migrations
{
    public partial class mig5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaseCasePdfFile",
                columns: table => new
                {
                    CasePdfFilesObjectId = table.Column<int>(type: "int", nullable: false),
                    CasesObjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseCasePdfFile", x => new { x.CasePdfFilesObjectId, x.CasesObjectId });
                    table.ForeignKey(
                        name: "FK_CaseCasePdfFile_Cases_CasesObjectId",
                        column: x => x.CasesObjectId,
                        principalTable: "Cases",
                        principalColumn: "ObjectId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CaseCasePdfFile_Files_CasePdfFilesObjectId",
                        column: x => x.CasePdfFilesObjectId,
                        principalTable: "Files",
                        principalColumn: "ObjectId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CaseCasePdfFile_CasesObjectId",
                table: "CaseCasePdfFile",
                column: "CasesObjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseCasePdfFile");
        }
    }
}
