
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using RaceDay.MVC.Models;

namespace RaceDay.MVC.Controllers
{
    public class ResultsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ResultsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> My()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.GetAsync("api/Results");

            if (response.StatusCode ==
                System.Net.HttpStatusCode.Unauthorized)
            {
                TempData["Error"] =
                    "Your session has expired. Please log in again.";

                return RedirectToAction("Login", "Account");
            }

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error =
                    "Unable to load your results. Please try again.";

                return View(new List<ResultViewModel>());
            }

            var results = await response.Content
                .ReadFromJsonAsync<List<ResultViewModel>>();

            return View(results ?? new List<ResultViewModel>());
        }
    }
}
