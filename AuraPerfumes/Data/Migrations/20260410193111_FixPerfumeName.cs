using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuraPerfumes.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixPerfumeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Pefume",
                newName: "Perfume");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Perfume",
                newName: "Pefume");
        }
    }
}
