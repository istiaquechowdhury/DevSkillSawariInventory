using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.RepositoryPatternInterfaces
{
    public interface IStockTransferRepository : IRepositoryBase<StockTransfer, Guid>
    {
        (IList<StockTransfer> data, int total, int totaldisplay) GetPagedStockTransfers(int pageIndex, int pageSize, DataTablesSearch search, string? order);
    }
}
