using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations
{
    /// <inheritdoc />
    public partial class addedStockListSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                       CREATE OR ALTER PROCEDURE GetStockList
                       @PageIndex INT,
                       @PageSize INT,
                       @OrderBy NVARCHAR(50),
                       @ItemName NVARCHAR(MAX) = NULL,
                       @Barcode NVARCHAR(MAX) = NULL,
                       @WarehouseId UNIQUEIDENTIFIER = NULL,
                       @BelowMinStock BIT = 0,
                       @MinInHand INT = NULL,
                       @MaxInHand INT = NULL,
                       @MinWeightedAvgCost DECIMAL(18, 2) = NULL,
                       @MaxWeightedAvgCost DECIMAL(18, 2) = NULL,
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
                       LEFT JOIN Warehouses w ON s.WarehouseId = w.Id
                       WHERE (@ItemName IS NULL OR p.ItemName LIKE '%' + @ItemName + '%')
                         AND (@Barcode IS NULL OR p.Barcode LIKE '%' + @Barcode + '%')
                         AND (@WarehouseId IS NULL OR s.WarehouseId = @WarehouseId)
                         AND (@BelowMinStock = 0 OR s.InHand < p.InStock)
                         AND (@MinInHand IS NULL OR s.InHand >= @MinInHand)
                         AND (@MaxInHand IS NULL OR s.InHand <= @MaxInHand)
                         AND (@MinWeightedAvgCost IS NULL OR s.WeightedAvgCost >= @MinWeightedAvgCost)
                         AND (@MaxWeightedAvgCost IS NULL OR s.WeightedAvgCost <= @MaxWeightedAvgCost);

                       -- Select the Paginated Data with Additional Filters
                       DECLARE @sql NVARCHAR(MAX);
                       SET @sql = '
                           SELECT 
                               s.Id, 
                               p.ItemName, 
                               p.Barcode, 
                               w.Name AS WareHouseName,
                               s.InHand, 
                               s.WeightedAvgCost,
                  			 p.InStock
                           FROM StockLists s
                           LEFT JOIN Products p ON s.ProductId = p.Id
                           LEFT JOIN Warehouses w ON s.WarehouseId = w.Id
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
            var sql = "DROP PROCEDURE [dbo].[GetStockList]";
            migrationBuilder.DropTable(sql);

        }
    }
}
