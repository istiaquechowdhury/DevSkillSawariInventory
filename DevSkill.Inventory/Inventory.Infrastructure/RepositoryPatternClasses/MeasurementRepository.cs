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
    public class MeasurementRepository : Repository<MeasurementUnit,Guid>, IMeasurementRepository
    {
        public MeasurementRepository(InventoryDbContext context) : base(context)
        {

        }

        public (IList<MeasurementUnit> data, int total, int totaldisplay) GetPagedMeasurements(int pageIndex, int pageSize, DataTablesSearch search, string? order)
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
                    x => x.Name.Contains(search.Value)||
                         x.Symbol.Contains(search.Value),

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
