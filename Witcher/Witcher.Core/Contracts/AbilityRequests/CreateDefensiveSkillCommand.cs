using Witcher.Core.Abstractions;
using Witcher.Core.Exceptions.RequestExceptions;
using System;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Contracts.AbilityRequests
{
	/// <summary>
	/// Команда добавления защитного навыка для способности
	/// </summary>
	public class CreateDefensiveSkillCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди способности
		/// </summary>
		public Guid AbilityId { get; set; }

		/// <summary>
		/// Навык
		/// </summary>
		public Skill Skill { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			if (!Enum.IsDefined(Skill))
				throw new RequestFieldIncorrectDataException<CreateDefensiveSkillCommand>(nameof(Skill));
		}
	}
}
