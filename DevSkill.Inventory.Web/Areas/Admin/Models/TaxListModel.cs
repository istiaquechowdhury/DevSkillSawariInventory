using Inventory.Domain;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class TaxListModel : DataTable
    {
        public int PageSize { get; set; } // Add a setter
        public int PageIndex { get; set; } // Add a setter
       
    }
}
