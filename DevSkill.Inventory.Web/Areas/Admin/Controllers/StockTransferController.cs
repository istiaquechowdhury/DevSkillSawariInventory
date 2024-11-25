using AutoMapper;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Inventory.Application.Services;
using Inventory.Domain.Entities;
using Inventory.Infrastructure.Identity;
using Inventory.Infrastructure.InventoryDb;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class StockTransferController : Controller
    {
        private readonly IStockTransferManagementService _stocktransferManagementService;
        private readonly IWareHouseManagementService _WareHouseManagementService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;



        public StockTransferController(IStockTransferManagementService stocktransferManagementService, IWareHouseManagementService WareHouseManagementService, IMapper mapper, ILogger<ProductController> logger, UserManager<ApplicationUser> userManager)
        {
           
            _mapper = mapper;
            _logger = logger;
            _stocktransferManagementService = stocktransferManagementService;
            _WareHouseManagementService = WareHouseManagementService;
            _userManager = userManager;

        }
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult GetBlogPostJsonData([FromBody] StockTransferListModel model)
        {

            var result = _stocktransferManagementService.GetStockTransfers(model.PageIndex, model.PageSize, model.Search, model.FormatSortExpression("Date", "WareHouse", "DestinationWarehouse", "TransferBy", "Note"));
            var blogPostJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totaldisplay,
                data = (from record in result.data
                        select new string[]
                        {
                         HttpUtility.HtmlEncode(record.Date),
                         HttpUtility.HtmlEncode(record.WareHouse?.Name),
                         HttpUtility.HtmlEncode(record.DestinationWarehouse?.Name),
                         HttpUtility.HtmlEncode(record.Product?.ItemName),
                         HttpUtility.HtmlEncode(record.TransferredQuantity),
                         HttpUtility.HtmlEncode(record.TransferBy),
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

            var model = new StockTransferModel();
            model.SetWareHouseValues(await _WareHouseManagementService.GetWareHousesAsync());
            var user = await _userManager.GetUserAsync(User); // Retrieve the user asynchronously
            model.TransferBy = user?.UserName ?? string.Empty;
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create(StockTransferModel stockTransferModel)
        {
            if (ModelState.IsValid)
            {

                var StockTransfer = _mapper.Map<StockTransfer>(stockTransferModel);
                StockTransfer.WareHouse = _WareHouseManagementService.GetWareHouse(stockTransferModel.WareHouseId);
                var user = await _userManager.GetUserAsync(User); // Retrieve the user asynchronously
                StockTransfer.TransferBy = user?.UserName ?? string.Empty;

                try
                {

                    _stocktransferManagementService.CreateStockTransfer(StockTransfer);
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
            stockTransferModel.SetWareHouseValues( await _WareHouseManagementService.GetWareHousesAsync());
            return View(stockTransferModel);
        }




        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public IActionResult Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _stocktransferManagementService.DeleteStockTransfer(id);

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



    }
}
