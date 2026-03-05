using Microsoft.EntityFrameworkCore.Migrations;


namespace AuraPerfumes.Data.Migrations
{
    public partial class AddPerfumeDescriptionAndVariants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Perfume",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PerfumeVariants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PerfumeId = table.Column<int>(type: "int", nullable: false),
                    Ml = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfumeVariants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfumeVariants_Perfume_PerfumeId",
                        column: x => x.PerfumeId,
                        principalTable: "Perfume",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PerfumeVariants_PerfumeId",
                table: "PerfumeVariants",
                column: "PerfumeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PerfumeVariants");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Perfume");
        }
    }
}