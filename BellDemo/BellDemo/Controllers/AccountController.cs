using BellDemo.Data;
using BellDemo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BellDemo.Models;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BellDemo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<User> userManager,
                                      SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ViewBag.success = false;
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var checkUser = await _userManager.FindByEmailAsync(model.Email);
                if (checkUser == null)
                {
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);

                        return Json(new
                        {
                            success = true,
                            redirectToUrl = this.Url.Action("index", "Home", null)
                        });
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    ModelState.AddModelError("INVALID", "Invalid Login Attempt");
                }

                ModelState.AddModelError("DUPLICATEEMAIL", "User account with entered email exists!");
            }

            return Json(new {
                success = false,
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                    .Select(m => m.ErrorMessage).ToArray()
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(bool reset = false)
        {
            @ViewBag.reset = reset;
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "WorkFlows", new { newlogin = true });
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
        
        public IActionResult ForgetPassword()
        {
            return View(new ForgetPasswordModel());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var checkUser = await _userManager.FindByEmailAsync(model.Email);
                //todo better error handeling with try catch and also if account not exists 
                if (checkUser != null)
                {
                    var apiKey = _configuration["SendGridKey"];
                    var client = new SendGridClient(apiKey);
                    var from = new EmailAddress("harshgorjiwala884@gmail.com", "Harsh");
                    var subject = "Reset your password";
                    var to = new EmailAddress(model.Email, checkUser.FirstName);
                    var url = new Uri($"{Request.Scheme}://{Request.Host}/Account/Reset?guid={checkUser.Id}");
                    
                    var htmlContent = $"Please click <b><a href='{url.AbsoluteUri}'>here</a></b> to reset your password.";
                    var msg = MailHelper.CreateSingleEmail(from, to, subject,"", htmlContent);
                    var response = await client.SendEmailAsync(msg);
                }

                return RedirectToAction("Login");
            }
            return View(model);
        }

        public async Task<IActionResult> Reset(string guid)
        {
            ResetModel model = new ResetModel {Guid = guid, Password = "", ConfirmPassword = ""};
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Reset(ResetModel model)
        {
            if (ModelState.IsValid)
            {
                var checkUser = await _userManager.FindByIdAsync(model.Guid);
               
                if (checkUser == null)
                {
                    ModelState.AddModelError(string.Empty, "User not exists!");
                    return View(model);
                }

                if (model.Password != model.ConfirmPassword)
                {
                    ModelState.AddModelError(string.Empty, "Password don't match!");
                    return View(model);
                }

                var success = await _userManager.ChangePasswordAsync(checkUser, model.CurrentPassword, model.Password);

                if (success.Succeeded)
                {
                    return RedirectToAction("Login", "Account", new { reset = true});
                }

                foreach (var identityError in success.Errors)
                {
                    ModelState.AddModelError(identityError.Code, identityError.Description);
                }

                return View(model);
            }
            return View(model);
        }
    }
}
