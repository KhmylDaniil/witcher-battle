using Sindie.ApiService.Core;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Services.Hasher;
using Sindie.ApiService.Storage.Postgresql;

namespace Witcher.MVC
{
	public class Startup
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

			services.AddCore(
				hasherOptions: configuration.Get<HasherOptions>(),
				authServer: configuration["Auth:AuthServer"],
				authClient: configuration["Auth:AuthClient"],
				authKey: configuration["Auth:AuthKey"]);

			services.AddHttpContextAccessor();
			services.AddTransient<IUserContext, UserContext>();

			services.AddCors(o => o.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
		}
	}
}
