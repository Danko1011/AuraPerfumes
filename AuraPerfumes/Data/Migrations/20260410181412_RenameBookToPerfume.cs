using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuraPerfumes.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameBookToPerfume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetail_PerfumeVariants_VariantId",
                table: "CartDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_CartDetail_Perfume_PerfumeId",
                table: "CartDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Perfume_PerfumeId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_Perfume_Gender_GenderId",
                table: "Perfume");

            migrationBuilder.DropForeignKey(
                name: "FK_PerfumeVariants_Perfume_PerfumeId",
                table: "PerfumeVariants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Perfume",
                table: "Perfume");

            migrationBuilder.RenameTable(
                name: "Perfume",
                newName: "Pefume");

            migrationBuilder.RenameIndex(
                name: "IX_Perfume_GenderId",
                table: "Pefume",
                newName: "IX_Pefume_GenderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pefume",
                table: "Pefume",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetail_Pefume_PerfumeId",
                table: "CartDetail",
                column: "PerfumeId",
                principalTable: "Pefume",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetail_PerfumeVariants_VariantId",
                table: "CartDetail",
                column: "VariantId",
                principalTable: "PerfumeVariants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Pefume_PerfumeId",
                table: "OrderDetail",
                column: "PerfumeId",
                principalTable: "Pefume",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pefume_Gender_GenderId",
                table: "Pefume",
                column: "GenderId",
                principalTable: "Gender",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PerfumeVariants_Pefume_PerfumeId",
                table: "PerfumeVariants",
                column: "PerfumeId",
                principalTable: "Pefume",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetail_Pefume_PerfumeId",
                table: "CartDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_CartDetail_PerfumeVariants_VariantId",
                table: "CartDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Pefume_PerfumeId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_Pefume_Gender_GenderId",
                table: "Pefume");

            migrationBuilder.DropForeignKey(
                name: "FK_PerfumeVariants_Pefume_PerfumeId",
                table: "PerfumeVariants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pefume",
                table: "Pefume");

            migrationBuilder.RenameTable(
                name: "Pefume",
                newName: "Perfume");

            migrationBuilder.RenameIndex(
                name: "IX_Pefume_GenderId",
                table: "Perfume",
                newName: "IX_Perfume_GenderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Perfume",
                table: "Perfume",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetail_PerfumeVariants_VariantId",
                table: "CartDetail",
                column: "VariantId",
                principalTable: "PerfumeVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetail_Perfume_PerfumeId",
                table: "CartDetail",
                column: "PerfumeId",
                principalTable: "Perfume",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Perfume_PerfumeId",
                table: "OrderDetail",
                column: "PerfumeId",
                principalTable: "Perfume",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Perfume_Gender_GenderId",
                table: "Perfume",
                column: "GenderId",
                principalTable: "Gender",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PerfumeVariants_Perfume_PerfumeId",
                table: "PerfumeVariants",
                column: "PerfumeId",
                principalTable: "Perfume",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
