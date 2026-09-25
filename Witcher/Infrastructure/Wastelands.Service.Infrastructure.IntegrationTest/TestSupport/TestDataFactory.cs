using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Infrastructure.IntegrationTest.TestSupport
{
	/// <summary>
	/// Создаёт и сохраняет минимальные валидные фикстуры прямо в тестовую БД. User — единственная
	/// сущность в проекте без публичного конструктора (см. UserService.RegisterUserAsync, который
	/// создаёт его через AutoMapper); здесь тот же эффект достигается напрямую через Activator, чтобы не
	/// тащить в тестовый проект весь маппинг-профиль ради одной сущности.
	/// </summary>
	internal static class TestDataFactory
	{
		public static async Task<User> CreateUserAsync(WastelandsDbContext dbContext, string namePrefix)
		{
			var suffix = Guid.NewGuid().ToString("N")[..8];
			var user = (User)Activator.CreateInstance(typeof(User), nonPublic: true)!;
			user.Name = namePrefix;
			user.Login = $"{namePrefix}_{suffix}";
			user.Password = "hash";

			dbContext.Set<User>().Add(user);
			await dbContext.SaveChangesAsync();

			return user;
		}

		public static async Task<Game> CreateGameAsync(WastelandsDbContext dbContext, long createdByUserId, string name = "Game")
		{
			var game = new Game(name, createdByUserId);

			dbContext.Set<Game>().Add(game);
			await dbContext.SaveChangesAsync();

			return game;
		}

		public static async Task<Character> CreateCharacterAsync(WastelandsDbContext dbContext, long userId, long gameId, string name)
		{
			var character = new Character(userId, gameId, name, hp: 10, sta: 10, @int: 8, str: 8, rea: 8, dex: 8, cra: 8, emp: 8, wil: 8, movement: 5);

			dbContext.Set<Character>().Add(character);
			await dbContext.SaveChangesAsync();

			return character;
		}

		public static async Task<BodyTemplate> CreateBodyTemplateAsync(WastelandsDbContext dbContext, long gameId, string name = "Body")
		{
			var bodyTemplate = new BodyTemplate(gameId, name, null);

			dbContext.Set<BodyTemplate>().Add(bodyTemplate);
			await dbContext.SaveChangesAsync();

			return bodyTemplate;
		}

		public static async Task<CreatureTemplate> CreateCreatureTemplateAsync(WastelandsDbContext dbContext, long gameId, BodyTemplate bodyTemplate, string name = "Wolf")
		{
			var creatureTemplate = new CreatureTemplate(
				gameId, bodyTemplate, CreatureType.Beast, name, null, hp: 10, sta: 10, @int: 8, @ref: 8, dex: 8, body: 8, emp: 8, cra: 8, will: 8, speed: 8, luck: 8, movement: 5);

			dbContext.Set<CreatureTemplate>().Add(creatureTemplate);
			await dbContext.SaveChangesAsync();

			return creatureTemplate;
		}

		public static async Task<ItemTemplate> CreateItemTemplateAsync(WastelandsDbContext dbContext, long gameId, string name = "Item")
		{
			var itemTemplate = ItemTemplate.CreateNonWeapon(gameId, name, null, ItemType.Other, weight: 1, cost: 1);

			dbContext.Set<ItemTemplate>().Add(itemTemplate);
			await dbContext.SaveChangesAsync();

			return itemTemplate;
		}

		public static async Task<BattleMap> CreateBattleMapAsync(WastelandsDbContext dbContext, long gameId, string name = "Map", int columns = 3, int rows = 3)
		{
			var battleMap = new BattleMap(gameId, name, null, columns, rows, HexTerrainStyle.Grass);

			dbContext.Set<BattleMap>().Add(battleMap);
			await dbContext.SaveChangesAsync();

			return battleMap;
		}
	}
}
