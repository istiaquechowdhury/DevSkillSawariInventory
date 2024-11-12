using DevSkill.Inventory.Web.Areas.Admin.Models;
using Inventory.Application.Services;
using Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class DashboardController : Controller
    {
        private readonly IProductManagementService _productManagementService;
        private readonly IStockListManagementService _stockListManagementService;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IWareHouseManagementService _wareHouseManagementService;


        public DashboardController(IProductManagementService productManagementService, IStockListManagementService stockListManagementService, UserManager<ApplicationUser> userManager, IWareHouseManagementService wareHouseManagementService) 
        {
            _productManagementService = productManagementService;
            _stockListManagementService = stockListManagementService;
            _userManager = userManager;
            _wareHouseManagementService = wareHouseManagementService;   
        }
        public async Task<IActionResult> Index()
        {
            var model = new DashBoardViewModel();
            model.ProductCount = await _productManagementService.GetProductCountAsync();
            model.StockListCount = await _stockListManagementService.GetStockListCountAsync();
            model.RegisterdUserCount = await _userManager.Users.CountAsync();
            model.WareHouseCount = await _wareHouseManagementService.GetWareHouseCountAsync();


            return View(model);
        }
    }
}
