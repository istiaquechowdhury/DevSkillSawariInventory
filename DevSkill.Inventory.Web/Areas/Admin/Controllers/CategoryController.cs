using AutoMapper;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Inventory.Application.Services;
using Inventory.Domain;
using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryManagementService _categoryManagementService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        public CategoryController(ICategoryManagementService categoryManagementServices, IMapper mapper, ILogger<ProductController> logger)
        {
            _categoryManagementService = categoryManagementServices;
            _mapper = mapper;   
            _logger = logger;
        }
        public IActionResult Index()
        {
           return View();   
           
        }
       
        public JsonResult GetCategoryData([FromBody] CategoryListModel model)
        {
         
            var result = _categoryManagementService.GetCategories(
                model.PageIndex,
                model.PageSize,
                model.Search,
                model.FormatSortExpression("Name")
            );

          
            if (result.data == null || result.data.Count == 0)
            {
                return Json(new { success = false, message = "No data available" });
            }

            var categoryJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totaldisplay,
                data = result.data.Select(record => new string[]
                {
                        HttpUtility.HtmlEncode(record.Name),
                        record.Id.ToString()
                }).ToArray()
            };

            return Json(categoryJsonData);
        }
      
        public IActionResult Create()
        {
            return View();
        }


    
        [HttpPost]

        [Authorize(Roles = "Admin")]
        public JsonResult Create(CategoryModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                var category = _mapper.Map<Category>(categoryModel);
                _categoryManagementService.CreateCategory(category);

                return Json(new { success = true });

            }
            return Json(new { success = false, message = "Invalid data" });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Update(Guid id)
        {
            Category category = _categoryManagementService.GetCategory(id);
            var model = _mapper.Map<CategoryUpdateModel>(category);

            return View(model);
        }



       
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult Update(CategoryUpdateModel categoryUpdateModel)
        {
            if (ModelState.IsValid)
            {
                var category = _categoryManagementService.GetCategory(categoryUpdateModel.id);
                category = _mapper.Map(categoryUpdateModel, category);
                _categoryManagementService.UpdateCategory(category);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Invalid data" });
        }

        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult Delete(Guid id)
        {
            try
            {
                _categoryManagementService.DeleteCategory(id);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
               
                Log.Error(ex, "Error while deleting category with ID {CategoryId}", id);
                return Json(new { success = false, message = "Error while deleting" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetCategoryById(Guid id)
        {
            var category = _categoryManagementService.GetCategory(id);
            if (category != null)
            {
                return Json(new { id = category.Id, name = category.Name });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetAll()
        {
            var categories = _categoryManagementService.GetCategories();
            return Json(new { data = categories }); // Return data in JSON format for DataTable or API usage

        }

        [HttpGet]
        public JsonResult IsUserAdmin()
        {
            // Check if the current user is in the "Admin" role
            bool isAdmin = User.IsInRole("Admin");
            return Json(isAdmin);
        }





    }
}
