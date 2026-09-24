using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.UnitTest.TestSupport
{
	/// <summary>Минимальные валидные фикстуры сущностей для доменных тестов — без EF/БД.</summary>
	internal static class TestBuilders
	{
		public static Character Character(long userId = 1, long gameId = 1, string name = "Hero", int hp = 10, int sta = 10, int stat = 8)
			=> new(userId, gameId, name, hp, sta, stat, stat, stat, stat, stat, stat, stat);

		public static BattleCharacter BattleCharacter(long battleId, Character character)
			=> new(battleId, character);

		public static BodyTemplate BodyTemplate(long gameId = 1, string name = "Human")
			=> new(gameId, name, null);

		public static CreatureTemplate CreatureTemplate(BodyTemplate bodyTemplate, long gameId = 1, string name = "Wolf", int hp = 10, int sta = 10, int stat = 8)
			=> new(gameId, bodyTemplate, CreatureType.Beast, name, null, hp, sta, stat, stat, stat, stat, stat, stat, stat, stat, stat);

		public static Creature Creature(long battleId, CreatureTemplate template, string? nameOverride = null)
			=> new(battleId, template, nameOverride);

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

		public static ItemTemplate ArmorTemplate(long gameId = 1, string name = "Leather Armor")
			=> ItemTemplate.CreateNonWeapon(gameId, name, null, ItemType.Armor, weight: 5, cost: 10);

		public static Item Item(long characterId, ItemTemplate template)
			=> new(characterId, template);
	}
}
