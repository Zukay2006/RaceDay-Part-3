
using RaceDay.MVC.Services;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// Session management
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// HTTP client for communicating with RaceDay API
builder.Services.AddTransient<ApiCookieHandler>();

builder.Services.AddHttpClient("RaceDayAPI", client =>
{
    var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"]
        ?? "https://localhost:7064/";

    client.BaseAddress = new Uri(apiBaseUrl);
})
.AddHttpMessageHandler<ApiCookieHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
