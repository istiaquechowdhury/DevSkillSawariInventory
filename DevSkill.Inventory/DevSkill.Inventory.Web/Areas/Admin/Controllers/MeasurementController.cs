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



    public class MeasurementController : Controller
    {
        private readonly IMeasurementManagementService _MeasurementManagementService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public MeasurementController(IMeasurementManagementService MeasurementManagementService, IMapper mapper, ILogger<ProductController> logger)
        {
            _MeasurementManagementService = MeasurementManagementService;
            _mapper = mapper;
            _logger = logger;   
        }

       
        public IActionResult Index()
        {
            return View();

        }
        public JsonResult GetMeasurementData([FromBody] MeasurementListModel model)
        {

            var result = _MeasurementManagementService.GetMeasurements(
                model.PageIndex,
                model.PageSize,
                model.Search,
                model.FormatSortExpression("Name")
            );


            if (result.data == null || result.data.Count == 0)
            {
                return Json(new { success = false, message = "No data available" });
            }

            var MeasurementJsonData = new
            {
                recordsTotal = result.total,
                recordsFiltered = result.totaldisplay,
                data = result.data.Select(record => new string[]
                {
                        HttpUtility.HtmlEncode(record.Name),
                        HttpUtility.HtmlEncode(record.Symbol),
                        record.Id.ToString()
                }).ToArray()
            };

            return Json(MeasurementJsonData);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult Create(MeasureMentModel measurementModel)
        {
            if (ModelState.IsValid)
            {
                var measurement = _mapper.Map<MeasurementUnit>(measurementModel);
                _MeasurementManagementService.CreateMeasurement(measurement);

                return Json(new { success = true });

            }
            return Json(new { success = false, message = "Invalid data" });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Update(Guid id)
        {
            MeasurementUnit measurement = _MeasurementManagementService.GetMeasurement(id);
            var model = _mapper.Map<MeasurementUpdateModel>(measurement);

            return View(model);
        }




        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult Update(MeasurementUpdateModel MeasurementUpdateModel)
        {
            if (ModelState.IsValid)
            {
                var Measurement = _MeasurementManagementService.GetMeasurement(MeasurementUpdateModel.id);
                Measurement = _mapper.Map(MeasurementUpdateModel, Measurement);
                _MeasurementManagementService.UpdateMeasurement(Measurement);
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
                _MeasurementManagementService.DeleteMeasurement(id);
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
        public JsonResult GetMeasurementById(Guid id)
        {
            var measurement = _MeasurementManagementService.GetMeasurement(id);
            if (measurement != null)
            {
                return Json(new { id = measurement.Id, name = measurement.Name, symbol = measurement.Symbol });
            }
            return Json(new { success = false });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public JsonResult GetAll()
        {
            var measurements =  _MeasurementManagementService.GetMeasurements();
            return Json(new { data = measurements }); // Return data in JSON format for DataTable or API usage
           
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

