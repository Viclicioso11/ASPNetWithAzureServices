using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using NicasourseAssesment.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("NicasourseAssesmentContextConnection") ?? throw new InvalidOperationException("Connection string 'NicasourseAssesmentContextConnection' not found.");

builder.Services.AddDbContext<NicasourseAssesmentContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<NicasourseAssesmentContext>();

var config = builder.Configuration;
// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddInfrastructureDependencyInjection(config);

builder.Services.AddMicrosoftIdentityWebAppAuthentication(config, "ActiveDirectory");

builder.Services.AddMvc(options =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();

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

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.Run();
