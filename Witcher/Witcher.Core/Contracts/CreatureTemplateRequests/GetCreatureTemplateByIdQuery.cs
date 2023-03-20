using Witcher.Core.Abstractions;
using System;

namespace Witcher.Core.Contracts.CreatureTemplateRequests
{
	/// <summary>
	/// Запрос шаблона существа по айди
	/// </summary>
	public class GetCreatureTemplateByIdQuery : IValidatableCommand<GetCreatureTemplateByIdResponse>
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
