using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pizzeria.Migrations
{
    /// <inheritdoc />
    public partial class addedNotMappedPropreties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataOrdine",
                table: "Ordini",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PrezzoTotale",
                table: "Ordini",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataOrdine",
                table: "Ordini");

            migrationBuilder.DropColumn(
                name: "PrezzoTotale",
                table: "Ordini");
        }
    }
}
