using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests.TurnBeginning;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Entities.Effects;
using Sindie.ApiService.Core.Requests.RunBattleRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.UnitTest.Core.RunBattleRequests
{
	/// <summary>
	/// Тест для <see cref="TurnBeginningHandler"/>
	/// </summary>
	[TestClass]
	public class TurnBeginningHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Battle _battle;
		private readonly CreaturePart _headPart;
		private readonly CreaturePart _torsoPart;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly Creature _creature;

		/// <summary>
		/// Тест для <see cref="TurnBeginningHandler"/>
		/// </summary>
		public TurnBeginningHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_battle = Battle.CreateForTest(game: _game);

			_creatureTemplate = CreatureTemplate.CreateForTest(
				game: _game,
				bodyTemplate: BodyTemplate.CreateForTest(game: _game),
				creatureType: CreatureType.Human);

			_creature = Creature.CreateForTest(
				battle: _battle,
				creatureTemlpate: _creatureTemplate,
				creatureType: CreatureType.Human,
				hp: 50,
				@ref: 7,
				speed: 7);

			_headPart = CreaturePart.CreateForTest(
				creature: _creature,
				bodyPartType: BodyPartType.Head,
				damageModifier: 3,
				hitPenalty: 6,
				minToHit: 1,
				maxToHit: 3,
				startingArmor: 5,
				currentArmor: 5);
			_torsoPart = CreaturePart.CreateForTest(
				creature: _creature,
				bodyPartType: BodyPartType.Torso,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 4,
				maxToHit: 10,
				currentArmor: 3,
				startingArmor: 3);
			_creature.CreatureParts.AddRange(new List<CreaturePart> { _headPart, _torsoPart });

			_creature.Effects.Add(FireEffect.CreateForTest(creature: _creature));
			_creature.Effects.Add(BleedingWoundEffect.CreateForTest(creature: _creature, damage: 3));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_battle,
				_creatureTemplate,
				_creature));
		}

		/// <summary>
		/// Тест метода Handle - обработка начала хода
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task TurnBeginning_ShouldReturnUnit()
		{
			var request = new TurnBeginningCommand()
			{
				BattleId = _battle.Id,
				CreatureId = _creature.Id,
			};

			var newHandler = new TurnBeginningHandler(_dbContext, AuthorizationService.Object, RollService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var message = result.Message;
			Assert.IsNotNull(message); ;
			Assert.IsTrue(message.Contains("3 урона"));
			Assert.IsTrue(message.Contains("2 урона"));

			var monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster);
			Assert.AreEqual(monster.HP, 45);
			var torsoPart = monster.CreatureParts.FirstOrDefault(x => x.BodyPartType == BodyPartType.Torso);
			Assert.IsNotNull(torsoPart);
			Assert.AreEqual(torsoPart.CurrentArmor, 2);

			result = await newHandler.Handle(request, default);
			Assert.IsNotNull(result);

			message = result.Message;
			Assert.IsNotNull(message);
			Assert.IsTrue(message.Contains("4 урона"));

			monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster);
			Assert.AreEqual(monster.HP, 38);
			torsoPart = monster.CreatureParts.FirstOrDefault(x => x.BodyPartType == BodyPartType.Torso);
			Assert.IsNotNull(torsoPart);
			Assert.AreEqual(torsoPart.CurrentArmor, 1);
		}
	}
}
