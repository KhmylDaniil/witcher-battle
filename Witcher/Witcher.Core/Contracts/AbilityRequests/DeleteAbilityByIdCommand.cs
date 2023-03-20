using Witcher.Core.Abstractions;
using System;

namespace Witcher.Core.Contracts.AbilityRequests
{
	/// <summary>
	/// Команда на удаление способности по айди
	/// </summary>
	public sealed class DeleteAbilityByIdCommand : IValidatableCommand
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
