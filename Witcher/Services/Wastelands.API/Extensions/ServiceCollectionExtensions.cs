using Microsoft.AspNetCore.Mvc.Filters;
using Wastelands.Core.Contracts.Contracts;
using Wastelands.Minio;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Application.Mapping;
using Wastelands.Service.Application.Options;
using Wastelands.Service.Infrastructure.Repositories;
using Wastelands.Service.Application.Services;
using Wastelands.API.Hubs;
using Wastelands.API.Services;

namespace Wastelands.API.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection ConfigureServices(
		this IServiceCollection services,
		IConfiguration configuration)
		{
			services.Configure<HasherOptions>(configuration.GetSection(HasherOptions.SectionName));
			services.AddMinioImageStorage(configuration);

			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<ICharacterRepository, CharacterRepository>();
			services.AddScoped<IGameRepository, GameRepository>();
			services.AddScoped<IUserGameRepository, UserGameRepository>();
			services.AddScoped<IGameJoinRequestRepository, GameJoinRequestRepository>();
			services.AddScoped<IBodyTemplateRepository, BodyTemplateRepository>();
			services.AddScoped<ICreatureTemplateRepository, CreatureTemplateRepository>();
			services.AddScoped<IItemTemplateRepository, ItemTemplateRepository>();
			services.AddScoped<IBattleRepository, BattleRepository>();

			services.AddScoped<IUserContext, UserContext>();
			services.AddScoped<IGameAccessGuard, GameAccessGuard>();
			services.AddScoped<IBattleNotifier, BattleNotifier>();
			services.AddScoped<IBattleParticipantAuthorizer, BattleParticipantAuthorizer>();
			services.AddScoped<IBattleCombatContextProvider, BattleCombatContextProvider>();
			services.AddScoped<IBattleHitResolver, BattleHitResolver>();
			services.AddScoped<IBattleDtoMapper, BattleDtoMapper>();
			services.AddScoped<IBattleTurnProcessor, BattleTurnProcessor>();

			services.AddScoped<IPasswordService, PasswordService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<ICharacterService, CharacterService>();
			services.AddScoped<ICharacterAbilityService, CharacterAbilityService>();
			services.AddScoped<ICharacterItemService, CharacterItemService>();
			services.AddScoped<IGameService, GameService>();
			services.AddScoped<IGameJoinRequestService, GameJoinRequestService>();
			services.AddScoped<IBodyTemplateService, BodyTemplateService>();
			services.AddScoped<ICreatureTemplateService, CreatureTemplateService>();
			services.AddScoped<ICreatureTemplateAbilityService, CreatureTemplateAbilityService>();
			services.AddScoped<ICreatureTemplateImageService, CreatureTemplateImageService>();
			services.AddScoped<IItemTemplateService, ItemTemplateService>();
			services.AddScoped<ICharacterImageService, CharacterImageService>();
			services.AddScoped<IBattleService, BattleService>();
			services.AddScoped<IBattleCombatService, BattleCombatService>();
			services.AddScoped<IBattleParticipantSheetService, BattleParticipantSheetService>();

			services.AddAutoMapper(typeof(MappingProfile));

			return services;
		}
	}
}
