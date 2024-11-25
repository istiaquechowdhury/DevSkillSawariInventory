﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations
{
    /// <inheritdoc />
    public partial class addedDestinationWareHouseId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DestinationWarehouseId",
                table: "Stocktransfers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Stocktransfers_DestinationWarehouseId",
                table: "Stocktransfers",
                column: "DestinationWarehouseId");

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          

            migrationBuilder.DropIndex(
                name: "IX_Stocktransfers_DestinationWarehouseId",
                table: "Stocktransfers");

            migrationBuilder.DropColumn(
                name: "DestinationWarehouseId",
                table: "Stocktransfers");
        }
    }
}
