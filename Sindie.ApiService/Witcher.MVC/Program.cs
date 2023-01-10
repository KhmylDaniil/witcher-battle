using Sindie.ApiService.Storage.Postgresql;
using Witcher.MVC;
using Witcher.MVC.Logger;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog(LogLevel.Warning);

// Add services to the container.
Startup.ConfigureServices(builder.Services, builder.Configuration, builder.Environment);

var app = builder.Build();

Entry.MigrateDB(app.Services);

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

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(endpoints =>
{
endpoints.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
});

app.Run();
