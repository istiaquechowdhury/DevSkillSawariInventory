using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Dtos
{
    public class StockListDto
    {
        public Guid Id { get; set; }    

        public string ItemName { get; set; }    

        public string Barcode { get; set; } 

        public string WareHouseName { get; set; }   

        public int InHand { get; set; }


        public Decimal WeightedAvgCost { get; set; }

        public int InStock { get; set; }    
    }
}
