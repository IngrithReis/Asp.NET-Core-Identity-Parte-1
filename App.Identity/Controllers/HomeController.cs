using App.Identity.Domain;
using App.Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.Identity.Controllers
{

    public class HomeController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly IUserClaimsPrincipalFactory<Users> _userClaimsPrincipalFactory;
        private readonly SignInManager<Users> _signInManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(UserManager<Users> userManager, IUserClaimsPrincipalFactory<Users> userClaimsPrincipalFactory,
            SignInManager<Users> signInManager, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _signInManager = signInManager;
            _logger = logger;
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
                    if( !await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError("", "Este endereço de e-mail não é válido!");
                        return View();
                    }

                  var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
                //var identity = new ClaimsIdentity("Identity.Aplication");
                //identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                //identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                await HttpContext.SignInAsync("Identity.Application", principal);

                //var resultSignIn = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false,false);

                //if (resultSignIn.Succeeded)
                //{ 
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

        [HttpGet]
        public async Task<IActionResult> ConfirmEmailAddress(string token, string email)
        {
            var user =await _userManager.FindByEmailAsync(email);

            if(user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    return View("Success");
                }
            }
            return View("ForgotPasswordError");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Name);
                if (user == null)
                {
                    var organization = new Organization
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = model.OrganizationName,


                    };
                    user = new Users
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.Name,
                        Organization = organization,
                        Email = model.Email
                       
                    };

                    

                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        var confirmEmail = Url.Action("ConfirmEmailAddress", "Home", 
                            new {token = token, email = user.Email}, Request.Scheme);

                        System.IO.File.WriteAllText("confirmEmail.txt", confirmEmail);
                    }
                    

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

        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if(user != null )
                {
                    var token = _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetUrl = Url.Action("ResetPassword", "Home",
                        new  { token = token, email = model.Email }, Request.Scheme);

                    _logger.Log(LogLevel.Warning, resetUrl);
                    System.IO.File.WriteAllText("resetLink.txt", resetUrl);

                    return View("ForgotPasswordConfirmation");
                }
                else
                {
                    return View("ForgotPasswordError");
                }

                
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            return View(new ResetPasswordModel { Token = token, Email = email});
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                    if (!result.Succeeded)
                    {
                        foreach(var erros in result.Errors)
                        {
                            ModelState.AddModelError("", erros.Description);
                        }

                        return View();
                    }

                    return View("Success");
                }
                ModelState.AddModelError("", "Invalid Request");
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Success()
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
