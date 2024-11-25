using System.ComponentModel.DataAnnotations;

namespace DevSkill.Inventory.Web.Areas.Admin.Models
{
    public class CategoryUpdateModel
    {
        public Guid id { get; set; }

        [Required]
        public string Name { get; set; }    
    }
}
