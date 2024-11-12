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
    public class CategoryManagementService : ICategoryManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

        public CategoryManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;   
        }

        public void CreateCategory(Category category)
        {
            if (!_inventoryUnitOfWork.Category.IsTitleDuplicate(category.Name))
            {
                _inventoryUnitOfWork.Category.Add(category);
                _inventoryUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Name should be unique.");
           
        }

        public IList<Category> GetCategories()
        {
            return _inventoryUnitOfWork.Category.GetAll();
        }

        public Category GetCategory(Guid categoryId)
        {
            return _inventoryUnitOfWork.Category.GetById(categoryId);
        }

        public (IList<Category> data, int total, int totaldisplay) GetCategories(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return _inventoryUnitOfWork.Category.GetPagedCategories(pageIndex, pageSize, search, order);
        }

        public void UpdateCategory(Category category)
        {
            if (!_inventoryUnitOfWork.Category.IsTitleDuplicate(category.Name,category.Id))
            {
                _inventoryUnitOfWork.Category.Edit(category);
                _inventoryUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Name should be unique.");
            
        }

        public void DeleteCategory(Guid id)
        {
            _inventoryUnitOfWork.Category.Remove(id);
            _inventoryUnitOfWork.Save();
        }

       
      

      
    }
}
