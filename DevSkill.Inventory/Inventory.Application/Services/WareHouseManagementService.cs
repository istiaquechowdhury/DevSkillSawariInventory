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
    public class WareHouseManagementService : IWareHouseManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

        public WareHouseManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public void CreateWareHouse(WareHouse wareHouse)
        {
            if (!_inventoryUnitOfWork.WareHouse.IsTitleDuplicate(wareHouse.Name))
            {
                _inventoryUnitOfWork.WareHouse.Add(wareHouse);
                _inventoryUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Name should be unique.");
        }

        public WareHouse GetWareHouse(Guid id)
        {
            return _inventoryUnitOfWork.WareHouse.GetById(id);  
        }

        public async Task<int> GetWareHouseCountAsync()
        {
            return await _inventoryUnitOfWork.WareHouse.GetCountAsync();
        }

        public (IList<WareHouse> data, int total, int totaldisplay) GetWareHouses(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return _inventoryUnitOfWork.WareHouse.GetPagedWareHouses(pageIndex, pageSize, search, order);
        }

        public async Task<IList<WareHouse>> GetWareHousesAsync()
        {
            return await _inventoryUnitOfWork.WareHouse.GetAllAsync();
        }

       

        public void UpdateWareHouse(WareHouse wareHouse)
        {
            if (!_inventoryUnitOfWork.WareHouse.IsTitleDuplicate(wareHouse.Name,wareHouse.Id))
            {
                _inventoryUnitOfWork.WareHouse.Edit(wareHouse);
                _inventoryUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Name should be unique.");
        }
    }
}
