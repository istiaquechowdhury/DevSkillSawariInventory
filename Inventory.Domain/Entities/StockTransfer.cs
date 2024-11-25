using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class StockTransfer : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public DateOnly Date {  get; set; } 

        public Guid WareHouseId { get; set; }   

        public WareHouse WareHouse { get; set; }

        public Guid DestinationWarehouseId { get; set; }    

        public WareHouse DestinationWarehouse { get; set; }

        public string TransferBy {  get; set; }

        public string Note { get; set; }

        public Guid ProductId { get; set; } 

        public Product Product { get; set; }
        
        public int TransferredQuantity { get; set; }   
        
        

        public StockTransfer()
        {
            Date = DateOnly.FromDateTime(DateTime.Now);
        }

    }
}
