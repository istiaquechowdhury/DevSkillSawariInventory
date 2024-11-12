using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Dtos
{
    public class ProductDto
    {
        public Guid Id { get; set;}

        public string ImagePath { get; set; }
        public string ItemName{ get; set;}

        public string Barcode { get; set;}


        public string CategoryName { get; set;}

        public Decimal TaxPercentage { get; set;}   

        public int TotalStock { get; set;}  

        public bool Status {  get; set;}   
        
        public int InStock { get; set;}     









    }
}
