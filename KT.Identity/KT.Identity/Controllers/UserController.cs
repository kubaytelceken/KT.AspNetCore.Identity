using KT.Identity.Entities;
using KT.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KT.Identity.Controllers
{
    [Authorize (Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async  Task<IActionResult> Index()
        {
            var users = await _userManager.GetUsersInRoleAsync("Member");
            return View(users);
        }

        public IActionResult Create()
        {
            return View(new UserAdminCreateModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserAdminCreateModel userCreateModel)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser()
                {
                    Email = userCreateModel.Email,
                    UserName = userCreateModel.Username,
                    Gender = userCreateModel.Gender
                };

                var memberRole = await _roleManager.FindByNameAsync("Member");
                if (memberRole == null)
                {
                    await _roleManager.CreateAsync(new AppRole()
                    {
                        Name = "Member",
                        CreatedTime = DateTime.Now
                    });
                }

                var identityResult = await _userManager.CreateAsync(appUser, userCreateModel.Username);
                if (identityResult.Succeeded == true)
                {
                    await _userManager.AddToRoleAsync(appUser, "Member");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    foreach (var item in identityResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }

            }
            return View(userCreateModel);

        }
    }
}
