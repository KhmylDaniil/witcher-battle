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
	public sealed class AddCreatureToBattleHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Battle _battle;
		private readonly CreatureTemplate _creatureTemplate;

		private readonly BodyTemplate _bodyTemplate;

		public AddCreatureToBattleHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_battle = Battle.CreateForTest(game: _game);
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);
			_creatureTemplate = CreatureTemplate.CreateForTest(game: _game, bodyTemplate: _bodyTemplate);
			_creatureTemplate.DamageTypeModifiers
				.Add(new(_creatureTemplate.Id, Witcher.Core.BaseData.Enums.DamageType.Slashing, Witcher.Core.BaseData.Enums.DamageTypeModifier.Vulnerability));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_battle,
				_creatureTemplate));
		}

		/// <summary>
		/// Тест метода Handle - добавить существо в бой
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_AddCreatureToBattle_ShouldReturnUnit()
		{
			var request = new CreateCreatureCommand()
			{
				BattleId = _battle.Id,
				CreatureTemplateId = _creatureTemplate.Id,
				Name = "testCreature"
			};

			var newHandler = new CreateCreatureHandler(_dbContext, AuthorizationService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);
			var battle = _dbContext.Battles.FirstOrDefault();
			Assert.IsNotNull(battle);

			var creature = battle.Creatures.Find(x => x.CreatureTemplateId == _creatureTemplate.Id);
			Assert.IsNotNull(creature);

			Assert.IsTrue(creature.InitiativeInBattle > 0);
			Assert.AreEqual("testCreature", creature.Name);
			Assert.IsNotNull(creature.DamageTypeModifiers);
			var damageTypeModifer = creature.DamageTypeModifiers.FirstOrDefault();

			Assert.AreEqual(Witcher.Core.BaseData.Enums.DamageType.Slashing, damageTypeModifer.DamageType);
			Assert.AreEqual(Witcher.Core.BaseData.Enums.DamageTypeModifier.Vulnerability, damageTypeModifer.DamageTypeModifier);
		}
	}
}
