using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Dtos
{
    public class StockListSearchDto
    {
        public string ItemName { get; set; }

        public string Barcode { get; set; }

        public string WarehouseId { get; set; }


        public string BelowMinStock { get; set; }
        public string MinInHand { get; set; }

        public string MaxInHand { get; set; }

        public string MinWeightedAvgCost { get; set; }

        public string MaxWeightedAvgCost { get; set; }

    }
}
