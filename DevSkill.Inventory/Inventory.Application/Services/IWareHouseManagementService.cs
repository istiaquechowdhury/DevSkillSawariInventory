using Inventory.Domain;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public interface IWareHouseManagementService
    {
        void CreateWareHouse(WareHouse wareHouse);
        WareHouse GetWareHouse(Guid id);
        (IList<WareHouse> data, int total, int totaldisplay) GetWareHouses(int pageIndex, int pageSize, DataTablesSearch search, string? order);
        Task<IList<WareHouse>> GetWareHousesAsync();
      
        void UpdateWareHouse(WareHouse wareHouse);

        Task<int> GetWareHouseCountAsync();
       
    }
}
