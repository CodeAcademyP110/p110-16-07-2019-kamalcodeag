using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application1.Database;
using Application1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application1.Controllers
{
    public class AccountController : Controller
    {
        private readonly MySample _context;
        private readonly UserManager<IdentityUser> _userManager;
        public AccountController(MySample context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        [ActionName("Register")]
        public IActionResult RegisterGet()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Register")]
        public async Task<IActionResult> RegisterPost(RegisterVM registerVM)
        {
            if(ModelState.IsValid)
            {
                MyUser newUser = new MyUser()
                {
                    FirstName = registerVM.FirstName,
                    LastName = registerVM.LastName,
                    Age = registerVM.Age,
                    UserName = registerVM.Username,
                    Email = registerVM.Email
                };

                IdentityResult result = await _userManager.CreateAsync(newUser, registerVM.Password);

                if(!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return RedirectToAction("Index", "Home");
            //return View();
        }
    }
}