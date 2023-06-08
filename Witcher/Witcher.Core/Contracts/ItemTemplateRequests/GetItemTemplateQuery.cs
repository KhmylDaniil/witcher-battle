using System.Collections.Generic;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BaseRequests;

namespace Witcher.Core.Contracts.ItemTemplateRequests
{
	public class GetItemTemplateQuery : GetBaseQuery, IValidatableCommand<IEnumerable<GetItemTemplateResponse>>
	{
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
