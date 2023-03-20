using Witcher.Core.Abstractions;
using System;

namespace Witcher.Core.Contracts.BodyTemplateRequests
{
	/// <summary>
	/// Запрос шаблона тела по айди
	/// </summary>
	public class GetBodyTemplateByIdQuery : IValidatableCommand<GetBodyTemplateByIdResponse>
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
