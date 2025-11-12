using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class adjust_apartment_pricingPlan_referenceFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apartments_PricingPlans_PricingPlanId",
                table: "Apartments");

            migrationBuilder.AlterColumn<Guid>(
                name: "PricingPlanId",
                table: "Apartments",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Apartments_PricingPlans_PricingPlanId",
                table: "Apartments",
                column: "PricingPlanId",
                principalTable: "PricingPlans",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apartments_PricingPlans_PricingPlanId",
                table: "Apartments");

            migrationBuilder.AlterColumn<Guid>(
                name: "PricingPlanId",
                table: "Apartments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Apartments_PricingPlans_PricingPlanId",
                table: "Apartments",
                column: "PricingPlanId",
                principalTable: "PricingPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
