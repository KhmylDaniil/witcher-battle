using Wastelands.Core.Contracts.Constants;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Services
{
	/// <summary>
	/// Общая логика поиска/валидации способностей — способности живут как List&lt;Ability&gt; и у
	/// CreatureTemplate, и у Character, поэтому CreatureTemplateService и CharacterService переиспользуют
	/// эти хелперы вместо двух копий одной и той же проверки.
	/// </summary>
	internal static class AbilityHelpers
	{
		public static Ability GetAbility(List<Ability> abilities, long abilityId)
		{
			var ability = abilities.FirstOrDefault(x => x.Id == abilityId);

			NotFoundException.ThrowIfNull(
				ability, ErrorCode.AbilityNotFound, nameof(Ability), nameof(Ability.Id), abilityId.ToString());

			return ability;
		}

		public static void ThrowIfConditionDuplicate(Ability ability, Condition condition, long? excludeConditionId)
		{
			if (ability.AppliedConditions.Any(x => x.Condition == condition && x.Id != excludeConditionId))
			{
				throw new InvalidArgumentException(
					ErrorCode.AbilityConditionAlreadyExisted,
					string.Format(ExceptionMessages.ValueMustBeUnique, nameof(condition)));
			}
		}
	}
}
