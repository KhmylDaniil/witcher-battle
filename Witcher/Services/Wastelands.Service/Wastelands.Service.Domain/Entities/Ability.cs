using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	public class Ability : Entity
	{
		public long CreatureTemplateId { get; private set; }

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

		public Ability(
			long creatureTemplateId,
			string name,
			Skill attackSkill,
			int attacksPerTurn,
			int damageDiceCount,
			int damageModifier,
			DamageType damageType)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(creatureTemplateId, nameof(creatureTemplateId));
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(attacksPerTurn, nameof(attacksPerTurn));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(damageDiceCount, nameof(damageDiceCount));

			CreatureTemplateId = creatureTemplateId;
			Name = name;
			AttackSkill = attackSkill;
			AttacksPerTurn = attacksPerTurn;
			DamageDiceCount = damageDiceCount;
			DamageModifier = damageModifier;
			DamageType = damageType;
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
