using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	public class UpdateBattleMapRequest : BaseRequest
	{
		public long Id { get; set; }

		public string Name { get; set; }

		public string? Description { get; set; }

		public int Columns { get; set; }

		public int Rows { get; set; }

		/// <summary>Стиль простого террейна для гексов, появившихся при увеличении карты.</summary>
		public HexTerrainStyle FillTerrainStyle { get; set; }
	}
}
