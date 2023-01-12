using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Sindie.ApiService.Core;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Services.Hasher;
using Sindie.ApiService.Core.Services.Token;
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

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidIssuer = configuration["Auth:AuthServer"],
						ValidateAudience = true,
						ValidAudience = configuration["Auth:AuthClient"],
						ValidateLifetime = true,
						IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
						ValidateIssuerSigningKey = true,
					};
				});

			services.AddHttpContextAccessor();
			services.AddTransient<IUserContext, UserContext>();

			services.AddCors(o => o.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
		}
	}
}
