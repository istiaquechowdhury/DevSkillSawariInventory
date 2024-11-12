using Inventory.Domain;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public interface IStockTransferManagementService
    {
        void CreateStockTransfer(StockTransfer stockTransfer);
        void DeleteStockTransfer(Guid id);
        (IList<StockTransfer> data, int total, int totaldisplay) GetStockTransfers(int pageIndex, int pageSize, DataTablesSearch search, string? order);

    }
}
