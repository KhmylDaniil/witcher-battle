using Witcher.Core.Abstractions;

namespace Witcher.Core.Contracts.UserRequests.GetUsers
{
	/// <summary>
	/// Запрос получение списка пользователей
	/// </summary>
	public sealed class GetUsersQuery: GetBaseQuery, IValidatableCommand<GetUsersQueryResponse>
	{
		/// <summary>
		/// Подстрока для поиска пользователей
		/// </summary>
		public string SearchText { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
