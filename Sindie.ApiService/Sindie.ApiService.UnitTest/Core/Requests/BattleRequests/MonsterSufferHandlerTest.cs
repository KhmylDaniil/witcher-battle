using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BattleRequests.MonsterSuffer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sindie.ApiService.UnitTest.Core.Requests.BattleRequests
{
	/// <summary>
	/// Тест для <see cref="MonsterSufferHandler"/>
	/// </summary>
	[TestClass]
	public class MonsterSufferHandlerTest: UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Battle _instance;
		private readonly BodyPartType _torso;
		private readonly BodyPartType _head;
		private readonly BodyTemplate _bodyTemplate;
		private readonly CreaturePart _headPart;
		private readonly CreaturePart _torsoPart;
		private readonly Condition _condition;
		private readonly Skill _skill;
		private readonly Skill _skillBleedingWound;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly Ability _ability;
		private readonly Creature _creature;
		private readonly CreatureType _creatureType;
		private readonly DamageType _damageType;

		/// <summary>
		/// Тест для <see cref="MonsterSufferHandler"/>
		/// </summary>
		public MonsterSufferHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_creatureType = CreatureType.CreateForTest(CreatureTypes.SpecterId, CreatureTypes.SpecterName);
			_instance = Battle.CreateForTest(game: _game);
			_torso = BodyPartType.CreateForTest(BodyPartTypes.TorsoId, BodyPartTypes.TorsoName);
			_head = BodyPartType.CreateForTest(BodyPartTypes.HeadId, BodyPartTypes.HeadName);
			_condition = Condition.CreateForTest(id: Conditions.BleedingWoundId, name: Conditions.BleedingWoundName);
			_damageType = DamageType.CreateForTest();

			_skill = Skill.CreateForTest();
			_skillBleedingWound = Skill.CreateForTest(id: Skills.BleedingWoundId, statName: "Int", name: Skills.BleedingWoundName);
			_bodyTemplate = BodyTemplate.CreateForTest(game: _game);

			_creatureTemplate = CreatureTemplate.CreateForTest(
				game: _game,
				bodyTemplate: _bodyTemplate,
				creatureType: _creatureType);

			_ability = Ability.CreateForTest(
				game: _game,
				name: "attack",
				attackDiceQuantity: 1,
				damageModifier: 1,
				attackSpeed: 1,
				accuracy: 1,
				attackSkill: _skill,
				damageType: _damageType,
				defensiveSkills: new List<Skill> { _skill });
			_ability.AppliedConditions.Add(AppliedCondition.CreateAppliedCondition(_ability, _condition, 100));

			_creature = Creature.CreateForTest(
				battle: _instance,
				creatureTemlpate: _creatureTemplate,
				creatureType: _creatureType,
				@int: 10);

			_creature.Abilities.Add(_ability);

			_creature.CreatureSkills.Add(CreatureSkill.CreateForTest(
				creature: _creature,
				skill: _skill,
				value: 10));

			_creature.CreatureSkills.Add(CreatureSkill.CreateForTest(
				creature: _creature,
				skill: _skillBleedingWound,
				statName: "Int",
				value: 10));

			_headPart = CreaturePart.CreateForTest(
				creature: _creature,
				bodyPartType: _head,
				name: _head.Name,
				damageModifier: 3,
				hitPenalty: 6,
				minToHit: 1,
				maxToHit: 3);
			_torsoPart = CreaturePart.CreateForTest(
				creature: _creature,
				bodyPartType: _torso,
				name: _torso.Name,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 4,
				maxToHit: 10,
				currentArmor: 3,
				startingArmor: 3);
			_creature.CreatureParts.AddRange(new List<CreaturePart> { _headPart, _torsoPart });

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_instance,
				_bodyTemplate,
				_torso,
				_head,
				_condition,
				_skill,
				_skillBleedingWound,
				_creatureTemplate,
				_ability,
				_creature,
				_creatureType,
				_damageType));
		}

		/// <summary>
		/// Тест метода Handle - получение монстром урона
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_MonsterSuffer_ShouldReturnUnit()
		{
			var request = new MonsterSufferCommand(
				battleId: _instance.Id,
				attackerId: _creature.Id,
				targetId: _creature.Id,
				abilityId: _ability.Id,
				damageValue: 10,
				successValue: 1,
				creaturePartId: _torsoPart.Id);

			var newHandler = new MonsterSufferHandler(_dbContext, AuthorizationService.Object, RollService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var message = result.Message;
			Assert.IsNotNull(message);
			Assert.IsTrue(message.Contains("повреждена"));
			Assert.IsTrue(message.Contains("7 урона"));
			Assert.IsTrue(message.Contains(Conditions.BleedingWoundName));

			var monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster);
			Assert.AreEqual(monster.HP, 3);
			var torsoPart = monster.CreatureParts.FirstOrDefault(x => x.BodyPartTypeId == _torso.Id);
			Assert.IsNotNull(torsoPart);
			Assert.AreEqual(torsoPart.CurrentArmor, 2);
		}

		/// <summary>
		/// Тест метода Handle - атака монстра
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_MonsterAttackСrit_ShouldReturnUnit()
		{
			var request = new MonsterSufferCommand(
				battleId: _instance.Id,
				attackerId: _creature.Id,
				targetId: _creature.Id,
				abilityId: _ability.Id,
				damageValue: 10,
				successValue: 1,
				creaturePartId: _headPart.Id);

			var newHandler = new MonsterSufferHandler(_dbContext, AuthorizationService.Object, RollService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var message = result.Message;
			Assert.IsNotNull(message);
			Assert.IsTrue(message.Contains("погибает"));
			var monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNull(monster);
		}
	}
}
