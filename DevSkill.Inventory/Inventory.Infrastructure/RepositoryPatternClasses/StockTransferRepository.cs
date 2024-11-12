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
    public class StockTransferRepository : Repository<StockTransfer, Guid>, IStockTransferRepository
    {
        public StockTransferRepository(InventoryDbContext context) : base(context)
        {
        }

        public (IList<StockTransfer> data, int total, int totaldisplay) GetPagedStockTransfers(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
            {
                // If there's no search value, return all data with the specified order
                return GetDynamic(
                    null, // No filter
                    order,
                    y => y.Include(z => z.WareHouse).Include(z => z.DestinationWarehouse).Include(z => z.Product),

                    pageIndex,
                    pageSize,
                    true
                );
            }
            else
            {
                DateTime parsedDate;
                bool isDateParsed = DateTime.TryParse(search.Value, out parsedDate);

                return GetDynamic(
                    x => (
                        isDateParsed ?
                        x.Date.Year.ToString().Contains(parsedDate.Year.ToString()) ||    // Match year
                        x.Date.Month.ToString().Contains(parsedDate.Month.ToString()) ||  // Match month
                        x.Date.Day.ToString().Contains(parsedDate.Day.ToString())         // Match day
                        : false  // Only compare if parsing succeeds
                    ) ||
                    x.WareHouse.Name.Contains(search.Value) ||
                    x.DestinationWarehouse.Name.Contains(search.Value) ||
                    x.TransferBy.Contains(search.Value) ||
                    x.Note.Contains(search.Value),


                            order,
                             y => y.Include(z => z.WareHouse).Include(z => z.DestinationWarehouse).Include(z => z.Product),
                            pageIndex,
                            pageSize,
                            true
                );
            }

        }
    }
}
