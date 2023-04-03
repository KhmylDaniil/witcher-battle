using System;
using Witcher.Core.Abstractions;

namespace Witcher.Core.Contracts.CharacterRequests
{
	public class GetCharacterByIdCommand : IValidatableCommand<GetCharacterByIdResponse>
	{
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
