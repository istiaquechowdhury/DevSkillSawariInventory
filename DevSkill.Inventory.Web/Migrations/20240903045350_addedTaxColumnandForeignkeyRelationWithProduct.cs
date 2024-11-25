using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations
{
    /// <inheritdoc />
    public partial class addedTaxColumnandForeignkeyRelationWithProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tax",
                table: "Products");

            migrationBuilder.AddColumn<Guid>(
                name: "TaxesId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Products_TaxesId",
                table: "Products",
                column: "TaxesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Taxes_TaxesId",
                table: "Products",
                column: "TaxesId",
                principalTable: "Taxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Taxes_TaxesId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_TaxesId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TaxesId",
                table: "Products");

            migrationBuilder.AddColumn<decimal>(
                name: "Tax",
                table: "Products",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
