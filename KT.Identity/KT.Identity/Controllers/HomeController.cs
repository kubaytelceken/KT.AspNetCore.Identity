using KT.Identity.Entities;
using KT.Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KT.Identity.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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

                var memberRole = await _roleManager.FindByNameAsync("Member");
                if(memberRole == null)
                {
                    await _roleManager.CreateAsync(new AppRole()
                    {
                        Name = "Member",
                        CreatedTime = DateTime.Now
                    });
                }
             
                var identityResult = await _userManager.CreateAsync(appUser, userCreateModel.Password);
                if(identityResult.Succeeded == true)
                {
                    await _userManager.AddToRoleAsync(appUser, "Member");
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



        public IActionResult SignIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new UserSignInModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInModel userSignInModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(userSignInModel.Username);
                var result =   await _signInManager.PasswordSignInAsync(userSignInModel.Username, userSignInModel.Password, userSignInModel.RememberMe, true);
                if (result.Succeeded)
                {
                   
                    var role = await _userManager.GetRolesAsync(user);
                    if(role.Contains("Admin"))
                    {
                        return RedirectToAction("AdminPanel");
                    }
                    else
                    {
                        return RedirectToAction("MemberPanel");
                    }
                }
                else if (result.IsLockedOut)
                {
                    // 
                    var lockoutDate = await _userManager.GetLockoutEndDateAsync(user);
                    
                    ModelState.AddModelError("", $"Hesabınız {(lockoutDate.Value.UtcDateTime - DateTime.UtcNow).Minutes} dakika askıya alınmıştır");
                }
                else
                {
                    var message = string.Empty;
                    var userControl = await _userManager.FindByNameAsync(userSignInModel.Username);
                    if (userControl != null)
                    {
                        int failedCount = await _userManager.GetAccessFailedCountAsync(userControl);
                        int lockoutCount = _userManager.Options.Lockout.MaxFailedAccessAttempts;
                        var failedResult = lockoutCount - failedCount;
                        message = $"{failedResult} kez daha girerseniz hesabınız geçici olarak kilitlenecektir.";
                    }
                    else
                    {
                        message = "Kullanıcı Adı veya Şifre Hatalı";
                    }
                    ModelState.AddModelError("", message);
                }
               
                //else if (result.IsLockedOut)
                //{
                //    // hesap kilitli
                //}
                //else if (result.IsNotAllowed)
                //{
                //    // Email & phone number doğrulanmamış
                //}
            }
            return View(userSignInModel);
        }

        [Authorize]
        public IActionResult GetUserInfo()
        {
            var userName = User.Identity.Name;
            var role = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;


            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {
            return View();
        }
        [Authorize(Roles = "Member")]
        public IActionResult MemberPanel()
        {
            return View();
        }


        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
