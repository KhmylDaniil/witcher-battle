using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Wastelands.Service.Infrastructure.IntegrationTest.TestSupport;
using Wastelands.Service.Infrastructure.Repositories;

namespace Wastelands.Service.Infrastructure.IntegrationTest
{
	/// <summary>
	/// CharacterRepository.GetQuery() scoping (Where(x => x.UserId == CurrentUserId)) — против реального
	/// Postgres, не мока: это ровно то место, где баг означает утечку чужих персонажей.
	/// </summary>
	[TestClass]
	public class CharacterRepositoryAccessScopingTests
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
		public async Task GetByIdAsync_Owner_ReturnsCharacter()
		{
			var owner = await TestDataFactory.CreateUserAsync(_dbContext, "owner");
			var game = await TestDataFactory.CreateGameAsync(_dbContext, owner.Id);
			var character = await TestDataFactory.CreateCharacterAsync(_dbContext, owner.Id, game.Id, "Hero");

			var repository = new CharacterRepository(_dbContext, new TestUserContext { CurrentUserId = owner.Id });
			var result = await repository.GetByIdAsync(character.Id);

			result.Should().NotBeNull();
			result!.Name.Should().Be("Hero");
		}

		[TestMethod]
		public async Task GetByIdAsync_DifferentUser_ReturnsNull()
		{
			var owner = await TestDataFactory.CreateUserAsync(_dbContext, "owner");
			var stranger = await TestDataFactory.CreateUserAsync(_dbContext, "stranger");
			var game = await TestDataFactory.CreateGameAsync(_dbContext, owner.Id);
			var character = await TestDataFactory.CreateCharacterAsync(_dbContext, owner.Id, game.Id, "Hero");

			var repository = new CharacterRepository(_dbContext, new TestUserContext { CurrentUserId = stranger.Id });
			var result = await repository.GetByIdAsync(character.Id);

			result.Should().BeNull();
		}

		[TestMethod]
		public async Task GetByIdUnscopedAsync_IgnoresOwnerScoping_ReturnsCharacterForAnyUser()
		{
			var owner = await TestDataFactory.CreateUserAsync(_dbContext, "owner");
			var stranger = await TestDataFactory.CreateUserAsync(_dbContext, "stranger");
			var game = await TestDataFactory.CreateGameAsync(_dbContext, owner.Id);
			var character = await TestDataFactory.CreateCharacterAsync(_dbContext, owner.Id, game.Id, "Hero");

			var repository = new CharacterRepository(_dbContext, new TestUserContext { CurrentUserId = stranger.Id });
			var result = await repository.GetByIdUnscopedAsync(character.Id);

			result.Should().NotBeNull();
		}

		[TestMethod]
		public async Task GetCharactersByGameIdAsync_IgnoresOwnerScoping_ReturnsCharactersRegardlessOfCallingUser()
		{
			var owner = await TestDataFactory.CreateUserAsync(_dbContext, "owner");
			var stranger = await TestDataFactory.CreateUserAsync(_dbContext, "stranger");
			var game = await TestDataFactory.CreateGameAsync(_dbContext, owner.Id);
			await TestDataFactory.CreateCharacterAsync(_dbContext, owner.Id, game.Id, "Hero");

			// GM-сценарий: мастер (не владелец персонажа) должен видеть всех персонажей своей игры.
			var repository = new CharacterRepository(_dbContext, new TestUserContext { CurrentUserId = stranger.Id });
			var result = await repository.GetCharactersByGameIdAsync(game.Id);

			result.Should().ContainSingle(c => c.Name == "Hero");
		}
	}
}
