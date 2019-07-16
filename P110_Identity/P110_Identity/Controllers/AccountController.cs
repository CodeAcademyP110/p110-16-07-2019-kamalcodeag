using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using P110_Identity.DAL;
using P110_Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using P110_Identity.Models;

namespace P110_Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Register()
        {
            ViewBag.Countries = _context.Countries;
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Countries = _context.Countries;
                return View(registerViewModel);
            }

            ApplicationUser newUser = new ApplicationUser
            {
                Firstname = registerViewModel.Firstname,
                Lastname = registerViewModel.Lastname,
                Email = registerViewModel.Email,
                UserName = registerViewModel.Username,
                CityId = registerViewModel.CityId
            };

            IdentityResult identityResult = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if(!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ViewBag.Countries = _context.Countries;
                return View(registerViewModel);
            }


            return RedirectToAction("Index", "Home");
        }
    }
}