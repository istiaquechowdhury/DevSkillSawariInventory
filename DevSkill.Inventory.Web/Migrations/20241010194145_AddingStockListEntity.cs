using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddingStockListEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create the new StockLists table
            migrationBuilder.CreateTable(
                name: "StockLists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InHand = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    WeightedAvgCost = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockLists_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockLists_WareHouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "WareHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Add indexes for ProductId and WarehouseId columns
            migrationBuilder.CreateIndex(
                name: "IX_StockLists_ProductId",
                table: "StockLists",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockLists_WarehouseId",
                table: "StockLists",
                column: "WarehouseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the StockLists table if we need to roll back this migration
            migrationBuilder.DropTable(
                name: "StockLists");
        }
    }
}
