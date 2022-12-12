using Maxsys.WorldCupPredictTheScore.Web.Areas.Identity.Models;
using Maxsys.WorldCupPredictTheScore.Web.Core.Extensions;
using Maxsys.WorldCupPredictTheScore.Web.Data;
using Maxsys.WorldCupPredictTheScore.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddMapper();



builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Gerencia p�ginas de erro
app.UseStatusCodePagesWithRedirects("/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

await Seeder.CreateAsync(app);
await Seeder.SeedAsync(app);
await Seeder.SeedRoles(app, addTestUsers:false);
app.Run();

// TODO Implementing the Secure Account Activation https://coding.abel.nu/2015/05/secure-account-activation-with-asp-net-identity/
// TODO Para próxima versão, PredictionScore (PredictScore) e PredictionResult podem ser unificados. Basta colocar a prop Points nulável em PredictionScore.
