using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ColorsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeleteToColorPalette : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "ColorPalettes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "ColorPalettes",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "ColorPalettes");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ColorPalettes");
        }
    }
}
