using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.BattleRequests;
using System.Linq;
using System.Threading.Tasks;

namespace Witcher.UnitTest.Core.Requests.BattleRequests
{
	[TestClass]
	public sealed class DeleteBattleHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Battle _battle;
		private readonly Character _character;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly Creature _creature;
		private readonly BodyTemplate _bodyTemplate;

		/// <summary>
		/// Конструктор для теста <see cref="DeleteBattleHandler"/>
		/// </summary>
		public DeleteBattleHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_battle = Battle.CreateForTest(game: _game);
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);
			_creatureTemplate = CreatureTemplate.CreateForTest(game: _game, bodyTemplate: _bodyTemplate);

			_creature = Creature.CreateForTest(battle: _battle, creatureTemlpate: _creatureTemplate);
			_character = Character.CreateForTest(game: _game, battle: _battle, creatureTemlpate: _creatureTemplate);

			_battle.Creatures.Add(_creature);
			_battle.Creatures.Add(_character);

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_battle,
				_character,
				_creature));
		}

		/// <summary>
		/// Тест метода Handle - удаление битвы по айди
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_DeleteBattle_ShouldReturnUnit()
		{
			var request = new DeleteBattleCommand { Id = _battle.Id };

			var newHandler = new DeleteBattleHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			Assert.IsNull(_dbContext.Battles.FirstOrDefault(x => x.Id == _battle.Id));

			var characterInDb = _dbContext.Characters.FirstOrDefault(x => x.Id == _character.Id);
			var creatureInDb = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(characterInDb);
			Assert.IsNull(creatureInDb);
		}
	}
}
