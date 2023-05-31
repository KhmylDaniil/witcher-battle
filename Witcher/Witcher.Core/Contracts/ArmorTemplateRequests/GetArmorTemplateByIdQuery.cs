using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;

namespace Witcher.Core.Contracts.ArmorTemplateRequests
{
	public class GetArmorTemplateByIdQuery : IValidatableCommand<GetArmorTemplateByIdResponse>
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
