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

		public string Name { get; private set; }

		public Skill AttackSkill { get; private set; }

		public int AttacksPerTurn { get; private set; }

		public int DamageDiceCount { get; private set; }

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
			DamageModifier = damageModifier;
			DamageType = damageType;
		}

		public static Ability ForCreatureTemplate(
			long creatureTemplateId,
			string name,
			Skill attackSkill,
			int attacksPerTurn,
			int damageDiceCount,
			int damageModifier,
			DamageType damageType)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(creatureTemplateId, nameof(creatureTemplateId));

			return new Ability(name, attackSkill, attacksPerTurn, damageDiceCount, damageModifier, damageType)
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
			int damageModifier,
			DamageType damageType)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(characterId, nameof(characterId));

			return new Ability(name, attackSkill, attacksPerTurn, damageDiceCount, damageModifier, damageType)
			{
				CharacterId = characterId,
			};
		}

		public void ChangeAbility(
			string name,
			Skill attackSkill,
			int attacksPerTurn,
			int damageDiceCount,
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
			DamageModifier = damageModifier;
			DamageType = damageType;
		}
	}
}
