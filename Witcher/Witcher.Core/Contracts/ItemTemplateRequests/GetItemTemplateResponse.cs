using System;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Contracts.ItemTemplateRequests
{
	public sealed class GetItemTemplateResponse
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public ItemType ItemType { get; set; }
	}
}