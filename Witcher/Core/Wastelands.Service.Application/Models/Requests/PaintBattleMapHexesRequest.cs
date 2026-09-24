using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Requests
{
	/// <summary>Перекраска набора гексов карты одним запросом (все изменения редактора до "Сохранить").</summary>
	public class PaintBattleMapHexesRequest : BaseRequest
	{
		public long BattleMapId { get; set; }

		public List<PaintBattleMapHexItem> Hexes { get; set; } = [];
	}

	public class PaintBattleMapHexItem
	{
		public int Column { get; set; }

		public int Row { get; set; }

		public HexTerrainType TerrainType { get; set; }

		public HexTerrainStyle TerrainStyle { get; set; }
	}
}
