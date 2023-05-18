using System.Collections.Generic;
using Witcher.Core.Contracts.BattleRequests;

namespace Witcher.Core.Contracts.CharacterRequests
{
	public class GetCharacterByIdResponse : GetCreatureByIdResponse
	{
		public List<GetCharacterByIdResponseItem> Items { get; set; }
	}
}