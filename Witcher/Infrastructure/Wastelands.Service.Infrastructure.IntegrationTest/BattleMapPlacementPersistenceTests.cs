using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Infrastructure.IntegrationTest.TestSupport;
using Wastelands.Service.Infrastructure.Repositories;

namespace Wastelands.Service.Infrastructure.IntegrationTest
{
	/// <summary>
	/// Позиции участников на карте боя и маркеры гексов доходят до БД, а удаление карты не удаляет бой,
	/// а отвязывает его (FK Battle.BattleMapId — SetNull).
	/// </summary>
	[TestClass]
	public class BattleMapPlacementPersistenceTests
	{
		[TestMethod]
		public async Task PlacementAndMarker_PersistAndReload()
		{
			var (battleId, mapId, creatureId, characterId, userContext) = await ArrangeAsync();

			await using (var writeContext = CreateDbContext())
			{
				var battles = new BattleRepository(writeContext, userContext);
				var maps = new BattleMapRepository(writeContext, userContext);
				var battle = (await battles.GetByIdAsync(battleId))!;
				var map = (await maps.GetByIdAsync(mapId))!;

				battle.AttachMap(map);
				battle.PlaceParticipantOnMap(ParticipantKind.Creature, creatureId, map, 1, 1);
				battle.PlaceParticipantOnMap(ParticipantKind.Character, characterId, map, 2, 1);
				map.SetMarker(0, 2, "Засада");
				await battles.UpdateAsync(battle);
				await maps.SaveTrackedChangesAsync();
			}

			await using var readContext = CreateDbContext();
			var reloaded = (await new BattleRepository(readContext, userContext).GetByIdAsync(battleId))!;
			var reloadedMap = (await new BattleMapRepository(readContext, userContext).GetByIdAsync(mapId))!;

			reloaded.BattleMapId.Should().Be(mapId);
			reloaded.FindParticipantAt(1, 1).Should().Be((ParticipantKind.Creature, creatureId));
			reloaded.FindParticipantAt(2, 1).Should().Be((ParticipantKind.Character, characterId));
			reloadedMap.FindHex(0, 2)!.MarkerText.Should().Be("Засада");
		}

		[TestMethod]
		public async Task DeletingAttachedMap_KeepsBattleWithoutMap()
		{
			var (battleId, mapId, _, _, userContext) = await ArrangeAsync();

			await using (var writeContext = CreateDbContext())
			{
				var battles = new BattleRepository(writeContext, userContext);
				var maps = new BattleMapRepository(writeContext, userContext);
				var battle = (await battles.GetByIdAsync(battleId))!;
				battle.AttachMap((await maps.GetByIdAsync(mapId))!);
				await battles.UpdateAsync(battle);
			}

			await using (var deleteContext = CreateDbContext())
			{
				var maps = new BattleMapRepository(deleteContext, userContext);
				await maps.DeleteAsync((await maps.GetByIdAsync(mapId))!);
			}

			await using var readContext = CreateDbContext();
			var reloaded = await new BattleRepository(readContext, userContext).GetByIdAsync(battleId);

			reloaded.Should().NotBeNull();
			reloaded!.BattleMapId.Should().BeNull();
		}

		private static async Task<(long BattleId, long MapId, long CreatureId, long CharacterId, TestUserContext UserContext)> ArrangeAsync()
		{
			await using var dbContext = CreateDbContext();
			var owner = await TestDataFactory.CreateUserAsync(dbContext, "owner");
			var game = await TestDataFactory.CreateGameAsync(dbContext, owner.Id);
			var bodyTemplate = await TestDataFactory.CreateBodyTemplateAsync(dbContext, game.Id);
			var creatureTemplate = await TestDataFactory.CreateCreatureTemplateAsync(dbContext, game.Id, bodyTemplate);
			var character = await TestDataFactory.CreateCharacterAsync(dbContext, owner.Id, game.Id, "Hero");
			var map = await TestDataFactory.CreateBattleMapAsync(dbContext, game.Id, columns: 4, rows: 4);

			var battle = new Battle(game.Id, "Battle");
			dbContext.Set<Battle>().Add(battle);
			await dbContext.SaveChangesAsync();

			var creature = new Creature(battle.Id, creatureTemplate, null);
			battle.Creatures.Add(creature);
			battle.Characters.Add(new BattleCharacter(battle.Id, character));
			await dbContext.SaveChangesAsync();

			return (battle.Id, map.Id, creature.Id, character.Id, new TestUserContext { CurrentUserId = owner.Id });
		}

		private static WastelandsDbContext CreateDbContext()
		{
			var options = new DbContextOptionsBuilder<WastelandsDbContext>().UseNpgsql(AssemblyFixture.DataSource).Options;
			return new WastelandsDbContext(options);
		}
	}
}
