using Microsoft.AspNetCore.Mvc.Filters;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Application.Mapping;
using Wastelands.Service.Application.Options;
using Wastelands.Service.Infrastructure.Repositories;
using Wastelands.Service.Application.Services;
using Wastelands.Service.MVC.Services;

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
			services.AddScoped<ICharacterRepository, CharacterRepository>();
			services.AddScoped<IGameRepository, GameRepository>();
			services.AddScoped<IUserGameRepository, UserGameRepository>();
			services.AddScoped<IGameJoinRequestRepository, GameJoinRequestRepository>();
			services.AddScoped<IBodyTemplateRepository, BodyTemplateRepository>();
			services.AddScoped<ICreatureTemplateRepository, CreatureTemplateRepository>();
			services.AddScoped<IBattleRepository, BattleRepository>();

			services.AddScoped<IUserContext, UserContext>();
			services.AddScoped<IGameAccessGuard, GameAccessGuard>();

			services.AddScoped<IPasswordService, PasswordService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<ICharacterService, CharacterService>();
			services.AddScoped<IGameService, GameService>();
			services.AddScoped<IGameJoinRequestService, GameJoinRequestService>();
			services.AddScoped<IBodyTemplateService, BodyTemplateService>();
			services.AddScoped<ICreatureTemplateService, CreatureTemplateService>();
			services.AddScoped<IBattleService, BattleService>();

			services.AddAutoMapper(typeof(MappingProfile));

			return services;
		}
	}
}
