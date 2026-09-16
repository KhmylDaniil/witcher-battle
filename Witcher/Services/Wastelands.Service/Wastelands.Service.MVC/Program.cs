using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json.Serialization;
using Wastelands.EfDataAccess.Extensions;
using Wastelands.Service.Infrastructure;
using Wastelands.Service.MVC.Extensions;
using Wastelands.Service.MVC.Validations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
	});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.SupportNonNullableReferenceTypes();
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddFluentValidationAutoValidation(configuration => configuration.DisableDataAnnotationsValidation = true)
	.AddValidatorsFromAssemblyContaining<LoginUserRequestValidator>();

ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
	options.LoginPath = "/login";

	// JSON API-клиент (SPA) ожидает 401/403 статусы, а не редирект на страницу логина —
	// редиректим только запросы Razor-страниц, /api/** оставляем со статус-кодом как есть.
	var originalRedirectToLogin = options.Events.OnRedirectToLogin;
	options.Events.OnRedirectToLogin = context =>
	{
		if (context.Request.Path.StartsWithSegments("/api"))
		{
			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			return Task.CompletedTask;
		}
		return originalRedirectToLogin(context);
	};

	var originalRedirectToAccessDenied = options.Events.OnRedirectToAccessDenied;
	options.Events.OnRedirectToAccessDenied = context =>
	{
		if (context.Request.Path.StartsWithSegments("/api"))
		{
			context.Response.StatusCode = StatusCodes.Status403Forbidden;
			return Task.CompletedTask;
		}
		return originalRedirectToAccessDenied(context);
	};
});

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
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
	// RoutePrefix пустой (UI на "/"), но JSON-документ всё равно раздаётся по маршруту
	// UseSwagger() по умолчанию ("swagger/{documentName}/swagger.json") — указываем его явно,
	// иначе UI резолвит относительный путь "v1/swagger.json" от "/" и получает 404.
	options.RoutePrefix = string.Empty;
	options.SwaggerEndpoint("/swagger/v1/swagger.json", "Wastelands.Service API v1");
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// React SPA (witcher-frontend/) — прод-сборка (`npm run build`) копируется в wwwroot/app, отсюда раздаётся
// статикой (UseStaticFiles выше), а client-side роутинг (react-router) обслуживается этим фолбэком.
// В dev SPA не использует этот путь — там Vite dev-server + прокси на /api (см. vite.config.ts).
app.MapFallbackToFile("/app/{*path:nonfile}", "app/index.html");

app.Run();