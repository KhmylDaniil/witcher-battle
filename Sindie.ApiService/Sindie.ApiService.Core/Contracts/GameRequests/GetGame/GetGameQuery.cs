using Sindie.ApiService.Core.Abstractions;

namespace Sindie.ApiService.Core.Contracts.GameRequests.GetGame
{
	public class GetGameQuery: GetBaseQuery, IValidatableCommand<GetGameResponse>
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

		/// <summary>
		/// Валидация
		/// </summary>
		public void Validate()
		{
			// Method intentionally left empty.
		}
	}
}
