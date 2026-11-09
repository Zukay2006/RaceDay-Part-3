
using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using System.Net.Http.Json;

namespace RaceDay.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            string? role = HttpContext.Session.GetString("Role");

            if (role == null)
                return RedirectToAction("Login", "Account");

            if (role == "Organiser")
                return RedirectToAction("Dashboard", "Organiser");

            try
            {
                var client = _httpClientFactory.CreateClient("RaceDayAPI");

                var events = await client.GetFromJsonAsync<List<EventViewModel>>(
                    "api/Events");

                var upcomingEvents = events?
                    .Where(e => e.EventDate >= DateTime.Today)
                    .OrderBy(e => e.EventDate)
                    .ToList() ?? new List<EventViewModel>();

                return View(upcomingEvents);
            }
            catch
            {
                ViewBag.Error = "Unable to load events. Please try again later.";
                return View(new List<EventViewModel>());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
