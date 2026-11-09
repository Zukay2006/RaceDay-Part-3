
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using RaceDay.MVC.Models;

namespace RaceDay.MVC.Controllers
{
    public class EnrolmentsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EnrolmentsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            EnrolmentRequestViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please select a valid category.";
                return RedirectToAction(
                    "Details", "Events", new { id = model.EventID });
            }

            if (HttpContext.Session.GetString("Role") != "Participant")
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.PostAsJsonAsync(
                "api/Enrolments", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] =
                    "You have successfully enrolled in this event!";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();

                TempData["Error"] = response.StatusCode ==
                    System.Net.HttpStatusCode.Unauthorized
                    ? "Your session has expired. Please log in again."
                    : error.Contains("already enrolled",
                        StringComparison.OrdinalIgnoreCase)
                        ? "You are already enrolled in this category."
                        : "Enrolment failed. Please try again.";
            }

            return RedirectToAction(
                "Details", "Events", new { id = model.EventID });
        }

        [HttpGet]
        public async Task<IActionResult> My()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
            {
                return RedirectToAction("Login", "Account");
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.GetAsync("api/Enrolments/my");

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
                    "Unable to load your enrolments. Please try again.";
                return View(new List<EnrolmentViewModel>());
            }

            var enrolments = await response.Content
                .ReadFromJsonAsync<List<EnrolmentViewModel>>();

            return View(enrolments ?? new List<EnrolmentViewModel>());
        }
    }
}
