using Inventory.Domain.Entities;
using Inventory.Domain.RepositoryPatternInterfaces;
using Inventory.Infrastructure.InventoryDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.RepositoryPatternClasses
{
    public class ReasonRepository : Repository<Reason,Guid>, IReasonRepository
    {
        public ReasonRepository(InventoryDbContext context) : base(context)
        {

        }
    }
}
