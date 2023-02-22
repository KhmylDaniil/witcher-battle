using MediatR;

namespace Sindie.ApiService.Core.Contracts.UserRequests.GetUsers
{
	/// <summary>
	/// Запрос получение списка пользователей
	/// </summary>
	public sealed class GetUsersQuery: GetBaseQuery, IRequest<GetUsersQueryResponse>
	{
		/// <summary>
		/// Подстрока для поиска пользователей
		/// </summary>
		public string SearchText { get; set; }
	}
}
