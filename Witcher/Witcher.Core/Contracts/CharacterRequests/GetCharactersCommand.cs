using MediatR;
using System.Collections.Generic;
using Witcher.Core.Contracts.BaseRequests;

namespace Witcher.Core.Contracts.CharacterRequests
{
	public sealed class GetCharactersCommand : GetBaseQuery, IRequest<IEnumerable<GetCharactersResponseItem>>
	{
		/// <summary>
		/// Фильтр по названию
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Фильтр по автору
		/// </summary>
		public string UserName { get; set; }
	}
}
