using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Requests.BattleRequests;

namespace Witcher.UnitTest.Core.Requests.BattleRequests
{
	[TestClass]
	public sealed class AddCharacterToBattleHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Battle _battle;
		private readonly Character _character;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly Creature _creature;
		private readonly BodyTemplate _bodyTemplate;

		public AddCharacterToBattleHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_battle = Battle.CreateForTest(game: _game);
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);
			_creatureTemplate = CreatureTemplate.CreateForTest(game: _game, bodyTemplate: _bodyTemplate);
			_creature = Creature.CreateForTest(battle: _battle, creatureTemlpate: _creatureTemplate);
			_character = Character.CreateForTest(game: _game, creatureTemlpate: _creatureTemplate);

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_battle,
				_character,
				_creature));
		}

		/// <summary>
		/// Тест метода Handle - добавить персонажа в бой
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_AddCharacterToBattle_ShouldReturnUnit()
		{
			var request = new AddCharacterToBattleCommand()
			{
				BattleId = _battle.Id,
				CharacterId = _character.Id,
			};

			var newHandler = new AddCharacterToBattleHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			var battle = _dbContext.Battles.FirstOrDefault();
			Assert.IsNotNull(battle);

			var character = battle.Creatures.Find(x => x.Id == _character.Id);
			var creature = battle.Creatures.Find(x => x.Id == _creature.Id);
			Assert.IsNotNull(character);
			Assert.IsNotNull(creature);

			Assert.IsTrue(creature.InitiativeInBattle > 0);
			Assert.IsTrue(character.InitiativeInBattle > 0);
		}
	}
}
