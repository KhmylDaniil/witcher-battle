using Witcher.Core.Abstractions;
using System.Collections.Generic;

namespace Witcher.Core.Contracts.GameRequests
{
	public class GetGameQuery : GetBaseQuery, IValidatableCommand<IEnumerable<GetGameResponseItem>>
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
