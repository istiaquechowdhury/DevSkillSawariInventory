using Inventory.Domain;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class StockListModel : DataTable
    {
        public StockListSearchDto SearchItem { get; set; }

        public IList<SelectListItem> WareHouses { get; private set; }

        public void SetWareHouseValues(IList<WareHouse> warehouses)
        {
            WareHouses = RazorUtility.SetWareHouseValues(warehouses);
        }

    }
}
