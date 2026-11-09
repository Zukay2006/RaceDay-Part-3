
using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using System.Net.Http.Json;

namespace RaceDay.MVC.Controllers
{
    public class EventsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EventsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var raceEvent = await client.GetFromJsonAsync<EventViewModel>(
                $"api/Events/{id}");

            if (raceEvent == null)
                return NotFound();

            var categories = await client.GetFromJsonAsync<List<CategoryViewModel>>(
                "api/Categories");

            ViewBag.Categories = categories?
                .Where(c => c.EventID == id)
                .ToList() ?? new List<CategoryViewModel>();

            return View(raceEvent);
        }
    }
}
