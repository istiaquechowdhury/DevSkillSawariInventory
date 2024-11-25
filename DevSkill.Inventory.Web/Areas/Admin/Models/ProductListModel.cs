using Inventory.Domain;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class ProductListModel : DataTable
    {
        public ProductSearchDto SearchItem { get; set; }

        public IList<SelectListItem> Categories { get; private set; }

        public void SetCategoryValues(IList<Category> categories)
        {
            Categories = RazorUtility.SetCategoryValues(categories);
        }

        public IList<SelectListItem> Taxes { get; private set; }

        public void SetTaxValues(IList<Taxes> Tax)
        {
            Taxes = RazorUtility.SetTaxValues(Tax);
        }
        public IList<Product> Items { get; set; } // Adjust Product to your actual product type

    }
}
