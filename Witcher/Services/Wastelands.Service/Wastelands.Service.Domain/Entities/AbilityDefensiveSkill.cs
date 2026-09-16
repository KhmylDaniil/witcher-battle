using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	public class AbilityDefensiveSkill : Entity
	{
		public long AbilityId { get; private set; }

		public Skill Skill { get; private set; }

		private AbilityDefensiveSkill()
		{
		}

		public AbilityDefensiveSkill(long abilityId, Skill skill)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(abilityId, nameof(abilityId));

			AbilityId = abilityId;
			Skill = skill;
		}
	}
}
