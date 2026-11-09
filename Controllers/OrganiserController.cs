using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using System.Net.Http.Json;

namespace RaceDay.MVC.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrganiserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private bool IsOrganiser()
        {
            return HttpContext.Session.GetString("Role") == "Organiser";
        }

        public IActionResult Dashboard()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            return View();
        }

        public async Task<IActionResult> Events()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var events = await client.GetFromJsonAsync<List<OrganiserEventViewModel>>(
                "api/Events");

            return View(events ?? new List<OrganiserEventViewModel>());
        }

        [HttpGet]
        public IActionResult CreateEvent()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            return View(new OrganiserEventViewModel
            {
                EventDate = DateTime.Now.AddDays(7)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(OrganiserEventViewModel model)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.PostAsJsonAsync("api/Events", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Event created successfully!";
                return RedirectToAction(nameof(Events));
            }

            ModelState.AddModelError("", "Unable to create event. Please try again.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditEvent(int id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var model = await client.GetFromJsonAsync<OrganiserEventViewModel>(
                $"api/Events/{id}");

            if (model == null)
                return NotFound();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEvent(
            int id, OrganiserEventViewModel model)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.PutAsJsonAsync(
                $"api/Events/{id}", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Event updated successfully!";
                return RedirectToAction(nameof(Events));
            }

            ModelState.AddModelError("", "Unable to update event.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.DeleteAsync($"api/Events/{id}");

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Event deleted successfully!";
            else
                TempData["Error"] = "Unable to delete event.";

            return RedirectToAction(nameof(Events));
        }

        public async Task<IActionResult> Categories()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var categories = await client.GetFromJsonAsync<List<CategoryViewModel>>(
                "api/Categories");

            return View(categories ?? new List<CategoryViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> CreateCategory()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var events = await client.GetFromJsonAsync<List<OrganiserEventViewModel>>(
                "api/Events");

            ViewBag.Events = events ?? new List<OrganiserEventViewModel>();

            return View(new CategoryViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CategoryViewModel model)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                var clientForEvents = _httpClientFactory.CreateClient("RaceDayAPI");
                ViewBag.Events = await clientForEvents.GetFromJsonAsync<List<OrganiserEventViewModel>>(
                    "api/Events") ?? new List<OrganiserEventViewModel>();

                return View(model);
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.PostAsJsonAsync("api/Categories", model);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Category created successfully!";
                return RedirectToAction(nameof(Categories));
            }

            ModelState.AddModelError("", "Unable to create category. Check that you selected an event you own.");

            ViewBag.Events = await client.GetFromJsonAsync<List<OrganiserEventViewModel>>(
                "api/Events") ?? new List<OrganiserEventViewModel>();

            return View(model);
        }

        public async Task<IActionResult> Enrolments()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var enrolments = await client.GetFromJsonAsync<
                List<OrganiserEnrolmentViewModel>>("api/Enrolments");

            return View(enrolments ?? new List<OrganiserEnrolmentViewModel>());
        }

        public async Task<IActionResult> Results()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var enrolments = await client.GetFromJsonAsync<
                List<OrganiserEnrolmentViewModel>>("api/Enrolments");

            return View(enrolments ?? new List<OrganiserEnrolmentViewModel>());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
     
            public async Task<IActionResult> SaveResult(
    int enrolmentId,
    TimeSpan? finishTime,
    int? position,
    string resultStatus)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            if (!finishTime.HasValue || !position.HasValue)
            {
                TempData["Error"] =
                    "Please enter both a finish time and finishing position.";

                return RedirectToAction(nameof(Results));
            }

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var result = new
            {
                FinishTime = finishTime,
                Position = position,
                ResultStatus = string.IsNullOrWhiteSpace(resultStatus)
                    ? "Pending"
                    : resultStatus
            };

            var response = await client.PutAsJsonAsync(
                $"api/Results/enrolment/{enrolmentId}", result);

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Race result saved successfully!";
            else
                TempData["Error"] =
                    "Unable to save the result. Please check the details.";

            return RedirectToAction(nameof(Results));
        }
    }
}