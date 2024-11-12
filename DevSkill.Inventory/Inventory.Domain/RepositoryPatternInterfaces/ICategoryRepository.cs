using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.RepositoryPatternInterfaces
{
    public interface ICategoryRepository : IRepositoryBase<Category, Guid>
    {
        (IList<Category> data, int total, int totaldisplay) GetPagedCategories(int pageIndex, int pageSize, DataTablesSearch search, string? order);
        bool IsTitleDuplicate(string title, Guid? id = null);
    }
}
