using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace vegetarian.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnIsPublic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogsUsers",
                table: "BlogsUsers");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);


            migrationBuilder.Sql(@"
                ALTER TABLE BlogsUsers DROP COLUMN Id;
                ALTER TABLE BlogsUsers ADD Id INT IDENTITY(1,1) NOT NULL;
                ALTER TABLE BlogsUsers ADD CONSTRAINT PK_BlogsUsers PRIMARY KEY (Id);
            ");

            migrationBuilder.CreateIndex(
                name: "IX_BlogsUsers_BlogId",
                table: "BlogsUsers",
                column: "BlogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogsUsers",
                table: "BlogsUsers");

            migrationBuilder.DropIndex(
                name: "IX_BlogsUsers_BlogId",
                table: "BlogsUsers");

            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "BlogsUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogsUsers",
                table: "BlogsUsers",
                column: "BlogId");
        }
    }
}
