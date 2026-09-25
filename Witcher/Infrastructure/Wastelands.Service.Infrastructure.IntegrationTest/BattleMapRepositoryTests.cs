using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Wastelands.Core.Contracts.Models;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Domain.Drafts;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Infrastructure.IntegrationTest.TestSupport;
using Wastelands.Service.Infrastructure.Repositories;

namespace Wastelands.Service.Infrastructure.IntegrationTest
{
	/// <summary>
	/// BattleMapRepository: скоуп по владельцу игры (как у шаблонов) и SaveTrackedChangesAsync — Resize
	/// удаляет гексы из коллекции и дописывает новые, это должно дойти до БД через change tracking без
	/// конфликтов с уникальным индексом (BattleMapId, Column, Row).
	/// </summary>
	[TestClass]
	public class BattleMapRepositoryTests
	{
		private WastelandsDbContext _dbContext = null!;

		[TestInitialize]
		public void Setup()
		{
			_dbContext = CreateDbContext();
		}

		[TestCleanup]
		public async Task TeardownAsync() => await _dbContext.DisposeAsync();

		[TestMethod]
		public async Task GetByIdAsync_GameOwner_ReturnsMapWithAllHexes()
		{
			var owner = await TestDataFactory.CreateUserAsync(_dbContext, "owner");
			var game = await TestDataFactory.CreateGameAsync(_dbContext, owner.Id);
			var battleMap = await TestDataFactory.CreateBattleMapAsync(_dbContext, game.Id, columns: 4, rows: 2);

			await using var readContext = CreateDbContext();
			var repository = new BattleMapRepository(readContext, new TestUserContext { CurrentUserId = owner.Id });
			var result = await repository.GetByIdAsync(battleMap.Id);

			result.Should().NotBeNull();
			result!.Hexes.Should().HaveCount(8);
		}

		[TestMethod]
		public async Task GetByIdAsync_NotGameOwner_ReturnsNull()
		{
			var owner = await TestDataFactory.CreateUserAsync(_dbContext, "owner");
			var otherGm = await TestDataFactory.CreateUserAsync(_dbContext, "othergm");
			var game = await TestDataFactory.CreateGameAsync(_dbContext, owner.Id);
			var battleMap = await TestDataFactory.CreateBattleMapAsync(_dbContext, game.Id);

			var repository = new BattleMapRepository(_dbContext, new TestUserContext { CurrentUserId = otherGm.Id });
			var result = await repository.GetByIdAsync(battleMap.Id);

			result.Should().BeNull();
		}

		[TestMethod]
		public async Task GetByIdUnscopedAsync_NotGameOwner_StillReturnsMapWithHexes()
		{
			// Путь для игроков: доступ к карте они получают через идущий бой, а не через владение игрой.
			var owner = await TestDataFactory.CreateUserAsync(_dbContext, "owner");
			var player = await TestDataFactory.CreateUserAsync(_dbContext, "player");
			var game = await TestDataFactory.CreateGameAsync(_dbContext, owner.Id);
			var battleMap = await TestDataFactory.CreateBattleMapAsync(_dbContext, game.Id, columns: 2, rows: 2);

			await using var readContext = CreateDbContext();
			var result = await new BattleMapRepository(readContext, new TestUserContext { CurrentUserId = player.Id }).GetByIdUnscopedAsync(battleMap.Id);

			result.Should().NotBeNull();
			result!.Hexes.Should().HaveCount(4);
		}

		[TestMethod]
		public async Task CreatureTemplateRepository_GetImageKeysUnscopedAsync_IgnoresOwnerScoping()
		{
			var owner = await TestDataFactory.CreateUserAsync(_dbContext, "owner");
			var player = await TestDataFactory.CreateUserAsync(_dbContext, "player");
			var game = await TestDataFactory.CreateGameAsync(_dbContext, owner.Id);
			var bodyTemplate = await TestDataFactory.CreateBodyTemplateAsync(_dbContext, game.Id);
			var template = await TestDataFactory.CreateCreatureTemplateAsync(_dbContext, game.Id, bodyTemplate);

			await using var readContext = CreateDbContext();
			var keys = await new CreatureTemplateRepository(readContext, new TestUserContext { CurrentUserId = player.Id })
				.GetImageKeysUnscopedAsync([template.Id]);

			keys.Should().ContainKey(template.Id);
		}

		[TestMethod]
		public async Task GetPagedWithoutHexesAsync_ReturnsOnlyOwnedMapsOfGame_WithoutHexes()
		{
			var owner = await TestDataFactory.CreateUserAsync(_dbContext, "owner");
			var otherGm = await TestDataFactory.CreateUserAsync(_dbContext, "othergm");
			var game = await TestDataFactory.CreateGameAsync(_dbContext, owner.Id);
			var otherGame = await TestDataFactory.CreateGameAsync(_dbContext, otherGm.Id);
			await TestDataFactory.CreateBattleMapAsync(_dbContext, game.Id, "Mine");
			await TestDataFactory.CreateBattleMapAsync(_dbContext, otherGame.Id, "Theirs");

			await using var readContext = CreateDbContext();
			var repository = new BattleMapRepository(readContext, new TestUserContext { CurrentUserId = owner.Id });
			var mine = await repository.GetPagedWithoutHexesAsync(new PagedRequest(), new BattleMapFilter { GameId = game.Id });
			var theirs = await repository.GetPagedWithoutHexesAsync(new PagedRequest(), new BattleMapFilter { GameId = otherGame.Id });

			mine.Entities.Should().ContainSingle().Which.Name.Should().Be("Mine");
			mine.Entities[0].Hexes.Should().BeEmpty();
			theirs.Entities.Should().BeEmpty();
		}

		[TestMethod]
		public async Task SaveTrackedChangesAsync_PersistsResizeAndPaint()
		{
			var owner = await TestDataFactory.CreateUserAsync(_dbContext, "owner");
			var game = await TestDataFactory.CreateGameAsync(_dbContext, owner.Id);
			var created = await TestDataFactory.CreateBattleMapAsync(_dbContext, game.Id, columns: 3, rows: 3);
			var userContext = new TestUserContext { CurrentUserId = owner.Id };

			await using (var writeContext = CreateDbContext())
			{
				var repository = new BattleMapRepository(writeContext, userContext);
				var battleMap = (await repository.GetByIdAsync(created.Id))!;
				battleMap.Resize(2, 4, HexTerrainStyle.Sand);
				battleMap.PaintHexes([new BattleMapHexPaintDraft(1, 3, HexTerrainType.Wall, HexTerrainStyle.FuturisticMetal)]);
				await repository.SaveTrackedChangesAsync();
			}

			await using var readContext = CreateDbContext();
			var reloaded = (await new BattleMapRepository(readContext, userContext).GetByIdAsync(created.Id))!;

			reloaded.Columns.Should().Be(2);
			reloaded.Rows.Should().Be(4);
			reloaded.Hexes.Should().HaveCount(8);
			reloaded.Hexes.Should().OnlyContain(h => h.Column < 2 && h.Row < 4);
			reloaded.FindHex(1, 3)!.TerrainType.Should().Be(HexTerrainType.Wall);
			reloaded.FindHex(1, 3)!.TerrainStyle.Should().Be(HexTerrainStyle.FuturisticMetal);
			reloaded.FindHex(0, 3)!.TerrainStyle.Should().Be(HexTerrainStyle.Sand);
		}

		private static WastelandsDbContext CreateDbContext()
		{
			var options = new DbContextOptionsBuilder<WastelandsDbContext>().UseNpgsql(AssemblyFixture.DataSource).Options;
			return new WastelandsDbContext(options);
		}
	}
}
