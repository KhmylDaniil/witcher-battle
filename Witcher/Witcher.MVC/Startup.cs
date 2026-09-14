using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Witcher.Core;
using Witcher.Core.Abstractions;
using Witcher.Core.Services.Hasher;
using Witcher.MVC.Services;
using Witcher.Storage.Postgresql;

namespace Witcher.MVC
{
	public static class Startup
	{
		public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
		{
			services.AddMvcCore().AddRazorViewEngine();
			services.AddControllersWithViews()
				.AddJsonOptions(options =>
				{
					// Contracts/*Requests местами содержат enum и ValueTuple (напр. (int current, int max) HP) —
					// без этих опций STJ сериализует enum числом, а ValueTuple полями Item1/Item2 не читает вовсе (пустой объект).
					options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
					options.JsonSerializerOptions.IncludeFields = true;
				});

			var sqlLoggerFactory = environment.IsDevelopment()
				? LoggerFactory.Create(builder => builder.AddConsole())
				: null;

			services.AddPostgreSqlStorage(
				options: configuration.Get<PostgreSqlOptions>(),
				sqlLoggerFactory: sqlLoggerFactory);

			services.AddCore(hasherOptions: configuration.Get<HasherOptions>());

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
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

			services.AddMemoryCache(options => options.ExpirationScanFrequency = TimeSpan.FromSeconds(600));

			services.AddHttpContextAccessor();
			services.AddTransient<IUserContext, UserContext>();

			services.AddCors(o => o.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

			services.AddSignalR();
			services.AddSingleton<IUserIdProvider, SignalRUserProvider>();

			// AutoMapper 16 folded DI registration into the main package with a different signature —
			// configAction is required now (no-op here, profiles are still discovered from the marker assembly).
			services.AddAutoMapper(cfg => { }, typeof(Program));
		}
	}
}
