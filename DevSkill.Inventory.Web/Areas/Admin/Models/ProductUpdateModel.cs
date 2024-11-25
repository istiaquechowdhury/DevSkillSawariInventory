using Inventory.Domain.Entities;
using Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class ProductUpdateModel
    {
       
        public Guid Id { get; set; }
        [Required]
        public string ItemName { get; set; }
        [Display(Name = "")]
        public string? Barcode { get; set; }

        [Display(Name = "")]
        public Guid? CategoryId { get; set; }
        public decimal? AverageCost { get; set; }
        public decimal? Price { get; set; }
        [Display(Name = "")]
        public Guid? TaxesId { get; set; }
        [Display(Name = "")]
        public int? InStock { get; set; }
        public bool Status { get; set; }
        [Display(Name = "")]
        public Guid? MeasurementUnitId { get; set; }
        [Display(Name = "")]
        public string? Description { get; set; }
        public string? ImagePath { get; set; }

        public IFormFile? ImageFile { get; set; }

        public bool RemoveImage { get; set; }
        public int Dhanmondi { get; set; }
        public int Mirpur { get; set; }
        public int Uttara { get; set; }
        public int WIP { get; set; }
        public int TotalInStock => Dhanmondi + Mirpur + Uttara + WIP;

        // Bind this to your DataTable or UI for total stock
        public int? TotalWarehouseStock => TotalInStock;


        public decimal CostPerUnit { get; set; }

        public DateOnly date { get; set; }




        public IList<SelectListItem>? Categories { get; private set; }


        public void SetCategoryValues(IList<Category> categories)
        {
            Categories = RazorUtility.SetCategoryValues(categories);
        }

        public IList<SelectListItem>? Tx { get; private set; }


        public void SetTaxValues(IList<Taxes> categories)
        {
            Tx = RazorUtility.SetTaxValues(categories);
        }

        public IList<SelectListItem>? Measurement { get; private set; }


        public void SetMeasureValues(IList<MeasurementUnit> measure)
        {
            Measurement = RazorUtility.SetMeasurementvalues(measure);
        }

    }
}
