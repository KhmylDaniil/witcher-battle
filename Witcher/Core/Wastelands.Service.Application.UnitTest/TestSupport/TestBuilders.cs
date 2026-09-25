using Wastelands.Service.Application.Models;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.UnitTest.TestSupport
{
	/// <summary>Минимальные валидные фикстуры сущностей для тестов слоя Application — без EF/БД.</summary>
	internal static class TestBuilders
	{
		public static Character Character(long userId = 1, long gameId = 1, string name = "Hero", int hp = 10, int sta = 10, int stat = 8)
			=> new(userId, gameId, name, hp, sta, stat, stat, stat, stat, stat, stat, stat, movement: 5);

		public static BattleCharacter BattleCharacter(long battleId, Character character)
			=> new(battleId, character);

		/// <summary>Шаблон существа с частями тела по умолчанию (Голова/Торс/руки/ноги — см. DefaultHumanBodyTemplatePartsDraft). Торс — Parts[1].</summary>
		public static CreatureTemplate CreatureTemplate(long gameId = 1, string name = "Wolf", int hp = 10, int sta = 10, int stat = 8)
		{
			var bodyTemplate = new BodyTemplate(gameId, "Body", null);
			return new CreatureTemplate(gameId, bodyTemplate, CreatureType.Beast, name, null, hp, sta, stat, stat, stat, stat, stat, stat, stat, stat, stat, movement: 5);
		}

		public static Creature Creature(long battleId, CreatureTemplate template, string? nameOverride = null)
			=> new(battleId, template, nameOverride);

		public static Ability CharacterAbility(
			long characterId = 1,
			string name = "Punch",
			Skill attackSkill = Skill.Brawl,
			int attacksPerTurn = 1,
			int damageDiceCount = 2,
			int attackModifier = 0,
			int damageModifier = 0,
			DamageType damageType = DamageType.Bludgeoning)
			=> Ability.ForCharacter(characterId, name, attackSkill, attacksPerTurn, damageDiceCount, attackModifier, damageModifier, damageType);

		public static Ability CreatureAbility(
			long creatureTemplateId = 1,
			string name = "Bite",
			Skill attackSkill = Skill.Brawl,
			int attacksPerTurn = 1,
			int damageDiceCount = 2,
			int attackModifier = 0,
			int damageModifier = 0,
			DamageType damageType = DamageType.Piercing)
			=> Ability.ForCreatureTemplate(creatureTemplateId, name, attackSkill, attacksPerTurn, damageDiceCount, attackModifier, damageModifier, damageType);

		public static ItemTemplate ArmorTemplate(long gameId = 1, string name = "Armor")
			=> ItemTemplate.CreateNonWeapon(gameId, name, null, ItemType.Armor, weight: 5, cost: 10);

		public static ItemTemplate MeleeWeaponTemplate(
			long gameId = 1,
			string name = "Sword",
			Skill attackSkill = Skill.Sword,
			bool isMultiAttack = false,
			int damageDiceCount = 2,
			int damageModifier = 0,
			int durability = 5)
			=> ItemTemplate.CreateWeapon(
				gameId, name, null, weight: 2, cost: 10, attackSkill, isMultiAttack, damageDiceCount,
				attackModifier: 0, damageModifier, DamageType.Slashing, WeaponKind.Melee, attackRange: 1, handsRequired: 1, durability);

		public static Item Item(long characterId, ItemTemplate template)
			=> new(characterId, template);

		/// <summary>getSkillValue по умолчанию — постоянная функция, если явно не задана.</summary>
		public static ParticipantCombatContext Context(
			List<Ability>? abilities = null,
			Func<Skill, int>? getSkillValue = null,
			CreatureTemplate? template = null,
			Creature? creature = null,
			Character? character = null)
			=> new()
			{
				Abilities = abilities ?? [],
				GetSkillValue = getSkillValue ?? (_ => 5),
				Template = template,
				Creature = creature,
				Character = character,
			};
	}
}
