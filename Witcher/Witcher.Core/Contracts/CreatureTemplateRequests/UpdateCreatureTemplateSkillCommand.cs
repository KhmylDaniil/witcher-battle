using Witcher.Core.Abstractions;
using Witcher.Core.Exceptions.RequestExceptions;
using System;

namespace Witcher.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Команда на создание/изменение навыка шаблона персонажа
	/// </summary>
	public class UpdateCreatureTemplateSkillCommand : UpdateCreatureTemplateRequestSkill, IValidatableCommand
	{
		/// <summary>
		/// Айди шаблона существа
		/// </summary>
		public Guid CreatureTemplateId { get; set; }

		public void Validate()
		{
			if (!Enum.IsDefined(Skill))
				throw new RequestFieldIncorrectDataException<UpdateCreatureTemplateSkillCommand>(nameof(Skill));

			if (Value < 1 || Value > 10)
				throw new RequestFieldIncorrectDataException<UpdateCreatureTemplateSkillCommand>(nameof(Value), "Значение должно быть в диапазоне от 1 до 10");
		}
	}
}
