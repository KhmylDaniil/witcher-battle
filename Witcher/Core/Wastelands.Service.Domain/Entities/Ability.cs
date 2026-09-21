using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	/// <summary>
	/// Способность — атакующее умение шаблона существа или персонажа игрока. Владелец ровно один:
	/// <see cref="CreatureTemplateId"/> для существ, <see cref="CharacterId"/> для персонажей — какой из
	/// двух задан, определяет использованный при создании фабричный метод.
	/// </summary>
	public class Ability : Entity
	{
		public long? CreatureTemplateId { get; private set; }

		public long? CharacterId { get; private set; }

		/// <summary>
		/// Заполнено только для способностей, сгенерированных экипировкой оружия (см.
		/// Ability.ForEquippedWeapon) — по этому полю снятие оружия находит и удаляет ровно те
		/// способности, что оно породило. Обычная колонка, без FK/cascade — соответствие
		/// поддерживается явным кодом в CharacterItemService, а не EF-каскадом.
		/// </summary>
		public long? EquippedItemId { get; private set; }

		public string Name { get; private set; }

		public Skill AttackSkill { get; private set; }

		public int AttacksPerTurn { get; private set; }

		public int DamageDiceCount { get; private set; }

		/// <summary>
		/// Добавляется к суммарному броску на попадание (см. BattleCombatCalculator.ResolveHit).
		/// </summary>
		public int AttackModifier { get; private set; }

		public int DamageModifier { get; private set; }

		public DamageType DamageType { get; private set; }

		public List<AbilityAppliedCondition> AppliedConditions { get; private set; } = [];

		public List<AbilityDefensiveSkill> DefensiveSkills { get; private set; } = [];

		private Ability()
		{
		}

		private Ability(
			string name,
			Skill attackSkill,
			int attacksPerTurn,
			int damageDiceCount,
			int attackModifier,
			int damageModifier,
			DamageType damageType)
		{
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(attacksPerTurn, nameof(attacksPerTurn));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(damageDiceCount, nameof(damageDiceCount));

			Name = name;
			AttackSkill = attackSkill;
			AttacksPerTurn = attacksPerTurn;
			DamageDiceCount = damageDiceCount;
			AttackModifier = attackModifier;
			DamageModifier = damageModifier;
			DamageType = damageType;
		}

		public static Ability ForCreatureTemplate(
			long creatureTemplateId,
			string name,
			Skill attackSkill,
			int attacksPerTurn,
			int damageDiceCount,
			int attackModifier,
			int damageModifier,
			DamageType damageType)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(creatureTemplateId, nameof(creatureTemplateId));

			return new Ability(name, attackSkill, attacksPerTurn, damageDiceCount, attackModifier, damageModifier, damageType)
			{
				CreatureTemplateId = creatureTemplateId,
			};
		}

		public static Ability ForCharacter(
			long characterId,
			string name,
			Skill attackSkill,
			int attacksPerTurn,
			int damageDiceCount,
			int attackModifier,
			int damageModifier,
			DamageType damageType)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(characterId, nameof(characterId));

			return new Ability(name, attackSkill, attacksPerTurn, damageDiceCount, attackModifier, damageModifier, damageType)
			{
				CharacterId = characterId,
			};
		}

		/// <summary>
		/// Способность, сгенерированная экипировкой оружия (см. CharacterItemService.EquipAsync) —
		/// name/appliedConditions приходят от ItemTemplate/Item, а не вводятся пользователем вручную,
		/// поэтому в отличие от ForCharacter сразу принимает и накладываемые состояния.
		/// </summary>
		public static Ability ForEquippedWeapon(
			long characterId,
			long equippedItemId,
			string name,
			Skill attackSkill,
			int attacksPerTurn,
			int damageDiceCount,
			int attackModifier,
			int damageModifier,
			DamageType damageType,
			IEnumerable<(Condition Condition, int ApplyChance)> appliedConditions)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(characterId, nameof(characterId));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(equippedItemId, nameof(equippedItemId));

			var ability = new Ability(name, attackSkill, attacksPerTurn, damageDiceCount, attackModifier, damageModifier, damageType)
			{
				CharacterId = characterId,
				EquippedItemId = equippedItemId,
			};

			foreach (var (condition, applyChance) in appliedConditions)
			{
				ability.AppliedConditions.Add(new AbilityAppliedCondition(condition, applyChance));
			}

			return ability;
		}

		public void ChangeAbility(
			string name,
			Skill attackSkill,
			int attacksPerTurn,
			int damageDiceCount,
			int attackModifier,
			int damageModifier,
			DamageType damageType)
		{
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(attacksPerTurn, nameof(attacksPerTurn));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(damageDiceCount, nameof(damageDiceCount));

			Name = name;
			AttackSkill = attackSkill;
			AttacksPerTurn = attacksPerTurn;
			DamageDiceCount = damageDiceCount;
			AttackModifier = attackModifier;
			DamageModifier = damageModifier;
			DamageType = damageType;
		}
	}
}
