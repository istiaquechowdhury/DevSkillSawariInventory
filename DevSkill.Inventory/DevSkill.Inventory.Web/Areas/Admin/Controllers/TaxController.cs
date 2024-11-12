using AutoMapper;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Inventory.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class TaxController : Controller
    {

        private readonly ITaxManagementService _TaxManagementService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public TaxController(ITaxManagementService TaxManagementService,IMapper mapper, ILogger<ProductController> logger)
        {
            _TaxManagementService = TaxManagementService;   
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public JsonResult GetBlogPostJsonData([FromBody] TaxListModel model)
        {
            if (model.PageSize <= 0)
            {
                model.PageSize = int.MaxValue; // or a large number to ensure all records are fetched
                model.PageIndex = 1;
            }

            var result = _TaxManagementService.GetTaxes(model.PageIndex, model.PageSize, model.Search, model.FormatSortExpression("Name","Percentage"));
            var blogPostJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totaldisplay,
                data = (from record in result.data
                        select new string[]
                        {
                         HttpUtility.HtmlEncode(record.Name),
                         HttpUtility.HtmlEncode(record.Percentage),



                         record.Id.ToString()
                        }
                    ).ToArray()
            };

            return Json(blogPostJsonData);

        }

    }
}
