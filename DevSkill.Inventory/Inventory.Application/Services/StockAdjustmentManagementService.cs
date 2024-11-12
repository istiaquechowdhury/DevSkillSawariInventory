using Inventory.Domain;
using Inventory.Domain.Entities;
using Inventory.Domain.RepositoryPatternInterfaces;
using Inventory.Domain.UnitOfWorkInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public class StockAdjustmentManagementService : IStockAdjustmentManagementService
    {
        private readonly IInventoryUnitOfWork _InventoryUnitOfWork;

        public StockAdjustmentManagementService(IInventoryUnitOfWork InventoryUnitOfWork)
        {
            _InventoryUnitOfWork = InventoryUnitOfWork;
        }
        public void CreateStockAdjustment(StockAdjustment stockAdjust)
        {
            _InventoryUnitOfWork.StockAdjustment.Add(stockAdjust);  
            _InventoryUnitOfWork.Save(); 
        }

        public void DeleteStockAdjustment(Guid id)
        {
           _InventoryUnitOfWork.StockAdjustment.Remove(id); 
            _InventoryUnitOfWork.Save();
        }

        public (IList<StockAdjustment> data, int total, int totaldisplay) GetStockAdjustments(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return _InventoryUnitOfWork.StockAdjustment.GetPagedStockAdjustments(pageIndex, pageSize, search, order);
        }
    }
}
