using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Domain.RepositoryPatternInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.UnitOfWorkInterfaces
{
    public interface IInventoryUnitOfWork : IUnitOfWork   
    {
        public IProductRepository Product { get; }   

        public ICategoryRepository Category { get; }

        public ITaxRepository Tax { get; }

        public IWareHouseRepository WareHouse { get; }

        public IStockListRepository StockList { get; }

        public IStockTransferRepository StockTransfer { get; }

        public IStockAdjustmentRepository StockAdjustment { get; }

        public IReasonRepository Reason { get; }    

        public IMeasurementRepository Measurement { get; }

        Task<(IList<ProductDto> data, int total, int totaldisplay)> GetPagedProductsUsingSPAsync(int pageIndex, int pageSize, ProductSearchDto search, string? order);
        Task<(IList<StockListDto> data, int total, int totaldisplay)> GetPagedStocklistUsingSPAsync(int pageIndex, int pageSize, StockListSearchDto search, string? order);
    }
}
