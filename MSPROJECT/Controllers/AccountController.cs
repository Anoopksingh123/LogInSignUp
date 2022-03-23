using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSPROJECT.ViewModel;
using MSPROJECT.Repository.Interface;
using MSPROJECT.Utils.Enums;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MSPROJECT.Controllers
{
    public class AccountController : Controller
    {
        private IUsers UserService;
        public AccountController(IUsers users)    //Services injected
        {
            UserService = users;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(SignInModel model)
        {
            if (ModelState.IsValid)
            {
                var result = UserService.SignIn(model);
                if(result==SignInEnum.success)
                {

                    //A claim is a statement about a subject by an issuer and    
                    //represent attributes of the subject that are useful in the context of authentication and authorization operations.    
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, model.Email),
                };
                    //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme    
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity    
                    var principal = new ClaimsPrincipal(identity);
                    //SignInAsync is a Extension method for Sign in a principal for the specified scheme.    
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                    {
                        IsPersistent = model.RememberMe
                    });
                    return RedirectToAction("Index", "Home");
                }
                else if (result == SignInEnum.WrongCredentials)
                {
                    ModelState.AddModelError(string.Empty, "Ivalid login credential!");
                }
                else if (result == SignInEnum.NotVerified)
                {
                    ModelState.AddModelError(string.Empty, "IUsers not verified please verify first!");
                }
                else if(result==SignInEnum.InActive)
                {
                    ModelState.AddModelError(string.Empty, "Your account is Inactive!");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Register(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                var result = UserService.SignUp(model);
                if (result == SignUpEnum.success)
                {
                    return RedirectToAction("VerifyAccount");
                }
                else if (result == SignUpEnum.Emailexist)
                {
                    ModelState.AddModelError(string.Empty, "This Email already exist ,Please try another");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Something Went wrong !..");
                }
            }
            return View();
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
        public IActionResult VerifyAccounts()
        {
            return View();
        }
        [HttpPost]
        public IActionResult VerifyAccounts(string Otp)
        {
            if (Otp != null)
            {
                if (UserService.VerifyAccounts(Otp))
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid OTP !");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please Enter OTP !");
            }
            return View();
        }
    }
}
