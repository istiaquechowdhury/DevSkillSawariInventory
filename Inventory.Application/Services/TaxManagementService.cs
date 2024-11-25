using Inventory.Domain;
using Inventory.Domain.Entities;
using Inventory.Domain.UnitOfWorkInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public class TaxManagementService : ITaxManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

        public TaxManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public Taxes GetTax(Guid taxesId)
        {
            return _inventoryUnitOfWork.Tax.GetById(taxesId);
        }

        public (IList<Taxes> data, int total, int totaldisplay) GetTaxes(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return _inventoryUnitOfWork.Tax.GetPagedTaxes(pageIndex, pageSize, search, order);
        }

        public IList<Taxes> GetTaxes()
        {
            return _inventoryUnitOfWork.Tax.GetAll();
        }
    }
}
