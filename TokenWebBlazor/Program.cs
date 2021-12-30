using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using TokenApp.Data;
using TokenWebBlazor;
using TokenWebBlazor.ConnectAPI;
using TokenWebBlazor.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddBlazoredLocalStorage();
builder.Services.AddTransient<IUserConnectAPI, UserConnectAPI>();
builder.Services.AddTransient<ITokenConnectAPI, TokenConnectAPI>();
builder.Services.AddHttpClient();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(
                   builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped(x => new HttpClient { BaseAddress = new Uri("https://localhost:7067") });

builder.Services.AddSingleton<WeatherForecastService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();


app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
