using FluentAssertions;
using Moq;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Models;
using Wastelands.Service.Application.UnitTest.TestSupport;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.UnitTest.Services
{
	[TestClass]
	public class BattleCombatServiceDefenseTests
	{
		private const long BattleId = 1;
		private const long AttackerId = 10;
		private const long DefenderId = 11;

		private static (Battle Battle, Character DefenderCharacter, Item? Weapon) CreateScenario(bool equipMeleeWeapon)
		{
			var battle = new Battle(1, "Battle").WithId(BattleId);
			var defenderCharacter = TestBuilders.Character(name: "Defender").WithId(DefenderId);

			Item? weapon = null;
			if (equipMeleeWeapon)
			{
				var template = TestBuilders.MeleeWeaponTemplate(attackSkill: Skill.Sword, durability: 5);
				weapon = TestBuilders.Item(DefenderId, template).WithId(50);
				weapon.Equip();
				defenderCharacter.Items.Add(weapon);
			}

			// ConfirmDefenderAsync проверяет Condition.Stun защитника через BattleParticipants.HasCondition,
			// которому нужен зарегистрированный в battle.Characters BattleCharacter (иначе NotFoundException).
			var defenderBattleCharacter = TestBuilders.BattleCharacter(BattleId, defenderCharacter);
			defenderBattleCharacter.Character = defenderCharacter;
			battle.Characters.Add(defenderBattleCharacter);

			battle.StartAttack(new BattleAttack(BattleId, ParticipantKind.Character, AttackerId, abilityId: 1, attacksAllowed: 1, ParticipantKind.Character, DefenderId, isBonusAction: false));

			return (battle, defenderCharacter, weapon);
		}

		[TestMethod]
		public async Task SetDefenderChoiceAsync_Parry_NoEquippedMeleeWeapon_Throws()
		{
			var (battle, defenderCharacter, _) = CreateScenario(equipMeleeWeapon: false);
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			fixture.SetContext(ParticipantKind.Character, DefenderId, TestBuilders.Context(character: defenderCharacter));
			var service = fixture.BuildService();

			var act = () => service.SetDefenderChoiceAsync(new() { BattleId = BattleId, IsParry = true, DefenseRoll = 5 });

			(await act.Should().ThrowAsync<InvalidArgumentException>())
				.Which.ErrorCode.Should().Be(ErrorCode.ParryNotAvailable);
		}

		[TestMethod]
		public async Task SetDefenderChoiceAsync_Parry_WithEquippedMeleeWeapon_UsesWeaponSkillAndSetsIsParry()
		{
			var (battle, defenderCharacter, _) = CreateScenario(equipMeleeWeapon: true);
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			fixture.SetContext(ParticipantKind.Character, DefenderId, TestBuilders.Context(character: defenderCharacter));
			var service = fixture.BuildService();

			await service.SetDefenderChoiceAsync(new() { BattleId = BattleId, IsParry = true, DefenseRoll = 5 });

			battle.Attack!.DefensiveSkill.Should().Be(Skill.Sword);
			battle.Attack!.IsParry.Should().BeTrue();
		}

		[TestMethod]
		public async Task SetDefenderChoiceAsync_NonParry_SkillNotOffered_Throws()
		{
			var (battle, defenderCharacter, _) = CreateScenario(equipMeleeWeapon: false);
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			fixture.SetContext(ParticipantKind.Character, DefenderId, TestBuilders.Context(character: defenderCharacter));
			fixture.HitResolver.Setup(h => h.GetAvailableDefensiveSkillsAsync(battle, battle.Attack!)).ReturnsAsync([Skill.Dodge]);
			var service = fixture.BuildService();

			var act = () => service.SetDefenderChoiceAsync(new() { BattleId = BattleId, DefensiveSkill = Skill.Sword, DefenseRoll = 5 });

			(await act.Should().ThrowAsync<InvalidArgumentException>())
				.Which.ErrorCode.Should().Be(ErrorCode.InvalidDefensiveSkillChoice);
		}

		[TestMethod]
		public async Task SetDefenderChoiceAsync_NonParry_SkillOffered_SetsChoice()
		{
			var (battle, defenderCharacter, _) = CreateScenario(equipMeleeWeapon: false);
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			fixture.SetContext(ParticipantKind.Character, DefenderId, TestBuilders.Context(character: defenderCharacter));
			fixture.HitResolver.Setup(h => h.GetAvailableDefensiveSkillsAsync(battle, battle.Attack!)).ReturnsAsync([Skill.Dodge, Skill.Acrobatics]);
			var service = fixture.BuildService();

			await service.SetDefenderChoiceAsync(new() { BattleId = BattleId, DefensiveSkill = Skill.Acrobatics, DefenseRoll = 4 });

			battle.Attack!.DefensiveSkill.Should().Be(Skill.Acrobatics);
			battle.Attack!.IsParry.Should().BeFalse();
		}

		[TestMethod]
		public async Task ConfirmDefenderAsync_BlockingWeaponSkillChosen_WearsWeaponAndSavesCharacter()
		{
			var (battle, defenderCharacter, weapon) = CreateScenario(equipMeleeWeapon: true);
			battle.Attack!.SetDefenderChoice(Skill.Sword, 5); // тот же навык, что у экипированного оружия -> блокирование
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			fixture.SetContext(ParticipantKind.Character, DefenderId, TestBuilders.Context(character: defenderCharacter));
			var service = fixture.BuildService();

			await service.ConfirmDefenderAsync(BattleId);

			weapon!.Durability.Should().Be(4);
			fixture.CharacterRepository.Verify(r => r.UpdateAsync(defenderCharacter), Times.Once);
		}

		[TestMethod]
		public async Task ConfirmDefenderAsync_DodgeChosen_DoesNotWearWeapon()
		{
			var (battle, defenderCharacter, weapon) = CreateScenario(equipMeleeWeapon: true);
			battle.Attack!.SetDefenderChoice(Skill.Dodge, 5);
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			fixture.SetContext(ParticipantKind.Character, DefenderId, TestBuilders.Context(character: defenderCharacter));
			var service = fixture.BuildService();

			await service.ConfirmDefenderAsync(BattleId);

			weapon!.Durability.Should().Be(5);
			fixture.CharacterRepository.Verify(r => r.UpdateAsync(It.IsAny<Character>()), Times.Never);
		}

		[TestMethod]
		public async Task ConfirmDefenderAsync_Parry_DoesNotWearWeaponEvenThoughSkillMatches()
		{
			var (battle, defenderCharacter, weapon) = CreateScenario(equipMeleeWeapon: true);
			battle.Attack!.SetDefenderChoice(Skill.Sword, 5, isParry: true);
			var fixture = new BattleCombatServiceFixture();
			fixture.SetBattle(BattleId, battle);
			fixture.SetContext(ParticipantKind.Character, DefenderId, TestBuilders.Context(character: defenderCharacter));
			var service = fixture.BuildService();

			await service.ConfirmDefenderAsync(BattleId);

			weapon!.Durability.Should().Be(5);
		}
	}
}
