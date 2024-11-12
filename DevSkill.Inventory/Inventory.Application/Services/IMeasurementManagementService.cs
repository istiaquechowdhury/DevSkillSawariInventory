using Inventory.Domain;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public interface IMeasurementManagementService
    {
        void CreateMeasurement(MeasurementUnit measurement);
        void DeleteMeasurement(Guid id);
        MeasurementUnit GetMeasurement(Guid id);
        void UpdateMeasurement(MeasurementUnit measurement);
        (IList<MeasurementUnit> data, int total, int totaldisplay) GetMeasurements(int pageIndex, int pageSize, DataTablesSearch search, string? order);
        IList<MeasurementUnit> GetMeasurements();
    }
}
