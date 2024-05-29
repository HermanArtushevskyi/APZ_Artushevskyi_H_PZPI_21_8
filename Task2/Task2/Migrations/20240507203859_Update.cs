using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task2.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "DronesSet");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "DronesSet");

            migrationBuilder.AddColumn<int>(
                name: "CurrentUserId",
                table: "DronesSet",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "DroneModelsSet",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "DronesToStationsSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DroneId = table.Column<int>(type: "int", nullable: false),
                    StationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DronesToStationsSet", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DronesToStationsSet");

            migrationBuilder.DropColumn(
                name: "CurrentUserId",
                table: "DronesSet");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "DroneModelsSet");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "DronesSet",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "DronesSet",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
