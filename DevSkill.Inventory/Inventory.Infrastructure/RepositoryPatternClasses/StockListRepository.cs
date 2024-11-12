using Inventory.Domain;
using Inventory.Domain.Entities;
using Inventory.Domain.RepositoryPatternInterfaces;
using Inventory.Infrastructure.InventoryDb;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.RepositoryPatternClasses
{
    public class StockListRepository : Repository<StockList,Guid>,IStockListRepository
    {
        public StockListRepository(InventoryDbContext context) : base(context)
        {

        }

        public (IList<StockList> data, int total, int totaldisplay) GetPagedStockLists(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
            {
                // If there's no search value, return all data with the specified order
                return GetDynamic(
                    null, // No filter
                    order,
                    y => y.Include(z => z.Product).Include(z => z.Warehouse),

                    pageIndex,
                    pageSize,
                    true
                );
            }
            else
            {


                return GetDynamic(
                    x => x.Product.ItemName.Contains(search.Value) ||
                         x.Product.Barcode.Contains(search.Value) ||
                         x.Warehouse.Name.Contains(search.Value) ||
                         x.InHand.ToString().Contains(search.Value) ||
                         x.WeightedAvgCost.ToString().Contains(search.Value),

                            order,
                              y => y.Include(z => z.Product).Include(z => z.Warehouse),
                            pageIndex,
                            pageSize,
                            true
                );
            }
        }
    }
}
