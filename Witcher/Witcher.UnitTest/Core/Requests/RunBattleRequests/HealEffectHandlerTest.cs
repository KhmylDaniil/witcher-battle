using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Entities.Effects;
using Witcher.Core.Requests.RunBattleRequests;
using System.Linq;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.UnitTest.Core.RunBattleRequests
{
	/// <summary>
	/// Тест для <see cref="HealEffectHandler"/>
	/// </summary>
	[TestClass]
	public class HealEffectHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Battle _battle;
		private readonly FreezeEffect _freezeEffect;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly Creature _creature;

		/// <summary>
		/// Тест для <see cref="HealEffectHandler"/>
		/// </summary>
		public HealEffectHandlerTest() : base()
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
				@ref: 2,
				speed: 2,
				body: 10);

			_freezeEffect = FreezeEffect.CreateForTest(creature: _creature, name: CritNames.GetConditionFullName(Condition.Freeze));

			_creature.Effects.Add(_freezeEffect);

			_creature.CreatureSkills.Add(new CreatureSkill(7, _creature, Skill.Physique));

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_battle,
				_freezeEffect,
				_creatureTemplate,
				_creature));
		}

		/// <summary>
		/// Тест метода Handle - обработка попытки снятия эффекта
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_TreatEffect_ShouldReturnUnit()
		{
			var request = new HealEffectCommand()
			{
				BattleId = _battle.Id,
				CreatureId = _creature.Id,
				TargetCreatureId = _creature.Id,
				EffectId = _freezeEffect.Id,
			};

			var newHandler = new HealEffectHandler(_dbContext, AuthorizationService.Object, RollService.Object);

			var monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster);
			Assert.AreEqual(monster.Speed, 0);
			Assert.AreEqual(monster.Ref, 1);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var message = _dbContext.Battles.FirstOrDefault(x => x.Id == _battle.Id).BattleLog;
			Assert.IsNotNull(message);
			Assert.IsTrue(message.Contains(CritNames.GetConditionFullName(Condition.Freeze)));
			Assert.IsTrue(message.Contains("снят"));

			monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster);
			Assert.AreEqual(monster.Speed, 2);
			Assert.AreEqual(monster.Ref, 2);
		}
	}
}
