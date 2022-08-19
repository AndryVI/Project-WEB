using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using WebH.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System;
using WebH.Services;

namespace WebH.Controllers
{
    public class AccountController : Controller
    {
        NotesService _notesservice;
        public AccountController(ILogger<HomeController> logger, [FromServices] NotesService notesservice)
        {
            _notesservice = notesservice;
            logger.LogInformation("This AccountController");
        }

        public IActionResult Denied() => View();

        #region Notes

        [Authorize(Roles = "User,Admin")]
        public IActionResult Notes()
        {
            var notes = _notesservice.GetNotes(HttpContext.User.Identity.Name);
            return View(notes);
        }


        [HttpPost]
        public IActionResult NotesAdd(string topic, string context)
        {
            Notes notes = new Notes() { LoginName = HttpContext.User.Identity.Name, Topic = topic, Context = context, DataTime = DateTime.Now };
            _notesservice.AddNotes(notes);

            return RedirectToAction("Notes", "Account");
        }

        #endregion


        #region Sign-Out

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion


        #region Sign-In

        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult SignInAccess()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(LoginBindingModel model)
        {
            var serviceModel = model.ToServiceModel();

            if (ModelState.IsValid)
            {
                var userOrNull = Database.Users.FirstOrDefault(x => x.Login == model.Login);
                if (userOrNull is User user)
                {
                    var isCorrectPassword = PasswordHasher.IsCorrectPassword(user, model.Password);
                    if (isCorrectPassword)
                    {
                        await SignInAsync(user);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Wrong login or password");
                return View(model);
            }
            else
            { 
                return View(model);
            }
        }

        private async Task SignInAsync(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Login  == "admin"? "Admin" : "User"),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);
        }

        #endregion


        #region Register

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var userAlreadyExists = Database.Users.Any(x => x.Login == model.Login);
                if (userAlreadyExists)
                {
                    ModelState.AddModelError(nameof(model.Login), "Login is already in use");
                    return View(model);
                }

                Database.AddUser(Guid.NewGuid(), model.Login, model.Password);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(model);
            }
        }

        #endregion

    }
}
