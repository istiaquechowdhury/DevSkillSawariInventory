using AutoMapper;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Inventory.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class StockController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        private readonly IStockListManagementService _stockListManagementService;
        private readonly IWareHouseManagementService _wareHouseManagementService;
        public StockController(ICategoryManagementService categoryManagementServices, IMapper mapper, ILogger<ProductController> logger,
        IStockListManagementService stockListManagementService,IWareHouseManagementService wareHouseManagementService)
        {
             _stockListManagementService = stockListManagementService;
            _mapper = mapper;
            _logger = logger;
            _wareHouseManagementService = wareHouseManagementService;   
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();  
        }

        public async Task<IActionResult> IndexSP()
        {
            var model = new StockListModel();

            model.SetWareHouseValues(await _wareHouseManagementService.GetWareHousesAsync());




            return View(model);
        }





        [Authorize(Roles = "Admin")]

        public JsonResult GetStocklistjsondata([FromBody] StockListModel model)
        {

            var result = _stockListManagementService.GetStockLists(model.PageIndex, model.PageSize, model.Search, model.FormatSortExpression("Product", "Warehouse", "InHand", "WeightedAvgCost"));
            var Stocklistjsondata = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totaldisplay,
                data = (from record in result.data
                        select new string[]
                        {


                         
                         HttpUtility.HtmlEncode(record.Product.ItemName),
                         HttpUtility.HtmlEncode(record.Product?.Barcode),
                   
                         HttpUtility.HtmlEncode(record.Warehouse?.Name),
                         HttpUtility.HtmlEncode(record.InHand),
                         HttpUtility.HtmlEncode(record.WeightedAvgCost),
                       





                         record.Id.ToString(),
                         HttpUtility.HtmlEncode(record.Product.InStock),
                         HttpUtility.HtmlEncode(record.Product.TotalWarehouseStock),



                        }
                    ).ToArray()
            };

            return Json(Stocklistjsondata);

        }

        public async Task<JsonResult> GetStocklistjsondataSP([FromBody] StockListModel model)
        {
            try
            {
                var result = await _stockListManagementService.GetStockListsSP(
                    model.PageIndex,
                    model.PageSize,
                    model.SearchItem,
                    model.FormatSortExpression("ItemName", "Warehouse", "InHand", "WeightedAvgCost")
                );

                var Stocklistjsondata = new
                {
                    recordsTotal = result.total,
                    recordsFiltered = result.totaldisplay,
                    data = (from record in result.data
                            select new string[]
                            {
                                HttpUtility.HtmlEncode(record.ItemName),
                                HttpUtility.HtmlEncode(record.Barcode),
                                HttpUtility.HtmlEncode(record.WareHouseName),
                                HttpUtility.HtmlEncode(record.InHand.ToString()),
                                HttpUtility.HtmlEncode(record.WeightedAvgCost.ToString("F2")),
                                HttpUtility.HtmlEncode(record.InStock),

                                record.Id.ToString(),
                            }
                        ).ToArray()
                };

                return Json(Stocklistjsondata);
            }
            catch (Exception ex)
            {
                // Log the error (if you have a logging framework) and return a JSON error message.
                return Json(new { error = "An error occurred: " + ex.Message });
            }
        }







    }
}
