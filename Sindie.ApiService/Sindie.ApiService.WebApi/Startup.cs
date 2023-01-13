using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Sindie.ApiService.Core;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Services.Hasher;
using Sindie.ApiService.Storage.Postgresql;
using Sindie.ApiService.WebApi.ExseptionMiddelware;
using Sindie.ApiService.WebApi.Services;
using Sindie.ApiService.WebApi.Swagger;
using Sindie.ApiService.WebApi.Versioning;

namespace Sindie.ApiService.WebApi
{
	public class Startup
	{
		public Startup(IConfiguration configuration, IHostEnvironment env)
		{
			Configuration = configuration;
			Env = env;
		}

		public IConfiguration Configuration { get; }

		public IHostEnvironment Env { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			var options = Configuration.Get<PostgreSqlOptions>();
			var hasherOptions = Configuration.Get<HasherOptions>();
			var AuthServer = Configuration["Auth:AuthServer"];
			var AuthClient = Configuration["Auth:AuthClient"];
			var AuthKey = Configuration["Auth:AuthKey"];

			var sqlLoggerFactory = Env.IsDevelopment()
				? LoggerFactory.Create(builder => { builder.AddConsole(); })
				: null;

			services.AddPostgreSqlStorage(options, sqlLoggerFactory);
			services.AddCore(hasherOptions);
			services.AddControllers();

			services.AddCustomSwagger();
			services.AddCustomApiVersioning();

			services.AddHttpContextAccessor();
			services.AddTransient<IUserContext, UserContext>();

			services.AddCors(o => o.AddPolicy("AllowAll", builder =>
			{
				builder.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader();
			}));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseCors("AllowAll");

			app.UseMiddleware<ExсeptionHandlingMiddelware>();
			app.UseSerilogRequestLogging();

			app.UseRouting();

			app.UseDefaultFiles();
			app.UseStaticFiles();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			app.UseCustomSwagger();
		}
	}
}
