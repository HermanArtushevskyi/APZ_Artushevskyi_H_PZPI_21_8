using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Task2.Migrations
{
    /// <inheritdoc />
    public partial class AddedBalances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "DronesSet",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "BalancesSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalancesSet", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BalancesSet");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "DronesSet");
        }
    }
}
