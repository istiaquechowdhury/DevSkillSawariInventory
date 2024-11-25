using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Entities
{
    public class Product : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; }
        public string? Barcode { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal? AverageCost { get; set; }
        public decimal? Price { get; set; }
        public Guid? TaxesId { get; set; }
        public int? InStock { get; set; }
        public bool Status { get; set; }
        public Category? Category { get; set; }
        public Taxes? Taxes { get; set; }
        public Guid? MeasurementUnitId { get; set; }

        public MeasurementUnit? MeasurementUnit { get; set; }

        public string? Description { get; set; }
        public string? ImagePath { get; set; }

        public int Dhanmondi { get; set; }
        public int Mirpur { get; set; }
        public int Uttara { get; set; }
        public int WIP { get; set; }
        public int TotalInStock => Dhanmondi + Mirpur + Uttara + WIP;

        //Bind this to your DataTable or UI for total stock
        public int TotalWarehouseStock => TotalInStock;
        public decimal CostPerUnit { get; set; }

        public DateOnly date {  get; set; }

        public Product()
        {
            date = DateOnly.FromDateTime(DateTime.Now);
        }
        public List<StockList> Stocklists { get; set; }
     






    }
}
