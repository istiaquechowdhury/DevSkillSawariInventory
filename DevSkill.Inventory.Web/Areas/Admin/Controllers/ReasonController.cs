using AutoMapper;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Inventory.Application.Services;
using Inventory.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class ReasonController : Controller
    {

        private readonly IReasonManagementService _reasonManagementService;
        private readonly IMapper _mapper;
        public ReasonController(IMapper mapper, IReasonManagementService reasonManagementService) 
        {
            _mapper = mapper;  
            _reasonManagementService = reasonManagementService; 
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]

        [Authorize(Roles = "Admin")]
        public JsonResult Create(ReasonCreateModel reasonCreateModel)
        {
            if (ModelState.IsValid)
            {
                var reason = _mapper.Map<Reason>(reasonCreateModel);
                _reasonManagementService.CreateReason(reason);

                return Json(new { success = true });

            }
            return Json(new { success = false, message = "Invalid data" });
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> GetAll()
        {
            var Reasons = await _reasonManagementService.GetReasonsAsync();
            return Json(new { data = Reasons }); // Return data in JSON format for DataTable or API usage

        }

    }
}
