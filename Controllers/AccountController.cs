
using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace RaceDay.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.PostAsJsonAsync("api/Auth/login", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid email, password or role.");
                return View(model);
            }

            // Store API authentication cookies for future requests.
            if (response.Headers.TryGetValues("Set-Cookie", out var cookieHeaders))
            {
                var apiCookies = cookieHeaders
                    .Select(cookie => cookie.Split(';')[0]);

                HttpContext.Session.SetString(
                    "ApiCookies",
                    string.Join("; ", apiCookies));
            }

            var result = await response.Content.ReadFromJsonAsync<JsonElement>();

            HttpContext.Session.SetInt32("UserId", result.GetProperty("userId").GetInt32());
            HttpContext.Session.SetString("FirstName", result.GetProperty("firstName").GetString() ?? "");
            HttpContext.Session.SetString("LastName", result.GetProperty("lastName").GetString() ?? "");
            HttpContext.Session.SetString("Email", result.GetProperty("email").GetString() ?? "");
            HttpContext.Session.SetString("Role", result.GetProperty("role").GetString() ?? "");

            string role = model.Role;

            if (role == "Organiser")
                return RedirectToAction("Dashboard", "Organiser");

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.PostAsJsonAsync("api/Auth/register", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Registration failed. Please check your details or email address.");
                return View(model);
            }
            // Store API authentication cookies for future requests.
            if (response.Headers.TryGetValues("Set-Cookie", out var cookieHeaders))
            {
                var apiCookies = cookieHeaders
                    .Select(cookie => cookie.Split(';')[0]);

                HttpContext.Session.SetString(
                    "ApiCookies",
                    string.Join("; ", apiCookies));
            }
            TempData["Success"] = "Registration successful! Please log in.";

            return RedirectToAction("Login");
        }


        public async Task<IActionResult> Logout()
        {
            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            await client.PostAsync("api/Auth/logout", null);

            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }

    }
}
