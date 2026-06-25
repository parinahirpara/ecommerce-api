using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceAPI.Migrations
{
    /// <inheritdoc />
    public partial class Add_Material_Variant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_MaterialId",
                table: "ProductVariants",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariants_Materials_MaterialId",
                table: "ProductVariants",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariants_Materials_MaterialId",
                table: "ProductVariants");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_MaterialId",
                table: "ProductVariants");
        }
    }
}
