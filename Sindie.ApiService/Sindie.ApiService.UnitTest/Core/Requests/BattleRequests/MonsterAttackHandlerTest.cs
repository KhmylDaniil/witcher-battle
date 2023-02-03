using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BattleRequests.MonsterAttack;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.UnitTest.Core.Requests.BattleRequests
{
	/// <summary>
	/// Тест для <see cref="MonsterAttackHandler"/>
	/// </summary>
	[TestClass]
	public class MonsterAttackHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Battle _instance;
		private readonly BodyTemplate _bodyTemplate;
		private readonly CreaturePart _headPart;
		private readonly CreaturePart _torsoPart;
		private readonly Condition _condition;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly Ability _ability;
		private readonly Creature _creature;
		private readonly Condition _head1Crit;
		private readonly Condition _head2Crit;

		/// <summary>
		/// Тест для <see cref="MonsterAttackHandler"/>
		/// </summary>
		public MonsterAttackHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_instance = Battle.CreateForTest(game: _game);
			_condition = Condition.CreateForTest();

			_head1Crit = Condition.CreateForTest(id: Crit.SimpleHead1Id, name: Crit.SimpleHead1);
			_head2Crit = Condition.CreateForTest(id: Crit.SimpleHead2Id, name: Crit.SimpleHead2);

			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);

			_creatureTemplate = CreatureTemplate.CreateForTest(
				game: _game,
				bodyTemplate: _bodyTemplate,
				creatureType: CreatureType.Specter);

			_ability = Ability.CreateForTest(
				game: _game,
				name: "attack",
				attackDiceQuantity: 1,
				damageModifier: 1,
				attackSpeed: 1,
				accuracy: 1,
				defensiveSkills: new List<Skill> { Skill.Melee });
			_ability.AppliedConditions.Add(AppliedCondition.CreateAppliedCondition(_ability, _condition, 100));

			_creature = Creature.CreateForTest(
				battle: _instance,
				creatureTemlpate: _creatureTemplate,
				creatureType: CreatureType.Specter);
			_creature.Abilities.Add(_ability);
			_creature.CreatureSkills.Add(CreatureSkill.CreateForTest(
				creature: _creature,
				skill: Skill.Melee,
				value: 10));

			_headPart = CreaturePart.CreateForTest(
				creature: _creature,
				bodyPartType: BodyPartType.Head,
				damageModifier: 3,
				hitPenalty: 6,
				minToHit: 1,
				maxToHit: 3);
			_torsoPart = CreaturePart.CreateForTest(
				creature: _creature,
				bodyPartType: BodyPartType.Torso,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 4,
				maxToHit: 10);
			_creature.CreatureParts.AddRange(new List<CreaturePart> { _headPart, _torsoPart });

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_instance,
				_bodyTemplate,
				_condition,
				_creatureTemplate,
				_ability,
				_creature,
				_head1Crit,
				_head2Crit));
		}

		/// <summary>
		/// Тест метода Handle - атака монстра
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_MonsterAttack_ShouldReturnUnit()
		{
			var request = new MonsterAttackCommand(
				battleId: _instance.Id,
				id: _creature.Id,
				abilityId: _ability.Id,
				targetCreatureId: _creature.Id,
				creaturePartId: _headPart.Id,
				defenseValue: 1,
				specialToHit: 0,
				specialToDamage: 0);

			var newHandler = new MonsterAttackHandler(_dbContext, AuthorizationService.Object, RollService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var message = result.Message;
			Assert.IsNotNull(message);
			Assert.IsTrue(message.Contains("Попадание"));
		}

		/// <summary>
		/// Тест метода Handle - атака монстра
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_MonsterAttackСrit_ShouldReturnUnit()
		{
			var request = new MonsterAttackCommand(
				battleId: _instance.Id,
				id: _creature.Id,
				abilityId: _ability.Id,
				targetCreatureId: _creature.Id,
				creaturePartId: _headPart.Id,
				defenseValue: 1,
				specialToHit: 3,
				specialToDamage: 0);

			var newHandler = new MonsterAttackHandler(_dbContext, AuthorizationService.Object, RollService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var message = result.Message;
			Assert.IsNotNull(message);
			Assert.IsTrue(message.Contains("повреждение"));
		}
	}
}
