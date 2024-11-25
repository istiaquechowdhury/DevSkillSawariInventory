using Inventory.Domain.Entities;
using Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class StockTransferModel
    {
        public DateOnly Date { get; set; }

        [Display(Name = "WareHouse From")]
        public Guid WareHouseId { get; set; }
        [Display(Name = "Warehouse To")]
        public Guid DestinationWareHouseId { get; set; }
        public string TransferBy { get; set; } = string.Empty;  

        public string Note { get; set; }

        public Guid ProductId { get; set; } 

        public int TransferredQuantity { get; set; }    

        public IList<SelectListItem>? WareHouses { get; private set; }


        public void SetWareHouseValues(IList<WareHouse> wareHouses)
        {
            WareHouses = RazorUtility.SetWareHouseValues(wareHouses);
        }


    }
}
