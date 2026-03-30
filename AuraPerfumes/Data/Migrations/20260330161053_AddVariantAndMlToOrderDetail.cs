using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuraPerfumes.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVariantAndMlToOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ml",
                table: "OrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VariantId",
                table: "OrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartDetail_VariantId",
                table: "CartDetail",
                column: "VariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetail_PerfumeVariants_VariantId",
                table: "CartDetail",
                column: "VariantId",
                principalTable: "PerfumeVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetail_PerfumeVariants_VariantId",
                table: "CartDetail");

            migrationBuilder.DropIndex(
                name: "IX_CartDetail_VariantId",
                table: "CartDetail");

            migrationBuilder.DropColumn(
                name: "Ml",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "VariantId",
                table: "OrderDetail");
        }
    }
}
