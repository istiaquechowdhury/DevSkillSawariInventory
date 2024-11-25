using Inventory.Domain;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Domain.UnitOfWorkInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public class StockListManagementService : IStockListManagementService
    {
        private readonly IInventoryUnitOfWork _InventoryUnitOfWork;


        public StockListManagementService(IInventoryUnitOfWork InventoryUnitOfWork)
        {
            _InventoryUnitOfWork = InventoryUnitOfWork;
        }

        public async Task<int> GetStockListCountAsync()
        {
            return await _InventoryUnitOfWork.StockList.GetCountAsync();  
        }

        public (IList<StockList> data, int total, int totaldisplay) GetStockLists(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return _InventoryUnitOfWork.StockList.GetPagedStockLists(pageIndex, pageSize, search, order);


        }

        public async Task<(IList<StockListDto> data, int total, int totaldisplay)> GetStockListsSP(int pageIndex, int pageSize, StockListSearchDto search, string? order)
        {
            return await _InventoryUnitOfWork.GetPagedStocklistUsingSPAsync(pageIndex, pageSize, search, order);
        }
    }
}
