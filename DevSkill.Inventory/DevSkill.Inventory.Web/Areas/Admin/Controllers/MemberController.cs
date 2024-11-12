using Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class MemberController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public MemberController(RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult AddClaim()
        //{
        //    var model = new ClaimAddModel();

        //    var users = from c in _userManager.Users.ToList() select c;
        //    model.UserId = users.First().Id;
        //    model.Users = new SelectList(users, "Id", "UserName");

        //    return View(model);
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<IActionResult> AddClaim(ClaimAddModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByIdAsync(model.UserId.ToString());
        //        await _userManager.AddClaimAsync(user, 
        //            new System.Security.Claims.Claim(model.ClaimName, model.ClaimValue));
        //    }
        //    var users = from c in _userManager.Users.ToList() select c;
        //    model.UserId = users.First().Id;
        //    model.Users = new SelectList(users, "Id", "UserName");

        //    return View(model);
        //}
        //[Authorize(Roles = "Admin")]
        public IActionResult CreateRole()
        {
            var model = new RoleCreateModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken/*Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> CreateRole(RoleCreateModel model)
        {
            if (ModelState.IsValid)
            {
                await _roleManager.CreateAsync(new ApplicationRole
                {
                    Id = Guid.NewGuid(),
                    NormalizedName = model.Name.ToUpper(),
                    Name = model.Name,
                    ConcurrencyStamp = DateTime.UtcNow.Ticks.ToString()
                });
                TempData["success"] = "Role Created successfully";
            }
            else
            {
                TempData["error"] = "Role Creation Failed";
            }


            return View(model);
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult ChangeRole()
        {
            var model = new RoleChangeModel();
            LoadValues(model);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken/*,Authorize(Roles = "Admin")*/]
        public async Task<IActionResult> ChangeRole(RoleChangeModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);
                var newRole = await _roleManager.FindByIdAsync(model.RoleId.ToString());
                await _userManager.AddToRoleAsync(user, newRole.Name);
                TempData["success"] = "Role Changed successfully";
            }
            else
            {
                TempData["error"] = "Role Change Failed";
            }

            LoadValues(model);
            return View(model);
        }

        private void LoadValues(RoleChangeModel model)
        {
            var users = from c in _userManager.Users.ToList() select c;
            var roles = from c in _roleManager.Roles.ToList() select c;

            model.UserId = users.First().Id;
            model.RoleId = roles.First().Id;

            model.Users = new SelectList(users, "Id", "UserName");
            model.Roles = new SelectList(roles, "Id", "Name");
        }


        //public async Task<IActionResult> Show()
        //{
        //    // Fetch roles from the AspNetRoles table
        //    var roles = _roleManager.Roles
        //        .Select(r => new RoleViewModel
        //        {
        //            Id = (r.Id),
        //            Name = r.Name,
        //            NormalizedName = r.NormalizedName,
        //            ConcurrencyStamp = r.ConcurrencyStamp
        //        })
        //        .ToList();

        //    // Fetch all users and their roles
        //    var userRoles = new List<UserRoleViewModel>();
        //    var users = _userManager.Users.ToList();

        //    foreach (var user in users)
        //    {
        //        var rolesForUser = await _userManager.GetRolesAsync(user);
        //        foreach (var roleName in rolesForUser)
        //        {
        //            var role = roles.FirstOrDefault(r => r.Name == roleName);
        //            if (role != null)
        //            {
        //                userRoles.Add(new UserRoleViewModel
        //                {
        //                    UserId = (user.Id),
        //                    RoleId = role.Id
        //                });
        //            }
        //        }
        //    }

        //    var viewModel = new Tuple<List<RoleViewModel>, List<UserRoleViewModel>>(roles, userRoles);

        //    return View(viewModel);
        //}
        public async Task<IActionResult> Show()
        {
            // Fetch all roles
            var roles = await _roleManager.Roles
                .Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name
                }).ToListAsync();

            // Fetch users and their roles using explicit joins
            var users = await _userManager.Users.ToListAsync();

            var userRoles = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roleNames = await _userManager.GetRolesAsync(user);

                foreach (var roleName in roleNames)
                {
                    userRoles.Add(new UserRoleViewModel
                    {
                        UserName = user.UserName,
                        RoleName = roleName // This will now correctly map the role name
                    });
                }
            }

            // Exclude users with no roles
            userRoles = userRoles.Where(ur => ur.RoleName != null).ToList();

            var model = new Tuple<List<RoleViewModel>, List<UserRoleViewModel>>(roles, userRoles);
            return View(model);
        }


    }
}
