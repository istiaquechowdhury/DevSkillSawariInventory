using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Dtos
{
    public class ProductSearchDto
    {
        public string ItemName { get; set; }
       
        public string CategoryId { get; set; }
        public string Barcode { get; set; }

        public string MinTotalStock { get; set; }  

        public string MaxTotalStock { get; set; }

        public string Status { get; set; }

        public string TaxesId { get; set; }

        public string BelowMinStock { get; set; }




    }
}
