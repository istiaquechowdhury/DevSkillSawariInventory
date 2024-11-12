using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingAgainSPagainagain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                    CREATE OR ALTER PROCEDURE GetProducts
                    @PageIndex int,
                    @PageSize int,
                    @OrderBy nvarchar(50),
                    @ItemName nvarchar(max) = '%',
                    @Barcode nvarchar(max) = '%',
                    @Status bit = 0,
                    @ImagePath nvarchar(max) = '%',
                    @Dhanmondi int = 0,
                    @Mirpur int = 0,
                    @Uttara int = 0,
                    @WIP int = 0,
                    @CategoryId uniqueidentifier = NULL,
                    @TaxesId uniqueidentifier = NULL,
                    @Total int OUTPUT,
                    @TotalDisplay int OUTPUT
                AS
                BEGIN
                    SET NOCOUNT ON;

                    -- 1. Calculate Total
                    SELECT @Total = COUNT(*) FROM Products;

                    -- 2. Calculate TotalDisplay based on filters
                    DECLARE @countsql nvarchar(4000) = '
                        SELECT @TotalDisplay = COUNT(*)
                        FROM Products p
                        WHERE p.ItemName LIKE @xItemName
                          AND p.Barcode LIKE @xBarcode
                          AND p.ImagePath LIKE @xImagePath
                          AND p.Status = @xStatus
                          ' 
                          + CASE WHEN @CategoryId IS NOT NULL THEN ' AND p.CategoryId = @xCategoryId' ELSE '' END
                          + CASE WHEN @TaxesId IS NOT NULL THEN ' AND p.TaxesId = @xTaxesId' ELSE '' END;

                    EXEC sp_executesql 
                        @countsql,
                        N'@xItemName nvarchar(max), @xBarcode nvarchar(max), @xImagePath nvarchar(max), @xStatus bit, @xCategoryId uniqueidentifier, @xTaxesId uniqueidentifier, @TotalDisplay int OUTPUT',
                        @ItemName, @Barcode, @ImagePath, @Status, @CategoryId, @TaxesId, @TotalDisplay = @TotalDisplay OUTPUT;

                    -- 3. Main data retrieval with pagination
                    DECLARE @sql nvarchar(4000) = '
                        SELECT p.Id, p.ItemName, p.Barcode, p.ImagePath, p.Status, 
                               c.Name AS CategoryName, t.Name AS TaxName,
                               (p.Dhanmondi + p.Mirpur + p.Uttara + p.WIP) AS TotalStock
                        FROM Products p
                        INNER JOIN Categories c ON p.CategoryId = c.Id
                        LEFT JOIN Taxes t ON p.TaxesId = t.Id
                        WHERE p.ItemName LIKE @xItemName
                          AND p.Barcode LIKE @xBarcode
                          AND p.ImagePath LIKE @xImagePath
                          AND p.Status = @xStatus'
                          + CASE WHEN @CategoryId IS NOT NULL THEN ' AND p.CategoryId = @xCategoryId' ELSE '' END
                          + CASE WHEN @TaxesId IS NOT NULL THEN ' AND p.TaxesId = @xTaxesId' ELSE '' END
                          + ' ORDER BY ' + @OrderBy
                          + ' OFFSET @PageSize * (@PageIndex - 1) ROWS FETCH NEXT @PageSize ROWS ONLY';

                    EXEC sp_executesql 
                        @sql,
                        N'@xItemName nvarchar(max), @xBarcode nvarchar(max), @xImagePath nvarchar(max), @xStatus bit, @xCategoryId uniqueidentifier, @xTaxesId uniqueidentifier, @PageSize int, @PageIndex int',
                        @ItemName, @Barcode, @ImagePath, @Status, @CategoryId, @TaxesId, @PageSize, @PageIndex;

                END;
                GO
                
                """;
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[GetProducts]";
            migrationBuilder.DropTable(sql);
        }
    }
}
