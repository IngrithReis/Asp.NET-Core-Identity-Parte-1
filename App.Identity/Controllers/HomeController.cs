using App.Identity.Domain;
using App.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.Identity.Controllers
{

    public class HomeController : Controller
    {
        private readonly UserManager<Users> _userManager;
        public HomeController(UserManager<Users> userManager)
        {
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var identity = new ClaimsIdentity("cookies");
                    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                    await HttpContext.SignInAsync("cookies", new ClaimsPrincipal(identity));

                    return RedirectToAction("About");
                }
                ModelState.AddModelError("", "Usuário ou senha inválida!");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Name);
                if (user == null)
                {
                    user = new Users
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.Name,
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded == false)
                    {


                        foreach (var erro in result.Errors)
                        {


                            ModelState.AddModelError(nameof(model.Password), erro.Description);

                        }
                        if (!ModelState.IsValid)
                        {
                            return View();
                        }


                    }


                }
                return View("Success");
            }
            return View();

        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
