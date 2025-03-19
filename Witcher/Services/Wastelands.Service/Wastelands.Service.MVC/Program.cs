using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json.Serialization;
using Wastelands.EfDataAccess.Extensions;
using Wastelands.Service.Infrastructure;
using Wastelands.Service.MVC.Extensions;
using Wastelands.Service.MVC.Validations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvcCore().AddRazorViewEngine();
builder.Services.AddControllersWithViews()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});

builder.Services.AddHttpContextAccessor();

builder.Services.AddFluentValidationAutoValidation(configuration => configuration.DisableDataAnnotationsValidation = true)
	.AddValidatorsFromAssemblyContaining<LoginUserRequestValidator>();

ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath = "/login");

builder.Services.AddServiceDbContext<WastelandsDbContext>(builder.Configuration, "postgres");

builder.Services.ConfigureServices(builder.Configuration);

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

app.Use(next => context =>
{
	context.Request.EnableBuffering();
	return next(context);
});

await app.MigrateDatabaseAsync<WastelandsDbContext>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Character}/{action=Index}")
	.WithStaticAssets();

app.Run();