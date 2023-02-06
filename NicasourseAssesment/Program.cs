using Infrastructure;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Logging;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("ActiveDirectory"));

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to 
    // the default policy
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddRazorPages()
.AddMvcOptions(options => { })
.AddMicrosoftIdentityUI();

var config = builder.Configuration;
// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddInfrastructureDependencyInjection(config);

builder.Services.AddMvc()
.AddRazorPagesOptions(options =>
{
    options.Conventions.AddPageRoute("/Files/List", "");
});

var app = builder.Build();

IdentityModelEventSource.ShowPII = true;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

app.Run();
