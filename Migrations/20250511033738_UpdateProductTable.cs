using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vegetarian.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Products_ParentId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ParentId",
                table: "Products");

            migrationBuilder.AddColumn<bool>(
                name: "IsCanDelete",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ProductChildren",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductChildren", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductChildren_Products_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductChildren_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductChildren_ParentId",
                table: "ProductChildren",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductChildren_ProductId",
                table: "ProductChildren",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductChildren");

            migrationBuilder.DropColumn(
                name: "IsCanDelete",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ParentId",
                table: "Products",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Products_ParentId",
                table: "Products",
                column: "ParentId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
