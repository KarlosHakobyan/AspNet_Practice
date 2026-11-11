using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNet.Migrations
{
    /// <inheritdoc />
    public partial class FixProductDetailsPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProductDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
