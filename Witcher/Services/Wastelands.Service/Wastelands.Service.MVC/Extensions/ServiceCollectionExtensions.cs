using Wastelands.Service.Domain.Contracts;
using Wastelands.Service.Domain.Contracts.Repositories;
using Wastelands.Service.Infrastructure.Mapping;
using Wastelands.Service.Infrastructure.Options;
using Wastelands.Service.Infrastructure.Repositories;
using Wastelands.Service.Infrastructure.Services;

namespace Wastelands.Service.MVC.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection ConfigureServices(
		this IServiceCollection services,
		IConfiguration configuration)
		{
			services.Configure<HasherOptions>(configuration.GetSection(HasherOptions.SectionName));

			services.AddScoped<IUserRepository, UserRepository>();

			services.AddScoped<IPasswordService, PasswordService>();
			services.AddScoped<IUserService, UserService>();

			services.AddAutoMapper(typeof(MappingProfile));

			return services;
		}
	}
}
