using Witcher.Core.Abstractions;
using System;

namespace Witcher.Core.Contracts.BodyTemplateRequests
{
	/// <summary>
	/// Команда на удаление шаблона тела по айди
	/// </summary>
	public sealed class DeleteBodyTemplateByIdCommand : IValidatableCommand
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
