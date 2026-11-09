
using System.Net.Http;

namespace RaceDay.MVC.Services
{
    public class ApiCookieHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiCookieHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var context = _httpContextAccessor.HttpContext;

            if (context != null)
            {
                var apiCookies = context.Session.GetString("ApiCookies");

                if (!string.IsNullOrEmpty(apiCookies))
                {
                    request.Headers.TryAddWithoutValidation(
                        "Cookie", apiCookies);
                }
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
