using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using book.webui.EmailServices;
using book.webui.Identify;
using book.webui.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace book.webui.Controllers
{
    // butun post tipli forumlar ucundur 
    [AutoValidateAntiforgeryToken]
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IEmailSender _emailSender;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager , IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender=emailSender;
        }
        public IActionResult Login(string? ReturnUrl = null)
        {
            return View(new LoginModel()
            {
                ReturnUrl = ReturnUrl
            });
        }
        // yalniz bir 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // var user = await _userManager.FindByNameAsync(model.UserName);
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Bu istifadeci adı ile daha önce hesap movcud deyil");
                return View(model);
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("", "Lutfen email hesabiniza gelen link ile uyelik onaylayiniz");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user, model.Passwords, true, false);

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "~/");
            }

            ModelState.AddModelError("", "Girilen kullanıcı adı veya parola yanlış");
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email

            };
            var result = await _userManager.CreateAsync(user, model.Passwords);
            if (result.Succeeded)
            {
                //generation token 
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new
                {
                    userId = user.Id,
                    token = code
                });
                //email
                await  _emailSender.SendEmailAsync( model.Email , "Hesabiniz onaylayin", $"Lütfen email hesabınızı onaylamak için linke <a href='https://localhost:7081{url}'>daxil olun </a>");
                return RedirectToAction("Login", "Account");
            }
            ModelState.AddModelError("", "Her hansi xetta bas verdi");
            return View(model);
        }

        public async System.Threading.Tasks.Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }
      
        public async System.Threading.Tasks.Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                CreateMessage("Gecersiz token", "danger");

                return View();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    CreateMessage("Hesab onaylandi", "success");
                    return View();
                }
                
            }
            TempData["message"] = "Hesab onaylanmadi.";
            return View();

        }
        private void CreateMessage(string message, string alerttype)
        {
            var msg = new AlertMessage()
            {
                Message = message,
                AlertType = alerttype
            };
            TempData["message"] = JsonConvert.SerializeObject(msg);
            //  TempData["message"]= $"{entity.Name} adli category elave olundu.";
        }
    }
}