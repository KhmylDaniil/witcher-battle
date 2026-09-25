using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Models.Dto
{
	public class BattleMapHexDto
	{
		public int Column { get; set; }

		public int Row { get; set; }

		public HexTerrainType TerrainType { get; set; }

		public HexTerrainStyle TerrainStyle { get; set; }

		public bool IsPassable { get; set; }

		public int? MovementCost { get; set; }

		public string? MarkerText { get; set; }

		public bool MarkerVisibleToPlayers { get; set; }
	}
}
