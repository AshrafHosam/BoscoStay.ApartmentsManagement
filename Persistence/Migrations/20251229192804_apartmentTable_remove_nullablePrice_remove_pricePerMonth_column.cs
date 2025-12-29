using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class apartmentTable_remove_nullablePrice_remove_pricePerMonth_column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerMonth",
                table: "Apartments");

            migrationBuilder.AlterColumn<double>(
                name: "PricePerDay",
                table: "Apartments",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PricePerDay",
                table: "Apartments",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<double>(
                name: "PricePerMonth",
                table: "Apartments",
                type: "double precision",
                nullable: true);
        }
    }
}
