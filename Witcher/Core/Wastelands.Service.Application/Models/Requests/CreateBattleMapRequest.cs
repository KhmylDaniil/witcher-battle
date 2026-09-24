using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class CreateBattleMapRequest : BaseRequest
	{
		public long GameId { get; set; }

		public string Name { get; set; }

		public string? Description { get; set; }

		public int Columns { get; set; }

		public int Rows { get; set; }

		/// <summary>Стиль, которым изначально заполняется вся карта (простой террейн).</summary>
		public HexTerrainStyle TerrainStyle { get; set; }
	}
}
