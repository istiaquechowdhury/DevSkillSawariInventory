using Inventory.Domain;
using Inventory.Domain.Entities;
using Inventory.Domain.RepositoryPatternInterfaces;
using Inventory.Infrastructure.InventoryDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.RepositoryPatternClasses
{
    public class TaxRepository : Repository<Taxes, Guid>, ITaxRepository
    {
        public TaxRepository(InventoryDbContext context) : base(context)
        {

        }

        public (IList<Taxes> data, int total, int totaldisplay) GetPagedTaxes(int pageIndex, int pageSize, DataTablesSearch search, string? order)
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
                //bool? statusFilter = null;

                //// Determine if the search value is "Active" or "Inactive"
                //if (search.Value.Equals("Active", StringComparison.OrdinalIgnoreCase))
                //{
                //    statusFilter = true;
                //}
                //else if (search.Value.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
                //{
                //    statusFilter = false;
                //}

                return GetDynamic(
                    x => x.Name.Contains(search.Value)||
                         x.Percentage.ToString().Contains(search.Value),

                    order,
                    null,
                    pageIndex,
                    pageSize,
                    true
                );
            }
        }
    }
}
