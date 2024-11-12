using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations
{
    /// <inheritdoc />
    public partial class addedProductIdandTransferredQuantityToStockTransfer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Stocktransfers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "TransferredQuantity",
                table: "Stocktransfers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Stocktransfers_ProductId",
                table: "Stocktransfers",
                column: "ProductId");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropIndex(
                name: "IX_Stocktransfers_ProductId",
                table: "Stocktransfers");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Stocktransfers");

            migrationBuilder.DropColumn(
                name: "TransferredQuantity",
                table: "Stocktransfers");
        }
    }
}
