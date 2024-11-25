using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Inventory.Domain.Entities
{
    public class StockAdjustment:IEntity<Guid>
    {
        public Guid Id { get; set; }

        public DateOnly dateOnly { get; set; }  

        public Guid ProductId { get; set; }     

        public Product Product { get; set; }    

        public Guid WareHouseId { get; set; }   

        public WareHouse WareHouse { get; set; }    

        public int Quantity { get; set; }   

        public Guid ReasonId { get; set; }

        public Reason Reason { get; set; }  

        public string AdjustedBy { get; set; }  

        public string Note {  get; set; }

        public StockAdjustment()
        {
            dateOnly = DateOnly.FromDateTime(DateTime.Now);
        }
    }
}
