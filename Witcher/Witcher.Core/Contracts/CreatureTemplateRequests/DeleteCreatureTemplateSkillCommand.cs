using Witcher.Core.Abstractions;
using System;

namespace Witcher.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Команда удаления навыка шаблона существа
	/// </summary>
	public class DeleteCreatureTemplateSkillCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди шаблона существ
		/// </summary>
		public Guid CreatureTemplateId { get; set; }

		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
