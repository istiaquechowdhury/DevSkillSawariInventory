using Inventory.Domain;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public interface ICategoryManagementService
    {
        void CreateCategory(Category category);
        IList<Category> GetCategories();
        Category GetCategory(Guid categoryId);
        (IList<Category> data, int total, int totaldisplay) GetCategories(int pageIndex, int pageSize, DataTablesSearch search, string? order);
        void UpdateCategory(Category category);
        void DeleteCategory(Guid id);
       
    }
}
