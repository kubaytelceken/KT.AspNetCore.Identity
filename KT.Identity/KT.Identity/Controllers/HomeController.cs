using KT.Identity.Entities;
using KT.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KT.Identity.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View(new UserCreateModel());
        }
         
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateModel userCreateModel)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser()
                {
                    Email = userCreateModel.Email,
                    UserName = userCreateModel.UserName,
                    Gender = userCreateModel.Gender
                };


                var identityResult = await _userManager.CreateAsync(appUser, userCreateModel.Password);
                if(identityResult.Succeeded == true)
                {
                    return RedirectToAction("Index", "Home");
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



        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInModel userSignInModel)
        {
            if (ModelState.IsValid)
            {
              var result =   await _signInManager.PasswordSignInAsync(userSignInModel.Username, userSignInModel.Password, false, true);
                if (result.Succeeded)
                {

                }
                else if (result.IsLockedOut)
                {
                    // hesap kilitli
                }
                else if (result.IsNotAllowed)
                {
                    // Email & phone number doğrulanmamış
                }
            }
            return View(userSignInModel);
        }
    }
}
