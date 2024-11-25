using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations
{
    /// <inheritdoc />
    public partial class addingGetAllProductsUpdateSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                 
                                      USE [Inventory]
                   GO

                   CREATE or ALTER PROCEDURE [dbo].[GetAllProducts]
                       @PageIndex INT,
                       @PageSize INT, 
                       @OrderBy NVARCHAR(50),
                       @ItemName NVARCHAR(MAX) = NULL,
                       @Barcode NVARCHAR(MAX) = NULL,
                       @ImagePath NVARCHAR(MAX) = NULL,
                       @CategoryId UNIQUEIDENTIFIER = NULL,
                       @MinTotalStock INT = NULL,
                       @MaxTotalStock INT = NULL,
                       @Status BIT = NULL,
                       @TaxesId UNIQUEIDENTIFIER = NULL,
                       @BelowMinStock BIT = 0, -- New optional parameter
                       @Total INT OUTPUT,
                       @TotalDisplay INT OUTPUT
                   AS
                   BEGIN
                       SET NOCOUNT ON;

                       -- Collect Total Count of Products
                       SELECT @Total = COUNT(*) FROM Products;

                       -- Collect Total Display Count with Filters Applied
                       SELECT @TotalDisplay = COUNT(*)
                       FROM Products p
                       LEFT JOIN Categories c ON p.CategoryId = c.Id
                       LEFT JOIN Taxes t ON p.TaxesId = t.Id
                       WHERE (@ItemName IS NULL OR p.ItemName LIKE '%' + @ItemName + '%')
                         AND (@Barcode IS NULL OR p.Barcode LIKE '%' + @Barcode + '%')
                         AND (@ImagePath IS NULL OR p.ImagePath LIKE '%' + @ImagePath + '%')
                         AND (@CategoryId IS NULL OR p.CategoryId = @CategoryId)
                         AND (@MinTotalStock IS NULL OR (p.Mirpur + p.Dhanmondi + p.Uttara + p.WIP) >= @MinTotalStock)
                         AND (@MaxTotalStock IS NULL OR (p.Mirpur + p.Dhanmondi + p.Uttara + p.WIP) <= @MaxTotalStock)
                         AND (@Status IS NULL OR p.Status = @Status)
                         AND (@TaxesId IS NULL OR p.TaxesId = @TaxesId)
                         AND (@BelowMinStock = 0 OR (p.Mirpur + p.Dhanmondi + p.Uttara + p.WIP) < p.InStock); -- Apply filter only if @BelowMinStock is 1

                       -- Select the Paginated Data with Additional Filters
                       DECLARE @sql NVARCHAR(MAX);
                       SET @sql = '
                           SELECT 
                               p.Id, 
                               p.ItemName, 
                               p.Barcode, 
                               p.ImagePath, 
                               c.Name AS CategoryName, 
                               p.Mirpur + p.Dhanmondi + p.Uttara + p.WIP AS TotalStock, 
                   			   p.InStock,
                               p.Status, 
                               t.Percentage AS TaxPercentage
                           FROM Products p
                           LEFT JOIN Categories c ON p.CategoryId = c.Id
                           LEFT JOIN Taxes t ON p.TaxesId = t.Id
                           WHERE (@ItemName IS NULL OR p.ItemName LIKE ''%'' + @ItemName + ''%'')
                             AND (@Barcode IS NULL OR p.Barcode LIKE ''%'' + @Barcode + ''%'')
                             AND (@ImagePath IS NULL OR p.ImagePath LIKE ''%'' + @ImagePath + ''%'')
                             AND (@CategoryId IS NULL OR p.CategoryId = @CategoryId)
                             AND (@MinTotalStock IS NULL OR (p.Mirpur + p.Dhanmondi + p.Uttara + p.WIP) >= @MinTotalStock)
                             AND (@MaxTotalStock IS NULL OR (p.Mirpur + p.Dhanmondi + p.Uttara + p.WIP) <= @MaxTotalStock)
                             AND (@Status IS NULL OR p.Status = @Status)
                             AND (@TaxesId IS NULL OR p.TaxesId = @TaxesId)
                             AND (@BelowMinStock = 0 OR (p.Mirpur + p.Dhanmondi + p.Uttara + p.WIP) < p.InStock)
                           ORDER BY ' + @OrderBy + '
                           OFFSET @PageSize * (@PageIndex - 1) ROWS
                           FETCH NEXT @PageSize ROWS ONLY;
                       ';

                       -- Define the Parameter List for sp_executesql
                       DECLARE @paramList NVARCHAR(MAX) = '@ItemName NVARCHAR(MAX), 
                                                            @Barcode NVARCHAR(MAX), 
                                                            @ImagePath NVARCHAR(MAX), 
                                                            @CategoryId UNIQUEIDENTIFIER, 
                                                            @MinTotalStock INT,
                                                            @MaxTotalStock INT,
                                                            @Status BIT,
                                                            @TaxesId UNIQUEIDENTIFIER,
                                                            @PageIndex INT, 
                                                            @PageSize INT,
                                                            @BelowMinStock BIT';

                       -- Execute the Paginated Query
                       EXEC sp_executesql 
                           @sql,
                           @paramList,
                           @ItemName = @ItemName,
                           @Barcode = @Barcode,
                           @ImagePath = @ImagePath,
                           @CategoryId = @CategoryId,
                           @MinTotalStock = @MinTotalStock,
                           @MaxTotalStock = @MaxTotalStock,
                           @Status = @Status,
                           @TaxesId = @TaxesId,
                           @PageIndex = @PageIndex,
                           @PageSize = @PageSize,
                           @BelowMinStock = @BelowMinStock;
                   END;
                   
                   """;

            migrationBuilder.Sql(sql);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = "DROP PROCEDURE [dbo].[GetAllProducts]";
            migrationBuilder.DropTable(sql);
        }
    }
}
