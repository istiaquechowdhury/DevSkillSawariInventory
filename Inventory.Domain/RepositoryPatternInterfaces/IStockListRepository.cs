using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.RepositoryPatternInterfaces
{
    public interface IStockListRepository : IRepositoryBase<StockList, Guid>
    {
        (IList<StockList> data, int total, int totaldisplay) GetPagedStockLists(int pageIndex, int pageSize, DataTablesSearch search, string? order);
    }
}
