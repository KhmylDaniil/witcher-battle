using Microsoft.VisualStudio.TestTools.UnitTesting;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Entities.Effects;
using Witcher.Core.Requests.RunBattleRequests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.UnitTest.Core.RunBattleRequests
{
	[TestClass]
	public class AttackWithWeaponHandlerTest : UnitTestBase
	{
		private readonly IAppDbContext _dbContext;
		private readonly Game _game;
		private readonly Battle _battle;
		private readonly CreaturePart _headPart;
		private readonly CreaturePart _torsoPart;
		private readonly CreaturePart _leftArmPart;
		private readonly CreaturePart _rightArmPart;
		private readonly CreaturePart _leftLegPart;
		private readonly CreaturePart _rightLegPart;
		private readonly CreatureTemplate _creatureTemplate;
		private readonly WeaponTemplate _weaponTemplate;
		private readonly Creature _creature;
		private readonly Character _character;
		private readonly Weapon _weapon;

		/// <summary>
		/// Тест для <see cref="CreatureAttackHandler"/>
		/// </summary>
		public AttackWithWeaponHandlerTest() : base()
		{
			_game = Game.CreateForTest();
			_battle = Battle.CreateForTest(game: _game);

			_creatureTemplate = CreatureTemplate.CreateForTest(
				game: _game,
				bodyTemplate: BodyTemplate.CreateForTest(game: _game),
				creatureType: CreatureType.Specter,
				speed: 4,
				body: 8);

			_weaponTemplate = WeaponTemplate.CreateForTest(
				game: _game,
				name: "sword",
				attackSkill: Skill.Melee,
				attackDiceQuantity: 2,
				damageModifier: 2,
				accuracy: 1);
			_weaponTemplate.AppliedConditions.Add(new AppliedCondition(Condition.Bleed, 100));

			_game.ItemTemplates.Add(_weaponTemplate);

			_creature = Creature.CreateForTest(
				battle: _battle,
				creatureTemlpate: _creatureTemplate,
				creatureType: CreatureType.Specter,
				@int: 10,
				@ref: 6,
				dex: 6,
				speed: 4,
				maxSpeed: 4,
				hp: 50);

			_character = Character.CreateForTest(game: _game, battle: _battle);

			_weapon = (Weapon)Item.CreateItem(_character, _weaponTemplate, 1);
			_character.Items.Add(_weapon);
			_weapon.IsEquipped = true;

			_creature.CreatureSkills.Add(CreatureSkill.CreateForTest(
				creature: _creature,
				skill: Skill.Melee,
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
				_battle,
				_creatureTemplate,
				_weaponTemplate,
				_weapon,
				_character,
				_creature));
		}

		[TestMethod]
		public async Task Handle_Attack_ShouldReturnUnit()
		{
			var request = new AttackCommand()
			{
				BattleId = _battle.Id,
				Id = _character.Id,
				TargetId = _creature.Id,
				AttackFormulaId = _weaponTemplate.Id,
				CreaturePartId = _torsoPart.Id,
				SpecialToHit = 3,
				AttackType = AttackType.Weapon
			};

			var newHandler = new AttackHandler(_dbContext, AuthorizationService.Object, RollService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var message = _dbContext.Battles.FirstOrDefault(x => x.Id == _battle.Id).BattleLog;
			Assert.IsNotNull(message);
			Assert.IsTrue(message.Contains("повреждена"));
			Assert.IsTrue(message.Contains(CritNames.GetConditionFullName(Condition.Bleed)));

			var monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster);
			Assert.IsTrue(monster.HP < 50);
			var torsoPart = monster.CreatureParts.FirstOrDefault(x => x.BodyPartType == BodyPartType.Torso);
			Assert.IsNotNull(torsoPart);
			Assert.AreEqual(2, torsoPart.CurrentArmor);
		}

		[TestMethod]
		public async Task Handle_CritAttack()
		{
			var request = new AttackCommand()
			{
				BattleId = _battle.Id,
				Id = _character.Id,
				TargetId = _creature.Id,
				AttackFormulaId = _weaponTemplate.Id,
				CreaturePartId = _leftArmPart.Id,
				SpecialToHit = 25,
				AttackType = AttackType.Weapon
			};

			var newHandler = new AttackHandler(_dbContext, AuthorizationService.Object, RollService.Object);

			var result = await newHandler.Handle(request, default);

			Assert.IsNotNull(result);

			var message = _dbContext.Battles.FirstOrDefault(x => x.Id == _battle.Id).BattleLog;
			Assert.IsNotNull(message);

			var monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster);
			var crit = monster.Effects.FirstOrDefault();
			Assert.IsNotNull(crit);
			Assert.IsTrue(crit is SimpleArmCritEffect);


			var secondRequest = new AttackCommand()
			{
				BattleId = _battle.Id,
				Id = _character.Id,
				TargetId = _creature.Id,
				AttackFormulaId = _weaponTemplate.Id,
				CreaturePartId = _leftArmPart.Id,
				SpecialToHit = 22,
				AttackType = AttackType.Weapon
			};


			result = await newHandler.Handle(secondRequest, default);

			Assert.IsNotNull(result);

			message = _dbContext.Battles.FirstOrDefault(x => x.Id == _battle.Id).BattleLog;
			Assert.IsNotNull(message);

			monster = _dbContext.Creatures.FirstOrDefault(x => x.Id == _creature.Id);
			Assert.IsNotNull(monster);
			var newCrit = monster.Effects.FirstOrDefault(x => x is SimpleArmCritEffect);
			Assert.IsTrue(monster.Effects.Count(x => x is SimpleArmCritEffect) == 1);
			Assert.AreNotEqual(newCrit.Id, crit.Id);
		}
	}
}
