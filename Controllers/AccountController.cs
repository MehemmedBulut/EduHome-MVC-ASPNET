using Eduprob.Helpers;
using Eduprob.Models;
using Eduprob.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eduprob.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager= signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            AppUser appUser = await _userManager.FindByNameAsync(loginVM.Username);
            
            if (appUser == null)
            {
                appUser = await _userManager.FindByEmailAsync(loginVM.Username);
                if (appUser == null)
                {
                    ModelState.AddModelError("Username", "Account not found with username or email");
                    return View();
                }
            }
            if (appUser.IsDeactive)
            {
                ModelState.AddModelError("Username", "Deactive");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser,loginVM.Password,loginVM.IsRemember,true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Max 5 for 1 min");
                return View();
            }
            if (!signInResult.Succeeded)
            {
                
                ModelState.AddModelError("Password", "Password wrong");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            AppUser appUser = new AppUser
            {
                Name = registerVM.Name,
                Email = registerVM.Email,
                Surname = registerVM.Surname,
                UserName = registerVM.Username,
            };
            IdentityResult ıdentityResult = await _userManager.CreateAsync(appUser,registerVM.Password);
            if (!ıdentityResult.Succeeded)
            {
                foreach (IdentityError error in ıdentityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }
            await _userManager.AddToRoleAsync(appUser,Helper.Member);
            await _signInManager.SignInAsync(appUser, registerVM.IsRemember);
            return RedirectToAction("Index", "Home");
        }
        //public async Task CreateRoles()
        //{
        //    if(!await _roleManager.RoleExistsAsync(Helper.Admin))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Admin });
        //    }
        //    if (!await _roleManager.RoleExistsAsync(Helper.Member))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Helper.Member });
        //    }
        //}
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
