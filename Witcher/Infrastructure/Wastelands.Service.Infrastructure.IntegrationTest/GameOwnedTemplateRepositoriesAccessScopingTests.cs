using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Wastelands.Service.Infrastructure.IntegrationTest.TestSupport;
using Wastelands.Service.Infrastructure.Repositories;

namespace Wastelands.Service.Infrastructure.IntegrationTest
{
	/// <summary>
	/// BodyTemplateRepository/CreatureTemplateRepository/ItemTemplateRepository все скоупят по владению
	/// игрой (GetQuery() джойнит на Game.CreatedByUserId), а не напрямую по UserId, как CharacterRepository
	/// — именно этот join-based скоуп стоит проверить против реального провайдера, а не мока.
	/// </summary>
	[TestClass]
	public class GameOwnedTemplateRepositoriesAccessScopingTests
	{
		private WastelandsDbContext _dbContext = null!;

		[TestInitialize]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<WastelandsDbContext>().UseNpgsql(AssemblyFixture.DataSource).Options;
			_dbContext = new WastelandsDbContext(options);
		}

		[TestCleanup]
		public async Task TeardownAsync() => await _dbContext.DisposeAsync();

		[TestMethod]
		public async Task BodyTemplateRepository_GetByIdAsync_GameOwner_ReturnsTemplate()
		{
			var owner = await TestDataFactory.CreateUserAsync(_dbContext, "owner");
			var game = await TestDataFactory.CreateGameAsync(_dbContext, owner.Id);
			var template = await TestDataFactory.CreateBodyTemplateAsync(_dbContext, game.Id, "Body");

			var repository = new BodyTemplateRepository(_dbContext, new TestUserContext { CurrentUserId = owner.Id });
			var result = await repository.GetByIdAsync(template.Id);

			result.Should().NotBeNull();
		}

		[TestMethod]
		public async Task BodyTemplateRepository_GetByIdAsync_NotGameOwner_ReturnsNull()
		{
			var owner = await TestDataFactory.CreateUserAsync(_dbContext, "owner");
			var otherGm = await TestDataFactory.CreateUserAsync(_dbContext, "othergm");
			var game = await TestDataFactory.CreateGameAsync(_dbContext, owner.Id);
			var template = await TestDataFactory.CreateBodyTemplateAsync(_dbContext, game.Id, "Body");

			var repository = new BodyTemplateRepository(_dbContext, new TestUserContext { CurrentUserId = otherGm.Id });
			var result = await repository.GetByIdAsync(template.Id);

			result.Should().BeNull();
		}

		[TestMethod]
		public async Task CreatureTemplateRepository_GetByIdAsync_GameOwner_ReturnsTemplate()
		{
			var owner = await TestDataFactory.CreateUserAsync(_dbContext, "owner");
			var game = await TestDataFactory.CreateGameAsync(_dbContext, owner.Id);
			var bodyTemplate = await TestDataFactory.CreateBodyTemplateAsync(_dbContext, game.Id);
			var creatureTemplate = await TestDataFactory.CreateCreatureTemplateAsync(_dbContext, game.Id, bodyTemplate, "Wolf");

			var repository = new CreatureTemplateRepository(_dbContext, new TestUserContext { CurrentUserId = owner.Id });
			var result = await repository.GetByIdAsync(creatureTemplate.Id);

			result.Should().NotBeNull();
			result!.Name.Should().Be("Wolf");
		}

		[TestMethod]
		public async Task CreatureTemplateRepository_GetByIdAsync_NotGameOwner_ReturnsNull()
		{
			var owner = await TestDataFactory.CreateUserAsync(_dbContext, "owner");
			var otherGm = await TestDataFactory.CreateUserAsync(_dbContext, "othergm");
			var game = await TestDataFactory.CreateGameAsync(_dbContext, owner.Id);
			var bodyTemplate = await TestDataFactory.CreateBodyTemplateAsync(_dbContext, game.Id);
			var creatureTemplate = await TestDataFactory.CreateCreatureTemplateAsync(_dbContext, game.Id, bodyTemplate, "Wolf");

			var repository = new CreatureTemplateRepository(_dbContext, new TestUserContext { CurrentUserId = otherGm.Id });
			var result = await repository.GetByIdAsync(creatureTemplate.Id);

			result.Should().BeNull();
		}

		[TestMethod]
		public async Task CreatureTemplateRepository_GetByIdUnscopedAsync_IgnoresGameOwnerScoping()
		{
			var owner = await TestDataFactory.CreateUserAsync(_dbContext, "owner");
			var otherGm = await TestDataFactory.CreateUserAsync(_dbContext, "othergm");
			var game = await TestDataFactory.CreateGameAsync(_dbContext, owner.Id);
			var bodyTemplate = await TestDataFactory.CreateBodyTemplateAsync(_dbContext, game.Id);
			var creatureTemplate = await TestDataFactory.CreateCreatureTemplateAsync(_dbContext, game.Id, bodyTemplate, "Wolf");

			var repository = new CreatureTemplateRepository(_dbContext, new TestUserContext { CurrentUserId = otherGm.Id });
			var result = await repository.GetByIdUnscopedAsync(creatureTemplate.Id);

			result.Should().NotBeNull();
		}

		[TestMethod]
		public async Task ItemTemplateRepository_GetByIdAsync_GameOwner_ReturnsTemplate()
		{
			var owner = await TestDataFactory.CreateUserAsync(_dbContext, "owner");
			var game = await TestDataFactory.CreateGameAsync(_dbContext, owner.Id);
			var itemTemplate = await TestDataFactory.CreateItemTemplateAsync(_dbContext, game.Id, "Rope");

			var repository = new ItemTemplateRepository(_dbContext, new TestUserContext { CurrentUserId = owner.Id });
			var result = await repository.GetByIdAsync(itemTemplate.Id);

			result.Should().NotBeNull();
		}

		[TestMethod]
		public async Task ItemTemplateRepository_GetByIdAsync_NotGameOwner_ReturnsNull()
		{
			var owner = await TestDataFactory.CreateUserAsync(_dbContext, "owner");
			var otherGm = await TestDataFactory.CreateUserAsync(_dbContext, "othergm");
			var game = await TestDataFactory.CreateGameAsync(_dbContext, owner.Id);
			var itemTemplate = await TestDataFactory.CreateItemTemplateAsync(_dbContext, game.Id, "Rope");

			var repository = new ItemTemplateRepository(_dbContext, new TestUserContext { CurrentUserId = otherGm.Id });
			var result = await repository.GetByIdAsync(itemTemplate.Id);

			result.Should().BeNull();
		}
	}
}
