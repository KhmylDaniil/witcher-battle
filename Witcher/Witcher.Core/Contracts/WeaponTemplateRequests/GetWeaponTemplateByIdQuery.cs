using System;
using Witcher.Core.Abstractions;

namespace Witcher.Core.Contracts.WeaponTemplateRequests
{
	public class GetWeaponTemplateByIdQuery : IValidatableCommand<GetWeaponTemplateByIdResponse>
	{
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
