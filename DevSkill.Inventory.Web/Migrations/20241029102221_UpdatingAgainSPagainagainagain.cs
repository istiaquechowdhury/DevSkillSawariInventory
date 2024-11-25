using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevSkill.Inventory.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingAgainSPagainagainagain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                    CREATE OR ALTER PROCEDURE GetProducts 
                  	@PageIndex int,
                  	@PageSize int , 
                  	@OrderBy nvarchar(50),
                  	@ItemName nvarchar(max) = '%',
                    @Barcode nvarchar(max) = '%',
                	@ImagePath nvarchar(max) = '%',
                  	@CategoryId uniqueidentifier = NULL,
                  	@Total int output,
                  	@TotalDisplay int output

                AS
                BEGIN

                   	SET NOCOUNT ON;

                  		Declare @sql nvarchar(2000);
                  		Declare @countsql nvarchar(2000);
                  		Declare @paramList nvarchar(MAX); 
                  		Declare @countparamList nvarchar(MAX);


                  		-- Collecting Total
                    Select @Total = count(*) from Products;


                     			-- Collecting Total Display
                   	SET @countsql = 'select @TotalDisplay = count(*) from Products p inner join 
                                          					Categories c on p.CategoryId = c.Id where 1 = 1 ';


                               	SET @countsql = @countsql + ' AND p.ItemName LIKE ''%'' + @xItemName + ''%''' 

                               	SET @countsql = @countsql + ' AND p.Barcode LIKE ''%'' + @xBarcode + ''%''' 

                				SET @countsql = @countsql + ' AND p.ImagePath LIKE ''%'' + @xImagePath + ''%''' 

                                IF @CategoryId IS NOT NULL
                               	SET @countsql = @countsql + ' AND p.CategoryId = @xCategoryId' 

                           					SELECT @countparamlist = '@xItemName nvarchar(max),
                          				    @xBarcode nvarchar(max),
                							@xImagePath nvarchar(max),


                           					@xCategoryId uniqueidentifier,
                           					@TotalDisplay int output' ;

                           					exec sp_executesql @countsql , @countparamlist ,
                             					@ItemName,
                             					@Barcode,
                								@ImagePath,
                                                @CategoryId,
                             					@TotalDisplay = @TotalDisplay output;

                           					-- Collecting Data
                           					SET @sql = 'select p.Id,p.ItemName,p.Barcode,p.ImagePath,c.Name as CategoryName from Products p inner join 
                                			Categories c on p.CategoryId = c.Id where 1 = 1 ';


                               	SET @sql = @sql + ' AND p.ItemName LIKE ''%'' + @xItemName + ''%''' 

                               	SET @sql = @sql + ' AND p.Barcode LIKE ''%'' + @xBarcode + ''%''' 

                				SET @sql = @sql + ' AND p.ImagePath LIKE ''%'' + @xImagePath + ''%''' 


                           					IF @CategoryId IS NOT NULL
                           					SET @sql = @sql + ' AND p.CategoryId = @xCategoryId'

                           					SET @sql = @sql + ' Order by '+@OrderBy+' OFFSET @PageSize * (@PageIndex - 1) 
                           					ROWS FETCH NEXT @PageSize ROWS ONLY';

                           					SELECT @paramlist = '@xItemName nvarchar(max),
                             					@xBarcode nvarchar(max),
                								@xImagePath nvarchar(max),
                                                @xCategoryId uniqueidentifier,
                             					@PageIndex int,
                             					@PageSize int' ;

                        				    exec sp_executesql @sql , @paramlist ,
                             					@ItemName,
                             					@Barcode,
                								@ImagePath,
                                                @CategoryId,
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
