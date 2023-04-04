using System;
using Witcher.Core.Abstractions;

namespace Witcher.Core.Contracts.CharacterRequests
{
	public class DeleteCharacterCommand : IValidatableCommand
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
