using AutoMapper;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Inventory.Application.Services;
using Inventory.Domain.Entities;
using Inventory.Domain.UnitOfWorkInterfaces;
using Inventory.Infrastructure.Identity;
using Inventory.Infrastructure.InventoryDb;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class StockAdjustmentController : Controller
    {
        private readonly IStockAdjustmentManagementService _StockAdjustmentManagementService;
        private readonly IMapper _mapper;
        private readonly ILogger<StockAdjustmentController> _logger;
        private readonly IWareHouseManagementService _WareHouseManagementService;
        private readonly IReasonManagementService _ReasonManagementService;
        private readonly InventoryDbContext _inventoryDbContext;
        private readonly UserManager<ApplicationUser> _userManager;





        public StockAdjustmentController(IStockAdjustmentManagementService StockAdjustmentManagementService, IMapper mapper, ILogger<StockAdjustmentController> logger,
            IWareHouseManagementService WareHouseManagementService, IReasonManagementService ReasonManagementService, InventoryDbContext inventoryDbContext, UserManager<ApplicationUser> userManager)
        {
            _StockAdjustmentManagementService = StockAdjustmentManagementService;
            _mapper = mapper;   
            _logger = logger; 
            _WareHouseManagementService = WareHouseManagementService;
            _ReasonManagementService = ReasonManagementService; 
            _inventoryDbContext = inventoryDbContext;
            _userManager = userManager; 
            

        }


        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IActionResult GetInStock(Guid productId)
        {
            var product = _inventoryDbContext.Products
                .FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                int totalStock = product.Dhanmondi + product.Mirpur + product.Uttara + product.WIP;

                return Json(totalStock);
            }

            return Json(0); // Return 0 if product not found
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IActionResult Search(string search)
        {
            var products = _inventoryDbContext.Products
                .Where(p => p.ItemName.Contains(search))
                .ToList();
            return Json(products);
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetBlogPostJsonData([FromBody] StockAdjustmentListModel model)
        {

            var result = _StockAdjustmentManagementService.GetStockAdjustments(model.PageIndex, model.PageSize, model.Search, model.FormatSortExpression("dateOnly", "Product", "WareHouse", "Quantity", "Reason", "AdjustedBy", "Note"));
            var blogPostJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totaldisplay,
                data = (from record in result.data
                        select new string[]
                        {
                         HttpUtility.HtmlEncode(record.dateOnly),
                         HttpUtility.HtmlEncode(record.Product?.ItemName),
                         HttpUtility.HtmlEncode(record.WareHouse?.Name),
                         HttpUtility.HtmlEncode(record.Quantity),
                         HttpUtility.HtmlEncode(record.Reason?.Name),
                         HttpUtility.HtmlEncode(record.AdjustedBy),
                         HttpUtility.HtmlEncode(record.Note),
                      

                         record.Id.ToString()
                        }
                    ).ToArray()
            };

            return Json(blogPostJsonData);

        }

        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create()
        {

            var model = new StockAdjustmentModel();
            model.WareHouseValues(await _WareHouseManagementService.GetWareHousesAsync());
            model.ReasonValues(await _ReasonManagementService.GetReasonsAsync());
            var user = await _userManager.GetUserAsync(User); // Retrieve the user asynchronously
            model.AdjustedBy = user?.UserName ?? string.Empty;

            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]


        public async Task<IActionResult> Create(StockAdjustmentModel Model)
        {
            if (ModelState.IsValid)
            {

                var StockAdjust = _mapper.Map<StockAdjustment>(Model);
                StockAdjust.WareHouse = _WareHouseManagementService.GetWareHouse(Model.WareHouseId);
                StockAdjust.Reason = _ReasonManagementService.GetReason(Model.ReasonId);
                var user = await _userManager.GetUserAsync(User); // Retrieve the user asynchronously
                StockAdjust.AdjustedBy = user?.UserName ?? string.Empty;
                try
                {

                    _StockAdjustmentManagementService.CreateStockAdjustment(StockAdjust);
                    TempData["success"] = "Data inserted successfully";

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Data insert falied,Data should be unique";
                    _logger.LogError(ex, "Blog post creation failed");

                }




                return RedirectToAction("Index");




            }
            TempData["error"] = "Data insert falied";
            Model.WareHouseValues(await _WareHouseManagementService.GetWareHousesAsync());
            Model.ReasonValues( await _ReasonManagementService.GetReasonsAsync());
            return View(Model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public IActionResult Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _StockAdjustmentManagementService.DeleteStockAdjustment(id);

                    TempData["success"] = "Data Deleted Successfully";

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["error"] = "Data Delete failed";

                    _logger.LogError(ex, "Blog post creation failed");

                    return RedirectToAction("Index");



                }

            }

            return View();
        }


        public async Task<IActionResult> AdjustedBy()
        {
            var user = await _userManager.GetUserAsync(User); // Get the logged-in user
            var model = new StockAdjustmentModel
            {
                AdjustedBy = user?.UserName ?? string.Empty // Set AdjustedBy to the username of the logged-in user or empty if null
            };

            return View(model); // Return the view with the model
        }

    }
}
