using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace BarCrawlers.Data.Migrations
{
    public partial class FixLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bars_Locations_LocationId",
                table: "Bars");

            migrationBuilder.AlterColumn<Guid>(
                name: "LocationId",
                table: "Bars",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Bars_Locations_LocationId",
                table: "Bars",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bars_Locations_LocationId",
                table: "Bars");

            migrationBuilder.AlterColumn<Guid>(
                name: "LocationId",
                table: "Bars",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bars_Locations_LocationId",
                table: "Bars",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
