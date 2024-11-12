using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.RepositoryPatternInterfaces
{
    public interface IWareHouseRepository : IRepositoryBase<WareHouse, Guid>
    {
        (IList<WareHouse> data, int total, int totaldisplay) GetPagedWareHouses(int pageIndex, int pageSize, DataTablesSearch search, string? order);
        bool IsTitleDuplicate(string title, Guid? id = null);
    }
}
