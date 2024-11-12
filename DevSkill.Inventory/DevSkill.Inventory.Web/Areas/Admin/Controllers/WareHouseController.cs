using AutoMapper;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Inventory.Application.Services;
using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]

    public class WareHouseController : Controller
    {
        private readonly ICategoryManagementService _categoryManagementService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        private readonly IWareHouseManagementService _warehouseManagementService;   
        public WareHouseController(ICategoryManagementService categoryManagementServices, IMapper mapper, ILogger<ProductController> logger, IWareHouseManagementService warehouseManagementService)
        {
            _categoryManagementService = categoryManagementServices;
            _mapper = mapper;
            _logger = logger;
            _warehouseManagementService = warehouseManagementService;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        //public JsonResult GetBlogPostJsonData([FromBody] WareHouseListModel model)
        //{

        //    var result = _warehouseManagementService.GetWareHouses(model.PageIndex, model.PageSize, model.Search, model.FormatSortExpression("Name"));
        //    var blogPostJsonData = new
        //    {
        //        recordsTotal = result.total,
        //        recordsFiltered = result.totaldisplay,
        //        data = (from record in result.data
        //                select new string[]
        //                {
        //                 HttpUtility.HtmlEncode(record.Name),

        //                 record.Id.ToString()
        //                }
        //            ).ToArray()
        //    };

        //    return Json(blogPostJsonData);

        //}

        //public IActionResult Create()
        //{
        //    return View();
        //}


        //[HttpPost, ValidateAntiForgeryToken]

        //public IActionResult Create(WareHouseModel Model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var wareHouse = _mapper.Map<WareHouse>(Model);
        //        try
        //        {

        //            _warehouseManagementService.CreateWareHouse(wareHouse);
        //            TempData["success"] = "Data inserted successfully";

        //            return RedirectToAction("Index");
        //        }
        //        catch (Exception ex)
        //        {
        //            TempData["error"] = "Data insert falied,Data should be unique";
        //            _logger.LogError(ex, "Blog post creation failed");

        //        }


        //        return RedirectToAction("Index");
        //    }

        //    return View(Model);
        //}
        //public IActionResult Update(Guid id)
        //{
        //    WareHouse wareHouse = _warehouseManagementService.GetWareHouse(id);
        //    var model = _mapper.Map<WareHouseUpdateModel>(wareHouse);

        //    return View(model);
        //}



        //[HttpPost, ValidateAntiForgeryToken]

        //public IActionResult Update(WareHouseUpdateModel updateModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var wareHouse = _warehouseManagementService.GetWareHouse(updateModel.id);
        //        wareHouse = _mapper.Map(updateModel, wareHouse);


        //        try
        //        {
        //            _warehouseManagementService.UpdateWareHouse(wareHouse);
        //            TempData["success"] = "Data updated Successfully";

        //            return RedirectToAction("Index");
        //        }
        //        catch (Exception ex)
        //        {
        //            TempData["error"] = "Data update falied,Data should be unique";

        //            _logger.LogError(ex, "Blog post creation failed");

        //            return RedirectToAction("Index");


        //        }

        //    }

        //    return View(updateModel);
        //}


        public IActionResult Index()
        {
            return View();

        }

        public JsonResult GetWarehouseData([FromBody] WareHouseListModel model)
        {

            var result = _warehouseManagementService.GetWareHouses(
                model.PageIndex,
                model.PageSize,
                model.Search,
                model.FormatSortExpression("Name")
            );


            if (result.data == null || result.data.Count == 0)
            {
                return Json(new { success = false, message = "No data available" });
            }

            var WarehouseJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totaldisplay,
                data = result.data.Select(record => new string[]
                {
                        HttpUtility.HtmlEncode(record.Name),
                        record.Id.ToString()
                }).ToArray()
            };

            return Json(WarehouseJsonData);
        }

        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public JsonResult Create(WareHouseModel warehouseModel)
        {
            if (ModelState.IsValid)
            {
                var warehouse = _mapper.Map<WareHouse>(warehouseModel);
                _warehouseManagementService.CreateWareHouse(warehouse);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Invalid data" });
        }

        public IActionResult Update(Guid id)
        {
            WareHouse wareHouse = _warehouseManagementService.GetWareHouse(id);
            var model = _mapper.Map<WareHouseUpdateModel>(wareHouse);

            return View(model);
        }




        [HttpPost]
        public JsonResult Update(WareHouseUpdateModel warehouseUpdateModel)
        {
            if (ModelState.IsValid)
            {
                var wareHouse = _warehouseManagementService.GetWareHouse(warehouseUpdateModel.id);
                wareHouse = _mapper.Map(warehouseUpdateModel, wareHouse);
                _warehouseManagementService.UpdateWareHouse(wareHouse);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Invalid data" });
        }

      


        [HttpGet]
        public JsonResult GetWarehouseById(Guid id)
        {
            var warehouse = _warehouseManagementService.GetWareHouse(id);
            if (warehouse != null)
            {
                return Json(new { id = warehouse.Id, name = warehouse.Name });
            }
            return Json(new { success = false });
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
