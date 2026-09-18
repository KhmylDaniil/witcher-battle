using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class RepairItemRequest : BaseRequest
	{
		public long CharacterId { get; set; }

		public long ItemId { get; set; }

		/// <summary>Обязательно для брони (какую часть чинить); игнорируется для оружия.</summary>
		public HumanBodyPart? Part { get; set; }

		public int Durability { get; set; }
	}
}
