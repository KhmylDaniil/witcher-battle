using MediatR;
using System.Collections.Generic;
using Witcher.Core.Contracts.BaseRequests;

namespace Witcher.Core.Contracts.ItemTemplateRequests
{
	public sealed class GetItemTemplateQuery : GetBaseQuery, IRequest<IEnumerable<GetItemTemplateResponse>>
	{
	}
}
