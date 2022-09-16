using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop_1.Models;
using Shop_1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_1.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ShopUser> userManager;
        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ShopUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult UsersList()
        {
            var model = userManager.Users;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUserRole(string userId)
        {
            ViewBag.userId = userId;
            var model = new List<UserRolesViewModel>();
            
            var user = await userManager.FindByIdAsync(userId);
            ViewBag.email = user.Email;

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("Error");
            }

            
            foreach (var role in roleManager.Roles.ToList())
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }
                
                model.Add(userRolesViewModel);
            }
            

            return View(model);
        }
            [HttpPost]
            public async Task<IActionResult> EditUserRole(List<UserRolesViewModel> model, string userId)
            {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("Error");
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }

            result = await userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }

            return RedirectToAction("UsersList", new { Id = userId });
        }


            [HttpGet]
            public IActionResult RolesList() {
                var model = roleManager.Roles;
                return View(model);
            }
            [HttpGet]
            public IActionResult AddRole()
            {
                return View();
            }
            [HttpPost]
            public async Task<IActionResult> AddRole(IdentityRole model)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult identityResult = await roleManager.CreateAsync(model);
                    if (identityResult.Succeeded)
                    {
                        return RedirectToAction("RolesList");
                    }
                    foreach (IdentityError error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
                return View(model);
            }
            public IActionResult DeleteRole()
            {
                return RedirectToAction("RolesList");
            }

        
    }
}
