using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OganiMasterMVC.Migrations
{
    /// <inheritdoc />
    public partial class addFeatured : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeaturedProduct_Products_ProductId",
                table: "FeaturedProduct");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeaturedProduct",
                table: "FeaturedProduct");

            migrationBuilder.RenameTable(
                name: "FeaturedProduct",
                newName: "FeaturedProducts");

            migrationBuilder.RenameIndex(
                name: "IX_FeaturedProduct_ProductId",
                table: "FeaturedProducts",
                newName: "IX_FeaturedProducts_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeaturedProducts",
                table: "FeaturedProducts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeaturedProducts_Products_ProductId",
                table: "FeaturedProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeaturedProducts_Products_ProductId",
                table: "FeaturedProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FeaturedProducts",
                table: "FeaturedProducts");

            migrationBuilder.RenameTable(
                name: "FeaturedProducts",
                newName: "FeaturedProduct");

            migrationBuilder.RenameIndex(
                name: "IX_FeaturedProducts_ProductId",
                table: "FeaturedProduct",
                newName: "IX_FeaturedProduct_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FeaturedProduct",
                table: "FeaturedProduct",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FeaturedProduct_Products_ProductId",
                table: "FeaturedProduct",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
