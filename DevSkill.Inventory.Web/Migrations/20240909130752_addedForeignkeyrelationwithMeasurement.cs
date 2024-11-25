using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations
{
    /// <inheritdoc />
    public partial class addedForeignkeyrelationwithMeasurement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
            name: "FK_Products_MeasurementUnits_MeasurementUnitId",
            table: "Products",
            column: "MeasurementUnitId",
            principalTable: "MeasurementUnits",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
            name: "FK_Products_MeasurementUnits_MeasurementUnitId",
            table: "Products");
        }
    }
}
