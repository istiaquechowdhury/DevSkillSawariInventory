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
    public class StockAdjustmentRepository : Repository<StockAdjustment,Guid>, IStockAdjustmentRepository
    {
        public StockAdjustmentRepository(InventoryDbContext context) : base(context)
        {

        }

        public (IList<StockAdjustment> data, int total, int totaldisplay) GetPagedStockAdjustments(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
            {
                // If there's no search value, return all data with the specified order
                return GetDynamic(
                    null, // No filter
                    order,
                    y => y.Include(z => z.Product).Include(z => z.WareHouse).Include(z=> z.Reason),

                    pageIndex,
                    pageSize,
                    true
                );
            }
            else
            {
                

                return GetDynamic(
                    x => x.dateOnly.ToString().Contains(search.Value) ||
                         x.Product.ItemName.Contains(search.Value) ||
                         x.WareHouse.Name.Contains(search.Value) ||
                         x.Quantity.ToString().Contains(search.Value) ||
                         x.Reason.Name.Contains(search.Value) ||
                         x.AdjustedBy.Contains(search.Value) ||
                         x.Note.Contains(search.Value),
                        
                            order,
                              y => y.Include(z => z.Product).Include(z => z.WareHouse).Include(z => z.Reason),
                            pageIndex,
                            pageSize,
                            true
                );
            }

        }
    }
}
