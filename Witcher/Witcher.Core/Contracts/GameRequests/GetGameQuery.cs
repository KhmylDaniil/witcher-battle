using System.Collections.Generic;
using Witcher.Core.Contracts.BaseRequests;
using MediatR;

namespace Witcher.Core.Contracts.GameRequests
{
	public sealed class GetGameQuery : GetBaseQuery, IRequest<IEnumerable<GetGameResponseItem>>
	{
		/// <summary>
		/// Фильтр по названию
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Фильтр по автору
		/// </summary>
		public string AuthorName { get; set; }
	}
}
