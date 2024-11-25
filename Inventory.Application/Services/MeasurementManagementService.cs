using Inventory.Domain;
using Inventory.Domain.Entities;
using Inventory.Domain.UnitOfWorkInterfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public class MeasurementManagementService : IMeasurementManagementService
    {

        private readonly IInventoryUnitOfWork _InventoryUnitOfWork;

        public MeasurementManagementService(IInventoryUnitOfWork InventoryUnitOfWork)
        {
            _InventoryUnitOfWork = InventoryUnitOfWork;
        }
        public void CreateMeasurement(MeasurementUnit measurement)
        {
            _InventoryUnitOfWork.Measurement.Add(measurement);
            _InventoryUnitOfWork.Save();    
        }

        public void DeleteMeasurement(Guid id)
        {
            _InventoryUnitOfWork.Measurement.Remove(id);
            _InventoryUnitOfWork.Save();
        }

        public (IList<MeasurementUnit> data, int total, int totaldisplay) GetMeasurements (int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return _InventoryUnitOfWork.Measurement.GetPagedMeasurements(pageIndex, pageSize, search, order);
        }

        public MeasurementUnit GetMeasurement(Guid id)
        {
            return _InventoryUnitOfWork.Measurement.GetById(id);    
        }

        public void UpdateMeasurement(MeasurementUnit measurement)
        {
            _InventoryUnitOfWork.Measurement.Edit(measurement);
            _InventoryUnitOfWork.Save();
        }

        public IList<MeasurementUnit> GetMeasurements()
        {
            return _InventoryUnitOfWork.Measurement.GetAll();   
        }
    }
}
