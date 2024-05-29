using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task2.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StationId",
                table: "DronesSet");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StationId",
                table: "DronesSet",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
