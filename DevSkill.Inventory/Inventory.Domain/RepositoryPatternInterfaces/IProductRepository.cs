using Inventory.Domain.UnitOfWorkInterfaces;
using Inventory.Domain.RepositoryPatternInterfaces;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Domain;

namespace Inventory.Domain.RepositoryPatternInterfaces
{
    public interface IProductRepository : IRepositoryBase<Product, Guid>
    {
        (IList<Product> data, int total, int totaldisplay) GetPagedProducts(int pageIndex, int pageSize, DataTablesSearch search, string? order);
        Product GetProduct(Guid id);
        public bool IsTitleDuplicate(string title, Guid? id = null);
    }
}
