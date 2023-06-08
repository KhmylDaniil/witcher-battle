using System;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Contracts.CharacterRequests
{
	public class GetCharacterByIdResponseItem
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public ItemType ItemType { get; set; }

		public bool? IsEquipped { get; set; }
	}
}