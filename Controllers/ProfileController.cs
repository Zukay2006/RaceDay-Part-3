
using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Models;
using System.Net.Http.Json;

namespace RaceDay.MVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProfileController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private bool IsParticipant()
        {
            return HttpContext.Session.GetString("Role") == "Participant";
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.GetAsync("api/Profile");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Unable to load your profile.";
                return View(new ProfileViewModel());
            }

            var profile = await response.Content.ReadFromJsonAsync<ProfileViewModel>();

            return View(profile ?? new ProfileViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileViewModel model)
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Account");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var updateRequest = new
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                Password = model.NewPassword
            };

            var response = await client.PutAsJsonAsync("api/Profile", updateRequest);

            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("FirstName", model.FirstName);
                HttpContext.Session.SetString("LastName", model.LastName);
                HttpContext.Session.SetString("Email", model.Email);

                TempData["Success"] = "Profile updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Unable to update your profile. Please check your details.";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPicture(IFormFile profilePicture)
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Account");

            if (profilePicture == null || profilePicture.Length == 0)
            {
                TempData["Error"] = "Please select a picture.";
                return RedirectToAction(nameof(Index));
            }

            using var content = new MultipartFormDataContent();
            using var stream = profilePicture.OpenReadStream();
            using var fileContent = new StreamContent(stream);

            fileContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(
                    profilePicture.ContentType);

            content.Add(fileContent, "file", profilePicture.FileName);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.PostAsync("api/Profile/picture", content);

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Profile picture uploaded successfully!";
            else
                TempData["Error"] = "Picture upload failed. Please try again.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Picture(string id)
        {
            if (!IsParticipant())
                return RedirectToAction("Login", "Account");

            if (string.IsNullOrWhiteSpace(id) ||
                Path.GetFileName(id) != id)
                return BadRequest();

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            var response = await client.GetAsync(
                $"api/Profile/picture/{Uri.EscapeDataString(id)}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var imageStream = await response.Content.ReadAsStreamAsync();

            var contentType = response.Content.Headers.ContentType?.MediaType
                              ?? "application/octet-stream";

            return File(imageStream, contentType);
        }
    }
}
