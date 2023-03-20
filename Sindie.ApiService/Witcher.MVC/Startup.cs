using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using Witcher.Core;
using Witcher.Core.Abstractions;
using Witcher.Core.Services.Hasher;
using Witcher.Storage.Postgresql;

namespace Witcher.MVC
{
	public static class Startup
	{
		public static void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
		{
			services.AddMvcCore().AddRazorViewEngine();
			services.AddControllersWithViews();

			var sqlLoggerFactory = environment.IsDevelopment()
				? LoggerFactory.Create(builder => builder.AddConsole())
				: null;

			services.AddPostgreSqlStorage(
				options: configuration.Get<PostgreSqlOptions>(),
				sqlLoggerFactory: sqlLoggerFactory);

			services.AddCore(hasherOptions: configuration.Get<HasherOptions>());

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath = "/login");

			services.AddMemoryCache(options => options.ExpirationScanFrequency = TimeSpan.FromSeconds(600));

			services.AddHttpContextAccessor();
			services.AddTransient<IUserContext, UserContext>();

			services.AddCors(o => o.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

			services.AddSignalR();

			services.AddAutoMapper(typeof(Program));
		}
	}
}
