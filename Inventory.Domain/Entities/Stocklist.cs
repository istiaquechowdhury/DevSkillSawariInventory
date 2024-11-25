using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class StockList : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; } 

        public Guid WarehouseId { get; set; } 
        
        public int InHand { get; set; } 

        public decimal WeightedAvgCost { get; set; }

        public Product Product { get; set; }

        public WareHouse Warehouse { get; set; }
    }
}
