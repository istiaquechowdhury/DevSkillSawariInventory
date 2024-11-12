using Inventory.Domain.Entities;
using Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class StockAdjustmentModel
    {
        [Display(Name = "Date")]
        [Required]
        public DateOnly dateOnly { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Display(Name = "Warehouse")]
        [Required]
        public Guid WareHouseId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Display(Name = "Reason")]
        [Required]
        public Guid ReasonId { get; set; }
    
        public string AdjustedBy { get; set; } = string.Empty;
        [Required]
        public string Note { get; set; }
        

        public IList<SelectListItem>? WareHouses { get; private set; }


        public void WareHouseValues(IList<WareHouse> wareHouses)
        {
            WareHouses = RazorUtility.SetWareHouseValues(wareHouses);
        }

        public IList<SelectListItem>? Reasons { get; private set; }


        public void ReasonValues(IList<Reason> reasons)
        {
            Reasons = RazorUtility.SetReasonValues(reasons);
        }




    }
}
