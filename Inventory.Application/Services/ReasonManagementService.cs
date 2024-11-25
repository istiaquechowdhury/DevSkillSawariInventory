using Inventory.Domain.Entities;
using Inventory.Domain.UnitOfWorkInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Services
{
    public class ReasonManagementService : IReasonManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;

        public ReasonManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public void CreateReason(Reason reason)
        {
            _inventoryUnitOfWork.Reason.Add(reason);    
            _inventoryUnitOfWork.Save();    
        }

        public Reason GetReason(Guid reasonId)
        {
            return _inventoryUnitOfWork.Reason.GetById(reasonId);   
        }

        public async Task<IList<Reason>> GetReasonsAsync()
        {
            return await _inventoryUnitOfWork.Reason.GetAllAsync();
        }
    }
}
