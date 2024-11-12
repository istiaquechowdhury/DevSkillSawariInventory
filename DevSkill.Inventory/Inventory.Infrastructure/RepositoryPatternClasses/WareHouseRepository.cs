using Inventory.Domain;
using Inventory.Domain.Entities;
using Inventory.Domain.RepositoryPatternInterfaces;
using Inventory.Infrastructure.InventoryDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.RepositoryPatternClasses
{
    public class WareHouseRepository : Repository<WareHouse, Guid>, IWareHouseRepository
    {
        public WareHouseRepository(InventoryDbContext context) : base(context)
        {


        }

        public (IList<WareHouse> data, int total, int totaldisplay) GetPagedWareHouses(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
            {
                // If there's no search value, return all data with the specified order
                return GetDynamic(
                    null, // No filter
                    order,
                    null,
                    pageIndex,
                    pageSize,
                    true
                );
            }
            else
            {


                return GetDynamic(
                    x => x.Name.Contains(search.Value),

                    order,
                    null,
                    pageIndex,
                    pageSize,
                    true
                );
            }

        }

        public bool IsTitleDuplicate(string title, Guid? id = null)
        {
            if (id.HasValue)
            {
                return GetCount(x => x.Id != id.Value && x.Name == title) > 0;
            }
            else
            {
                return GetCount(x => x.Name == title) > 0;
            }
        }


    }
}
