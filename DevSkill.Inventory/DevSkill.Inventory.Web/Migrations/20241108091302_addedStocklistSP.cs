using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations
{
    /// <inheritdoc />
    public partial class addedStocklistSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """

                                            USE [Inventory]
                      GO
                      SET ANSI_NULLS ON
                      GO
                      SET QUOTED_IDENTIFIER ON
                      GO

                      CREATE OR ALTER PROCEDURE [dbo].[GetAllStockLists]
                          @PageIndex INT,
                          @PageSize INT, 
                          @OrderBy NVARCHAR(50),
                          @ItemName NVARCHAR(MAX) = NULL,
                          @Barcode NVARCHAR(MAX) = NULL,
                          @WarehouseId UNIQUEIDENTIFIER = NULL, -- Filter by WarehouseId
                          @BelowMinStock BIT = 0, -- Optional parameter for below minimum stock
                          @MinInHand INT = NULL, -- Minimum InHand filter
                          @MaxInHand INT = NULL, -- Maximum InHand filter
                          @MinWeightedAvgCost DECIMAL(18, 2) = NULL, -- Minimum WeightedAvgCost filter
                          @MaxWeightedAvgCost DECIMAL(18, 2) = NULL, -- Maximum WeightedAvgCost filter
                          @Total INT OUTPUT,
                          @TotalDisplay INT OUTPUT
                      AS
                      BEGIN
                          SET NOCOUNT ON;

                          -- Collect Total Count of Stock Items
                          SELECT @Total = COUNT(*) FROM StockLists;

                          -- Collect Total Display Count with Filters Applied
                          SELECT @TotalDisplay = COUNT(*)
                          FROM StockLists s
                          LEFT JOIN Products p ON s.ProductId = p.Id
                          WHERE (@ItemName IS NULL OR p.ItemName LIKE '%' + @ItemName + '%')
                            AND (@Barcode IS NULL OR p.Barcode LIKE '%' + @Barcode + '%')
                            AND (@WarehouseId IS NULL OR s.WarehouseId = @WarehouseId)
                            AND (@BelowMinStock = 0 OR s.InHand < p.InStock) -- Apply below min stock filter if specified
                            AND (@MinInHand IS NULL OR s.InHand >= @MinInHand) -- Minimum InHand filter
                            AND (@MaxInHand IS NULL OR s.InHand <= @MaxInHand) -- Maximum InHand filter
                            AND (@MinWeightedAvgCost IS NULL OR s.WeightedAvgCost >= @MinWeightedAvgCost) -- Minimum WeightedAvgCost filter
                            AND (@MaxWeightedAvgCost IS NULL OR s.WeightedAvgCost <= @MaxWeightedAvgCost); -- Maximum WeightedAvgCost filter

                          -- Select the Paginated Data with Additional Filters
                          DECLARE @sql NVARCHAR(MAX);
                          SET @sql = '
                              SELECT 
                                  s.Id, 
                                  p.ItemName, 
                                  p.Barcode, 
                                  w.Name AS WareHouseName,  
                                  s.InHand, 
                                  s.WeightedAvgCost
                              FROM StockLists s
                              LEFT JOIN Products p ON s.ProductId = p.Id
                              LEFT JOIN WareHouses w ON s.WarehouseId = w.Id
                              WHERE (@ItemName IS NULL OR p.ItemName LIKE ''%'' + @ItemName + ''%'')
                                AND (@Barcode IS NULL OR p.Barcode LIKE ''%'' + @Barcode + ''%'')
                                AND (@WarehouseId IS NULL OR s.WarehouseId = @WarehouseId)
                                AND (@BelowMinStock = 0 OR s.InHand < p.InStock)
                                AND (@MinInHand IS NULL OR s.InHand >= @MinInHand)
                                AND (@MaxInHand IS NULL OR s.InHand <= @MaxInHand)
                                AND (@MinWeightedAvgCost IS NULL OR s.WeightedAvgCost >= @MinWeightedAvgCost)
                                AND (@MaxWeightedAvgCost IS NULL OR s.WeightedAvgCost <= @MaxWeightedAvgCost)
                              ORDER BY ' + @OrderBy + '
                              OFFSET @PageSize * (@PageIndex - 1) ROWS
                              FETCH NEXT @PageSize ROWS ONLY;
                          ';

                          -- Define the Parameter List for sp_executesql
                          DECLARE @paramList NVARCHAR(MAX) = '@ItemName NVARCHAR(MAX), 
                                                               @Barcode NVARCHAR(MAX), 
                                                               @WarehouseId UNIQUEIDENTIFIER, 
                                                               @PageIndex INT, 
                                                               @PageSize INT,
                                                               @BelowMinStock BIT,
                                                               @MinInHand INT,
                                                               @MaxInHand INT,
                                                               @MinWeightedAvgCost DECIMAL(18, 2),
                                                               @MaxWeightedAvgCost DECIMAL(18, 2)';

                          -- Execute the Paginated Query
                          EXEC sp_executesql 
                              @sql,
                              @paramList,
                              @ItemName = @ItemName,
                              @Barcode = @Barcode,
                              @WarehouseId = @WarehouseId,
                              @PageIndex = @PageIndex,
                              @PageSize = @PageSize,
                              @BelowMinStock = @BelowMinStock,
                              @MinInHand = @MinInHand,
                              @MaxInHand = @MaxInHand,
                              @MinWeightedAvgCost = @MinWeightedAvgCost,
                              @MaxWeightedAvgCost = @MaxWeightedAvgCost;
                      END;
                      
                      """;

            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[GetAllStockLists]";
            migrationBuilder.DropTable(sql);
        }
    }
}
