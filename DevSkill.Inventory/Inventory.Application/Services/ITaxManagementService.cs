using Inventory.Domain;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public interface ITaxManagementService
    {
        Taxes GetTax(Guid taxesId);
        (IList<Taxes> data, int total, int totaldisplay) GetTaxes(int pageIndex, int pageSize, DataTablesSearch search, string? order);
        IList<Taxes> GetTaxes();
    }
}
