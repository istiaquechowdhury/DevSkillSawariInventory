using Inventory.Domain;
using Inventory.Domain.Entities;
using Inventory.Domain.RepositoryPatternInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public interface IStockAdjustmentManagementService
    {
        (IList<StockAdjustment> data, int total, int totaldisplay) GetStockAdjustments(int pageIndex, int pageSize, DataTablesSearch search, string? order);

        void CreateStockAdjustment(StockAdjustment stockAdjust);
        void DeleteStockAdjustment(Guid id);
    }
}
