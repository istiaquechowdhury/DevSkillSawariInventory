using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.RepositoryPatternInterfaces
{
    public interface IMeasurementRepository : IRepositoryBase<MeasurementUnit, Guid>
    {
        (IList<MeasurementUnit> data, int total, int totaldisplay) GetPagedMeasurements(int pageIndex, int pageSize, DataTablesSearch search, string? order);
    }
}
