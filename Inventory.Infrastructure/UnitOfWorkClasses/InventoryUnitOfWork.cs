using Inventory.Application;
using Inventory.Domain;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Domain.RepositoryPatternInterfaces;
using Inventory.Domain.UnitOfWorkInterfaces;
using Inventory.Infrastructure.InventoryDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.UnitOfWorkClasses
{
    public class InventoryUnitOfWork : UnitOfWork, IInventoryUnitOfWork
    {
        public IProductRepository Product { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public ITaxRepository Tax { get; private set; }

        public IWareHouseRepository WareHouse { get; private set; }

        public IStockListRepository StockList {  get; private set; }

        public IStockTransferRepository StockTransfer {  get; private set; }

        public IStockAdjustmentRepository StockAdjustment { get; private set; }

        public IReasonRepository Reason { get; private set; }

        public IMeasurementRepository Measurement { get; private set; }

        public InventoryUnitOfWork (InventoryDbContext dbContext,
            IProductRepository _Product,ICategoryRepository _Category, ITaxRepository _Tax,IWareHouseRepository _wareHouse,
           IStockListRepository _StockList,IStockTransferRepository _stockTransfer,IStockAdjustmentRepository _stockAdjustment,
            IReasonRepository _Reason, IMeasurementRepository _Measurement) : base(dbContext)
        {
            Product = _Product;
            Category = _Category;
            Tax = _Tax; 
            WareHouse = _wareHouse;
            StockList = _StockList;
            StockTransfer = _stockTransfer;
            StockAdjustment = _stockAdjustment;
            Reason = _Reason;
            Measurement = _Measurement; 


            
        }

        public async Task< (IList<ProductDto> data, int total, int totaldisplay)> GetPagedProductsUsingSPAsync(int pageIndex, int pageSize, ProductSearchDto search, string? order)
        {
            var Procedurename = "GetAllProducts";
            var result = await SqlUtility.QueryWithStoredProcedureAsync<ProductDto>
            (Procedurename, new Dictionary<string, object>
            {
                {"PageIndex", pageIndex},
                {"PageSize", pageSize},
                {"OrderBy", order},
                {"ItemName", string.IsNullOrEmpty(search.ItemName) ? null : search.ItemName },
                {"Barcode", string.IsNullOrEmpty(search.Barcode) ? null : search.Barcode },
                {"CategoryId", string.IsNullOrEmpty(search.CategoryId) ? null : Guid.Parse(search.CategoryId) },
                {"MinTotalStock", string.IsNullOrEmpty(search.MinTotalStock) ? null : search.MinTotalStock },
                {"MaxTotalStock", string.IsNullOrEmpty(search.MaxTotalStock) ? null : search.MaxTotalStock },
                {"Status", string.IsNullOrEmpty(search.Status) ? null : search.Status },
                {"TaxesId", string.IsNullOrEmpty(search.TaxesId) ? null : search.TaxesId },
                {"BelowMinStock", string.IsNullOrEmpty(search.BelowMinStock) ? null : search.BelowMinStock }



            },
            new Dictionary<string,Type>
            {
                {"Total",typeof(int)},
                {"TotalDisplay",typeof (int)},  
            });

            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);



        }
        public async Task<(IList<StockListDto> data, int total, int totaldisplay)> GetPagedStocklistUsingSPAsync(int pageIndex, int pageSize, StockListSearchDto search, string? order)
        {
            var Procedurename = "GetStockList";
            var result = await SqlUtility.QueryWithStoredProcedureAsync<StockListDto>
            (Procedurename, new Dictionary<string, object>
            {
                {"PageIndex", pageIndex},
                {"PageSize", pageSize},
                {"OrderBy", order},
                {"ItemName", string.IsNullOrEmpty(search.ItemName) ? null : search.ItemName },
                {"Barcode", string.IsNullOrEmpty(search.Barcode) ? null : search.Barcode },
                {"WarehouseId", string.IsNullOrEmpty(search.WarehouseId) ? null : Guid.Parse(search.WarehouseId) },
                {"BelowMinStock", string.IsNullOrEmpty(search.BelowMinStock) ? null : search.BelowMinStock },
                {"MinInHand", string.IsNullOrEmpty(search.MinInHand) ? null : search.MinInHand },
                {"MaxInHand", string.IsNullOrEmpty(search.MaxInHand) ? null : search.MaxInHand },
                {"MinWeightedAvgCost", string.IsNullOrEmpty(search.MinWeightedAvgCost) ? null : search.MinWeightedAvgCost },
                {"MaxWeightedAvgCost", string.IsNullOrEmpty(search.MaxWeightedAvgCost) ? null : search.MaxWeightedAvgCost },




            },
            new Dictionary<string, Type>
            {
                {"Total",typeof(int)},
                {"TotalDisplay",typeof (int)},
            });

            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }
    }
}
