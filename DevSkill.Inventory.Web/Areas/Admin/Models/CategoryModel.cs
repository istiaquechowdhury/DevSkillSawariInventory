using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class CategoryModel
    {
        [Required]
        public string Name { get; set; }        
    }
}
