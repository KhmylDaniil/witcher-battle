using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.BattleRequests.TreatEffect;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Entities.Effects;
using Sindie.ApiService.Core.Requests.BattleRequests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.BattleRequests
{
	/// <summary>
	/// Тест для <see cref="TreatEffectHandler"/>
	/// </summary>
	[TestClass]
	public class TreatEffectHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Battle _battle;
		private readonly Condition _freezeCondition;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly Creature _creature;
		private readonly CreatureType _creatureType;
		private readonly Skill _skill;

		/// <summary>
		/// Тест для <see cref="TreatEffectHandler"/>
		/// </summary>
		public TreatEffectHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_creatureType = CreatureType.CreateForTest();
			_battle = Battle.CreateForTest(game: _game);
			_freezeCondition = Condition.CreateForTest(id: Conditions.FreezeId, name: Conditions.FreezeName);
			_skill = Skill.CreateForTest(id: Skills.PhysiqueId, name: Skills.PhysiqueName, statName: "Body");

			_creatureTemplate = CreatureTemplate.CreateForTest(
				game: _game,
				bodyTemplate: BodyTemplate.CreateForTest(game: _game),
				creatureType: _creatureType);

			_creature = Creature.CreateForTest(
				battle: _battle,
				creatureTemlpate: _creatureTemplate,
				creatureType: _creatureType,
				hp: 50,
				@ref: 2,
				speed: 2,
				body: 10);

			_creature.Effects.Add(FreezeEffect.CreateForTest(creature: _creature, condition: _freezeCondition));
			_creature.CreatureSkills.Add(new CreatureSkill(7, _creature, _skill));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_battle,
				_skill,
				_freezeCondition,
				_creatureTemplate,
				_creature,
				_creatureType));
		}

		/// <summary>
		/// Тест метода Handle - обработка попытки снятия эффекта
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_TreatEffect_ShouldReturnUnit()
		{
			var request = new TreatEffectCommand()
			{
				BattleId = _battle.Id,
				CreatureId = _creature.Id,
				EffectId = _freezeCondition.Id,
			};

			var newHandler = new TreatEffectHandler(_dbContext, AuthorizationService.Object, RollService.Object);

			var monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster);
			Assert.AreEqual(monster.Speed, 1);
			Assert.AreEqual(monster.Ref, 1);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var message = result.Message;
			Assert.IsNotNull(message); ;
			Assert.IsTrue(message.Contains(Conditions.FreezeName));
			Assert.IsTrue(message.Contains("снят"));

			monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster);
			Assert.AreEqual(monster.Speed, 2);
			Assert.AreEqual(monster.Ref, 2);
		}
	}
}
