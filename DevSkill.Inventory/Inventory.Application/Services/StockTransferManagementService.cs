using Inventory.Domain;
using Inventory.Domain.Entities;
using Inventory.Domain.UnitOfWorkInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public class StockTransferManagementService : IStockTransferManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

        public StockTransferManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }
        public void CreateStockTransfer(StockTransfer stockTransfer)
        {
            _inventoryUnitOfWork.StockTransfer.Add(stockTransfer);
            _inventoryUnitOfWork.Save();
        }

        public void DeleteStockTransfer(Guid id)
        {
             _inventoryUnitOfWork.StockTransfer.Remove(id);   
            _inventoryUnitOfWork.Save();

        }

        public (IList<StockTransfer> data, int total, int totaldisplay) GetStockTransfers(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return _inventoryUnitOfWork.StockTransfer.GetPagedStockTransfers(pageIndex, pageSize, search, order);
        }
    }
}
