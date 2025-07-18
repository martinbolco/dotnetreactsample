using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;


#nullable disable

namespace server_api.Migrations
{
	/// <inheritdoc />
	[ExcludeFromCodeCoverage]
	public partial class addPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Products",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");
        }
    }
}
