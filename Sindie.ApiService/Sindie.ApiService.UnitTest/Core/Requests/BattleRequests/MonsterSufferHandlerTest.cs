﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BattleRequests.MonsterSuffer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Sindie.ApiService.Core.BaseData.Enums;

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
		private readonly BodyTemplate _bodyTemplate;
		private readonly CreaturePart _headPart;
		private readonly CreaturePart _torsoPart;
		private readonly Condition _condition;

		private readonly CreatureTemplate _creatureTemplate;
		private readonly Ability _ability;
		private readonly Creature _creature;

		/// <summary>
		/// Тест для <see cref="MonsterSufferHandler"/>
		/// </summary>
		public MonsterSufferHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_instance = Battle.CreateForTest(game: _game);
			_condition = Condition.CreateForTest(id: Conditions.BleedingWoundId, name: Conditions.BleedingWoundName);

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
				creatureType: CreatureType.Specter,
				@int: 10,
				hp: 20);

			_creature.Abilities.Add(_ability);

			_creature.CreatureSkills.Add(CreatureSkill.CreateForTest(
				creature: _creature,
				skill: Skill.Melee,
				value: 10));

			_creature.CreatureSkills.Add(CreatureSkill.CreateForTest(
				creature: _creature,
				skill: Skill.BleedingWound,
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
				maxToHit: 10,
				currentArmor: 3,
				startingArmor: 3);
			_creature.CreatureParts.AddRange(new List<CreaturePart> { _headPart, _torsoPart });

			_dbContext = CreateInMemoryContext(x => x.AddRange(
				_game,
				_instance,
				_bodyTemplate,
				_condition,
				_creatureTemplate,
				_ability,
				_creature));
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
			Assert.AreEqual(monster.HP, 13);
			var torsoPart = monster.CreatureParts.FirstOrDefault(x => x.BodyPartType == BodyPartType.Torso);
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
				damageValue: 20,
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
