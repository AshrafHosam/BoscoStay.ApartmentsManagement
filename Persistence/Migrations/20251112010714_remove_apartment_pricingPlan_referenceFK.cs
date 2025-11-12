using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class remove_apartment_pricingPlan_referenceFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apartments_PricingPlans_PricingPlanId",
                table: "Apartments");

            migrationBuilder.DropTable(
                name: "PricingPlans");

            migrationBuilder.DropIndex(
                name: "IX_Apartments_PricingPlanId",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "PricingPlanId",
                table: "Apartments");

            migrationBuilder.AddColumn<double>(
                name: "PricePerDay",
                table: "Apartments",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PricePerHour",
                table: "Apartments",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PricePerMonth",
                table: "Apartments",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerDay",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "PricePerHour",
                table: "Apartments");

            migrationBuilder.DropColumn(
                name: "PricePerMonth",
                table: "Apartments");

            migrationBuilder.AddColumn<Guid>(
                name: "PricingPlanId",
                table: "Apartments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PricingPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    PricePerDay = table.Column<double>(type: "double precision", nullable: true),
                    PricePerHour = table.Column<double>(type: "double precision", nullable: true),
                    PricePerMonth = table.Column<double>(type: "double precision", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricingPlans", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apartments_PricingPlanId",
                table: "Apartments",
                column: "PricingPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartments_PricingPlans_PricingPlanId",
                table: "Apartments",
                column: "PricingPlanId",
                principalTable: "PricingPlans",
                principalColumn: "Id");
        }
    }
}
