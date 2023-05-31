using System.Collections.Generic;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BaseRequests;

namespace Witcher.Core.Contracts.CharacterRequests
{
	public class GetCharactersCommand : GetBaseQuery, IValidatableCommand<IEnumerable<GetCharactersResponseItem>>
	{
		/// <summary>
		/// Фильтр по названию
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Фильтр по автору
		/// </summary>
		public string UserName { get; set; }

		public void Validate()
		{
			// Method intentionally left empty.
		}
	}


}
