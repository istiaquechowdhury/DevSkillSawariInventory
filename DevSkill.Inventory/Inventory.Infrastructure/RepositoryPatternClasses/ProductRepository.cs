using Inventory.Infrastructure.RepositoryPatternClasses;
using Inventory.Domain.Entities;
using Inventory.Domain.UnitOfWorkInterfaces;
using Inventory.Domain.RepositoryPatternInterfaces;
using Inventory.Infrastructure.InventoryDb;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Domain;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;



namespace Inventory.Infrastructure.RepositoryPatternClasses
{
    public class ProductRepository : Repository<Product,Guid>, IProductRepository
    {
        public ProductRepository(InventoryDbContext context) : base(context)
        {

        }

        public (IList<Product> data, int total, int totaldisplay) GetPagedProducts(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            //if (string.IsNullOrWhiteSpace(search.Value))
            //    return GetDynamic(null, order, y => y.Include(z => z.Category), pageIndex, pageSize, true);
            if (string.IsNullOrWhiteSpace(search.Value))
            {
                // If there's no search value, return all data with the specified order
                return GetDynamic(
                    null, // No filter
                    order,
                    y => y.Include(z => z.Category).Include(z=>z.Taxes),
                   
                    pageIndex,
                    pageSize,
                    true
                );
            }
            else
            {
                bool? statusFilter = null;

                // Determine if the search value is "Active" or "Inactive"
                if (search.Value.Equals("Active", StringComparison.OrdinalIgnoreCase))
                {
                    statusFilter = true;
                }
                else if (search.Value.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
                {
                    statusFilter = false;
                }

                return GetDynamic(
                    x => x.ItemName.Contains(search.Value) ||
                         x.Barcode.Contains(search.Value) ||
                         x.Category.Name.Contains(search.Value) ||
                         x.AverageCost.ToString().Contains(search.Value) ||
                         x.Price.ToString().Contains(search.Value) ||
                         x.Taxes.Percentage.ToString().Contains(search.Value) ||
                         x.InStock.ToString().Contains(search.Value) ||
                         x.ImagePath.Contains(search.Value) ||
                         (statusFilter.HasValue ? x.Status == statusFilter.Value : false),
                            order,
                            y => y.Include(z => z.Category).Include(z => z.Taxes),
                            pageIndex,
                            pageSize,
                            true
                );
            }

        }

        public Product GetProduct(Guid id)
        {
            return (Get(x => x.Id == id, y => y.Include(z => z.Category).Include(z => z.Taxes))).FirstOrDefault();
        }

        public bool IsTitleDuplicate(string title, Guid? id = null)
        {
            if (id.HasValue)
            {
                return GetCount(x => x.Id != id.Value && x.ItemName == title) > 0;
            }
            else
            {
                return GetCount(x => x.ItemName == title) > 0;
            }
        }
    }
}