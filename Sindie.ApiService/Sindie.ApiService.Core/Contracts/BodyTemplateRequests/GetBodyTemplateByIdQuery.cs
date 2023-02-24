using Sindie.ApiService.Core.Abstractions;
using System;

namespace Sindie.ApiService.Core.Contracts.BodyTemplateRequests
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
