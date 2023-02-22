using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Sindie.ApiService.Core;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Services.Hasher;
using Sindie.ApiService.Core.Services.Token;
using Sindie.ApiService.Storage.Postgresql;
using Sindie.ApiService.WebApi.ExseptionMiddelware;
using Sindie.ApiService.WebApi.Services;
using Sindie.ApiService.WebApi.Swagger;
using Sindie.ApiService.WebApi.Versioning;
using System.Collections.Generic;

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
			services.AddCore(hasherOptions, AuthServer, AuthClient, AuthKey);
			services.AddControllers();

			services.AddCustomSwagger();
			services.AddCustomApiVersioning();

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.RequireHttpsMetadata = true;
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidIssuer = AuthServer,
						ValidateAudience = true,
						ValidAudience = AuthClient,
						ValidateLifetime = true,
						IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
						ValidateIssuerSigningKey = true,
					};
				});

			services.AddHttpContextAccessor();
			services.AddTransient<IUserContext, UserContext>();

			services.AddCors(o => o.AddPolicy("AllowAll", builder =>
			{
				builder.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader();
			}));

			//services.AddRequestLogging();
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
