﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Entities.Effects;
using Sindie.ApiService.Core.Requests.BattleRequests.CreatureAttack;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.UnitTest.Core.Requests.BattleRequests
{
	/// <summary>
	/// Тест для <see cref="CreatureAttackHandler"/>
	/// </summary>
	[TestClass]
	public class CreatureAttackHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Battle _instance;
		private readonly CreaturePart _headPart;
		private readonly CreaturePart _torsoPart;
		private readonly CreaturePart _leftArmPart;
		private readonly CreaturePart _rightArmPart;
		private readonly CreaturePart _leftLegPart;
		private readonly CreaturePart _rightLegPart;
		private readonly Condition _bleedingWound;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly Ability _ability;
		private readonly Creature _creature;
		private readonly Condition _simpleLegCrit;
		private readonly Condition _simpleArmCrit;
		private readonly Condition _difficultLegCrit;
		private readonly Condition _deadlyLegCrit;

		/// <summary>
		/// Тест для <see cref="CreatureAttackHandler"/>
		/// </summary>
		public CreatureAttackHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_instance = Battle.CreateForTest(game: _game);

			_bleedingWound = Condition.CreateForTest(id: Conditions.BleedingWoundId, name: Conditions.BleedingWoundName);
			_simpleLegCrit = Condition.CreateForTest(id: Crit.SimpleLegId, name: Crit.SimpleLeg);
			_simpleArmCrit = Condition.CreateForTest(id: Crit.SimpleArmId, name: Crit.SimpleArm);
			_difficultLegCrit = Condition.CreateForTest(id: Crit.DifficultLegId, name: Crit.DifficultLeg);
			_deadlyLegCrit = Condition.CreateForTest(id: Crit.DeadlyLegId, name: Crit.DeadlyLeg);

			_creatureTemplate = CreatureTemplate.CreateForTest(
				game: _game,
				bodyTemplate: BodyTemplate.CreateForTest(game: _game),
				creatureType: CreatureType.Specter,
				speed: 4);

			_ability = Ability.CreateForTest(
				game: _game,
				name: "attack",
				attackDiceQuantity: 2,
				damageModifier: 2,
				attackSpeed: 1,
				accuracy: 1,
				defensiveSkills: new List<Skill> { Skill.Melee, Skill.Dodge, Skill.Athletics });
			_ability.AppliedConditions.Add(AppliedCondition.CreateAppliedCondition(_ability, _bleedingWound, 100));

			_creature = Creature.CreateForTest(
				battle: _instance,
				creatureTemlpate: _creatureTemplate,
				creatureType: CreatureType.Specter,
				@int: 10,
				@ref: 6,
				dex: 6,
				speed: 4,
				maxSpeed: 4,
				hp: 50);

			_creature.Abilities.Add(_ability);

			_creature.CreatureSkills.Add(CreatureSkill.CreateForTest(
				creature: _creature,
				skill: Skill.Melee,
				value: 10));

			_creature.CreatureSkills.Add(CreatureSkill.CreateForTest(
				creature: _creature,
				skill: Skill.BleedingWound,
				value: 10));

			_creature.CreatureSkills.Add(CreatureSkill.CreateForTest(
				creature: _creature,
				skill: Skill.Dodge,
				value: 6,
				maxValue: 6));

			_creature.CreatureSkills.Add(CreatureSkill.CreateForTest(
				creature: _creature,
				skill: Skill.Athletics,
				value: 5,
				maxValue: 5));

			_headPart = CreaturePart.CreateForTest(
				creature: _creature,
				bodyPartType: BodyPartType.Head,
				damageModifier: 3,
				hitPenalty: 6,
				minToHit: 1,
				maxToHit: 1);
			_torsoPart = CreaturePart.CreateForTest(
				creature: _creature,
				bodyPartType: BodyPartType.Torso,
				damageModifier: 1,
				hitPenalty: 1,
				minToHit: 2,
				maxToHit: 4,
				currentArmor: 3,
				startingArmor: 3);
			_leftArmPart = CreaturePart.CreateForTest(
				creature: _creature,
				bodyPartType: BodyPartType.Arm,
				damageModifier: 0.5,
				hitPenalty: 3,
				minToHit: 5,
				maxToHit: 5);
			_rightArmPart = CreaturePart.CreateForTest(
				creature: _creature,
				bodyPartType: BodyPartType.Arm,
				damageModifier: 0.5,
				hitPenalty: 3,
				minToHit: 6,
				maxToHit: 6);
			_leftLegPart = CreaturePart.CreateForTest(
				creature: _creature,
				bodyPartType: BodyPartType.Leg,
				damageModifier: 0.5,
				hitPenalty: 2,
				minToHit: 7,
				maxToHit: 8);
			_rightLegPart = CreaturePart.CreateForTest(
				creature: _creature,
				bodyPartType: BodyPartType.Leg,
				damageModifier: 0.5,
				hitPenalty: 2,
				minToHit: 9,
				maxToHit: 10);

			_creature.CreatureParts.AddRange(new List<CreaturePart> { _headPart, _torsoPart, _rightArmPart, _leftArmPart, _rightLegPart, _leftLegPart });

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_instance,
				_bleedingWound,
				_difficultLegCrit,
				_deadlyLegCrit,
				_simpleArmCrit,
				_simpleLegCrit,
				_creatureTemplate,
				_ability,
				_creature));
		}

		/// <summary>
		/// Тест метода Handle - получение монстром урона
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_CreatureAttack_ShouldReturnUnit()
		{
			var request = new CreatureAttackCommand(
				battleId: _instance.Id,
				attackerId: _creature.Id,
				targetCreatureId: _creature.Id,
				creaturePartId: _torsoPart.Id,
				specialToHit: 3);

			var newHandler = new CreatureAttackHandler(_dbContext, AuthorizationService.Object, RollService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var message = result.Message;
			Assert.IsNotNull(message);
			Assert.IsTrue(message.Contains("повреждена"));
			Assert.IsTrue(message.Contains(Conditions.BleedingWoundName));

			var monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster);
			Assert.IsTrue(monster.HP < 50);
			var torsoPart = monster.CreatureParts.FirstOrDefault(x => x.BodyPartType == BodyPartType.Torso);
			Assert.IsNotNull(torsoPart);
			Assert.AreEqual(torsoPart.CurrentArmor, 2);
		}

		[TestMethod]
		public async Task Handle_EffectIsSingleton_ShouldReturnUnit()
		{
			var request = new CreatureAttackCommand(
				battleId: _instance.Id,
				attackerId: _creature.Id,
				targetCreatureId: _creature.Id,
				specialToHit: 3);

			var newHandler = new CreatureAttackHandler(_dbContext, AuthorizationService.Object, RollService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var message = result.Message;
			Assert.IsNotNull(message);
			Assert.IsTrue(message.Contains(Conditions.BleedingWoundName));

			var monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster);
			var effect = monster.Effects.FirstOrDefault();
			Assert.IsNotNull(effect);
			Assert.IsTrue(effect is BleedingWoundEffect);

			var result2 = await newHandler.Handle(request, default);

			var message2 = result2.Message;
			Assert.IsNotNull(message2);
			Assert.IsTrue(!message2.Contains(Conditions.BleedingWoundName));

			var monster2 = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster2);
			var effect2 = monster.Effects.FirstOrDefault();
			Assert.IsNotNull(effect2);
			Assert.IsTrue(effect2 is BleedingWoundEffect);

			Assert.AreEqual(effect.Id, effect2.Id);
		}

		/// <summary>
		/// Тест метода Handle - атака монстра
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_MonsterIsDead_ShouldReturnUnit()
		{
			var request = new CreatureAttackCommand(
				battleId: _instance.Id,
				attackerId: _creature.Id,
				targetCreatureId: _creature.Id,
				specialToHit: 3,
				specialToDamage: 100);

			var newHandler = new CreatureAttackHandler(_dbContext, AuthorizationService.Object, RollService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var message = result.Message;
			Assert.IsNotNull(message);
			Assert.IsTrue(message.Contains("погибает"));
			var monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNull(monster);
		}

		/// <summary>
		/// Тест метода Handle - атака монстра
		/// </summary>
		/// <returns></returns>
		[TestMethod]
		public async Task Handle_SharedPenaltyCrits_ShouldReturnUnit()
		{
			var request = new CreatureAttackCommand(
				battleId: _instance.Id,
				attackerId: _creature.Id,
				targetCreatureId: _creature.Id,
				creaturePartId: _leftLegPart.Id,
				specialToHit: 8);

			var newHandler = new CreatureAttackHandler(_dbContext, AuthorizationService.Object, RollService.Object);

			//first attack
			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var message = result.Message;
			Assert.IsNotNull(message);
			Assert.IsTrue(message.Contains(Conditions.StunName));
			var monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster);
			var stun = monster.Effects.FirstOrDefault(x => x is StunEffect);
			Assert.IsNotNull(stun);
			
			var leftLegSimpleCrit = monster.Effects.FirstOrDefault(x => x is SimpleLegCritEffect) as ICrit;
			Assert.IsNotNull(leftLegSimpleCrit);
			Assert.AreEqual((int)leftLegSimpleCrit.Severity, 13);
			Assert.AreEqual(leftLegSimpleCrit.Severity, Enums.Severity.Unstabilizied | Enums.Severity.Simple);

			Assert.AreEqual(monster.Speed, 2);

			var dodge = monster.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Dodge);
			Assert.IsNotNull(dodge);
			Assert.AreEqual(dodge.SkillValue, 4);

			var athletics = monster.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Athletics);
			Assert.IsNotNull(athletics);
			Assert.AreEqual(athletics.SkillValue, 3);

			//second attack
			request = new CreatureAttackCommand(
				battleId: _instance.Id,
				attackerId: _creature.Id,
				targetCreatureId: _creature.Id,
				creaturePartId: _rightLegPart.Id,
				specialToHit: 3);

			result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			message = result.Message;
			Assert.IsNotNull(message);

			monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster);
			stun = monster.Effects.FirstOrDefault(x => x is StunEffect);
			Assert.IsNotNull(stun);

			var rightLegSimpleCrit = monster.Effects.FirstOrDefault(x => x is SimpleLegCritEffect crit && crit.CreaturePartId == _rightLegPart.Id);
			Assert.IsNotNull(rightLegSimpleCrit);

			Assert.AreEqual(monster.Speed, 2);

			dodge = monster.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Dodge);
			Assert.IsNotNull(dodge);
			Assert.AreEqual(dodge.SkillValue, 4);

			athletics = monster.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Athletics);
			Assert.IsNotNull(athletics);
			Assert.AreEqual(athletics.SkillValue, 3);

			//stabilize
			(leftLegSimpleCrit as SimpleLegCritEffect).Stabilize(monster);

			monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster);
			Assert.AreEqual(monster.Speed, 2);

			dodge = monster.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Dodge);
			Assert.IsNotNull(dodge);
			Assert.AreEqual(dodge.SkillValue, 4);

			athletics = monster.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Athletics);
			Assert.IsNotNull(athletics);
			Assert.AreEqual(athletics.SkillValue, 3);

			//third attack
			request = new CreatureAttackCommand(
				battleId: _instance.Id,
				attackerId: _creature.Id,
				targetCreatureId: _creature.Id,
				creaturePartId: _rightLegPart.Id,
				specialToHit: 8);

			result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			message = result.Message;
			Assert.IsNotNull(message);

			monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster);

			var rightLegDifficultCrit = monster.Effects.FirstOrDefault(x => x is DifficultLegCritEffect crit && crit.CreaturePartId == _rightLegPart.Id);
			Assert.IsNotNull(rightLegDifficultCrit);

			Assert.AreEqual(monster.Speed, 1);

			dodge = monster.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Dodge);
			Assert.IsNotNull(dodge);
			Assert.AreEqual(dodge.SkillValue, 1);

			athletics = monster.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Athletics);
			Assert.IsNotNull(athletics);
			Assert.AreEqual(athletics.SkillValue, 1);

			//dismember limb
			request = new CreatureAttackCommand(
				battleId: _instance.Id,
				attackerId: _creature.Id,
				targetCreatureId: _creature.Id,
				creaturePartId: _rightLegPart.Id,
				specialToHit: 17);

			result = await newHandler.Handle(request, default);
			message = result.Message;
			Assert.IsNotNull(message);

			monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster);

			var rightLegDeadlyCrit = monster.Effects.FirstOrDefault(x => x is DeadlyLegCritEffect crit && crit.CreaturePartId == _rightLegPart.Id);
			Assert.IsNotNull(rightLegDifficultCrit);

			Assert.AreEqual(monster.Speed, 1);

			dodge = monster.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Dodge);
			Assert.IsNotNull(dodge);
			Assert.AreEqual(dodge.SkillValue, 1);

			athletics = monster.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Athletics);
			Assert.IsNotNull(athletics);
			Assert.AreEqual(athletics.SkillValue, 1);
		}
	}
}
