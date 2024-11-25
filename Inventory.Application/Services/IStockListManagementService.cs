using Inventory.Domain;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public interface IStockListManagementService
    {
        (IList<StockList> data, int total, int totaldisplay) GetStockLists(int pageIndex, int pageSize, DataTablesSearch search, string? order);

        Task<int> GetStockListCountAsync();

        Task<(IList<StockListDto> data, int total, int totaldisplay)> GetStockListsSP(int pageIndex, int pageSize, StockListSearchDto search, string? order);

    }
}
