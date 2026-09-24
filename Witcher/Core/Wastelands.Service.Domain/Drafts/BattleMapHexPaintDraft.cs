using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Drafts
{
	/// <summary>Новое состояние одного гекса для <see cref="Entities.BattleMap.PaintHexes"/>.</summary>
	public record BattleMapHexPaintDraft(int Column, int Row, HexTerrainType TerrainType, HexTerrainStyle TerrainStyle);
}
