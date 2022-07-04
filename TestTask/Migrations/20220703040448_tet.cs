using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestTaskDomain.Migrations
{
    public partial class tet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanySubdivisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanySubdivisionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySubdivisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanySubdivisions_CompanySubdivisions_CompanySubdivisionId",
                        column: x => x.CompanySubdivisionId,
                        principalTable: "CompanySubdivisions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanySubdivisions_CompanySubdivisionId",
                table: "CompanySubdivisions",
                column: "CompanySubdivisionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanySubdivisions");
        }
    }
}
