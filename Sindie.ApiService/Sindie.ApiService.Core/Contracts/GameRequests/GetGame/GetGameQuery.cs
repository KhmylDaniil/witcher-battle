using MediatR;

namespace Sindie.ApiService.Core.Contracts.GameRequests.GetGame
{
	public class GetGameQuery: GetBaseQuery, IRequest<GetGameResponse>
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
