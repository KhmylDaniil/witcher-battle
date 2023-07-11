using MediatR;
using System.Collections.Generic;
using Witcher.Core.Contracts.BaseRequests;

namespace Witcher.Core.Contracts.UserRequests
{
	/// <summary>
	/// Запрос получение списка пользователей
	/// </summary>
	public sealed class GetUsersQuery : GetBaseQuery, IRequest<IEnumerable<GetUsersQueryResponse>>
	{
		/// <summary>
		/// Подстрока для поиска пользователей
		/// </summary>
		public string SearchText { get; set; }
	}
}
