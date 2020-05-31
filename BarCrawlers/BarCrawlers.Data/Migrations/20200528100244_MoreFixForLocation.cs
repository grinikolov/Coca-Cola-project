using Microsoft.EntityFrameworkCore.Migrations;

namespace BarCrawlers.Data.Migrations
{
    public partial class MoreFixForLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Longtitude",
                table: "Locations",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "Lattitude",
                table: "Locations",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Longtitude",
                table: "Locations",
                type: "float",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<double>(
                name: "Lattitude",
                table: "Locations",
                type: "float",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
