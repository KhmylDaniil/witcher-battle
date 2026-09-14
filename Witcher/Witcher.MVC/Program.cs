using Witcher.Storage.Postgresql;
using Witcher.MVC;
using Witcher.MVC.Hubs;
using Witcher.MVC.Logger;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureSerilog();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Game}/{action=Index}");

app.MapHub<MessageHub>("/messageHub");

// React SPA (witcher-frontend/) — прод-сборка (`npm run build`) копируется в wwwroot/app, отсюда раздаётся
// статикой (UseStaticFiles выше), а client-side роутинг (react-router) обслуживается этим фолбэком:
// любой GET без расширения файла под /app/** получает index.html и дальше маршрутизируется в браузере.
// В dev SPA не использует этот путь — там Vite dev-server + прокси на /api и /messageHub (см. vite.config.ts).
app.MapFallbackToFile("/app/{*path:nonfile}", "app/index.html");

app.Run();
