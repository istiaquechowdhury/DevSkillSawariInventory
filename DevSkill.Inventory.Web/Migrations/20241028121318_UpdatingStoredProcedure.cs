using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingStoredProcedure : Migration
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
                        @CategoryId uniqueidentifier = NULL,
                        @TaxesId uniqueidentifier = NULL,
                        @Total int output,
                        @TotalDisplay int output
                    AS
                    BEGIN
                        SET NOCOUNT ON;

                        DECLARE @sql nvarchar(2000);
                        DECLARE @countsql nvarchar(2000);
                        DECLARE @paramList nvarchar(MAX); 
                        DECLARE @countparamList nvarchar(MAX);

                        -- Collecting Total
                        SELECT @Total = COUNT(*) FROM Products;

                        -- Collecting Total Display
                        SET @countsql = 'SELECT @TotalDisplay = COUNT(*) 
                                         FROM Products p 
                                         INNER JOIN Categories c ON p.CategoryId = c.Id 
                                         INNER JOIN Taxes t ON p.TaxesId = t.Id 
                                         WHERE 1 = 1';

                        SET @countsql = @countsql + ' AND p.ItemName LIKE ''%'' + @xItemName + ''%''';
                        SET @countsql = @countsql + ' AND p.Barcode LIKE ''%'' + @xBarcode + ''%''' ;

                        IF @CategoryId IS NOT NULL
                            SET @countsql = @countsql + ' AND p.CategoryId = @xCategoryId';

                        IF @TaxesId IS NOT NULL
                            SET @countsql = @countsql + ' AND p.TaxesId = @xTaxesId';

                        SELECT @countparamList = '@xItemName nvarchar(max),
                                                  @xBarcode nvarchar(max),
                                                  @xCategoryId uniqueidentifier,
                                                  @xTaxesId uniqueidentifier,
                                                  @TotalDisplay int output';

                        EXEC sp_executesql @countsql, @countparamList,
                                           @ItemName,
                                           @Barcode,
                                           @CategoryId,
                                           @TaxesId,
                                           @TotalDisplay = @TotalDisplay OUTPUT;

                        -- Collecting Data
                        SET @sql = 'SELECT p.Id, 
                                           p.ItemName, 
                                           p.Barcode, 
                                           p.ImagePath, 

                                           p.Status, 
                                           c.Name AS CategoryName, 
                                           t.Percentage AS Tax,
                                           (p.Mirpur + p.Uttara + p.WIP + p.Dhanmondi) AS TotalStock
                                    FROM Products p 
                                    INNER JOIN Categories c ON p.CategoryId = c.Id 
                                    INNER JOIN Taxes t ON p.TaxesId = t.Id 
                                    WHERE 1 = 1';

                        SET @sql = @sql + ' AND p.ItemName LIKE ''%'' + @xItemName + ''%''';
                        SET @sql = @sql + ' AND p.Barcode LIKE ''%'' + @xBarcode + ''%''' ;

                        IF @CategoryId IS NOT NULL
                            SET @sql = @sql + ' AND p.CategoryId = @xCategoryId';

                        IF @TaxesId IS NOT NULL
                            SET @sql = @sql + ' AND p.TaxesId = @xTaxesId';

                        SET @sql = @sql + ' ORDER BY ' + @OrderBy + ' 
                                            OFFSET @PageSize * (@PageIndex - 1) ROWS 
                                            FETCH NEXT @PageSize ROWS ONLY';

                        SELECT @paramList = '@xItemName nvarchar(max),
                                             @xBarcode nvarchar(max),
                                             @xCategoryId uniqueidentifier,
                                             @xTaxesId uniqueidentifier,
                                             @PageIndex int,
                                             @PageSize int';

                        EXEC sp_executesql @sql, @paramList,
                                           @ItemName,
                                           @Barcode,
                                           @CategoryId,
                                           @TaxesId,
                                           @PageIndex,
                                           @PageSize;
                    END
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
