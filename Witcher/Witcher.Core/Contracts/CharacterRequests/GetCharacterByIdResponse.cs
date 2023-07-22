using System.Collections.Generic;
using Witcher.Core.Contracts.BattleRequests;

namespace Witcher.Core.Contracts.CharacterRequests
{
	public sealed class GetCharacterByIdResponse : GetCreatureByIdResponse
	{
		public List<GetCharacterByIdResponseItem> Items { get; set; }
	}
}