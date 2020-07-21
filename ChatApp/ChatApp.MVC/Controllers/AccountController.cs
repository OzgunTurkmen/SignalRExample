using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp.MVC.Model;
using Flurl.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.MVC.Controllers
{
    public class AccountController : Controller
    {

        public IActionResult Login()
        {
            var isLogged = HttpContext.Session.GetString("username");

            if (isLogged != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            var url = "https://localhost:44349/" + "Account/Login";

            var response = await url
                .PostJsonAsync(user)
                .ReceiveJson<User>();

            if (response.Password != "")
            {
                HttpContext.Session.SetString("username", user.Username);

                TempData["Message"] = "Hoşgeldin " + response.Username;
                return RedirectToAction("Index", "Home");
            }

            TempData["Message"] = "Böyle bir kullanıcı yok yada şifreniz hatalı";
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            var url = "https://localhost:44349/" + "Account/Register";

            var response = await url
                .PostJsonAsync(user)
                .ReceiveJson<User>();

            if (response.IsSuccess)
            {
                TempData["Message"] = "Hoşgeldin " + response.Username;
                return RedirectToAction("Login");
            }

            TempData["Message"] = "Bu kullanıcı adı alınmış";
            return View();
        }
    }
}
