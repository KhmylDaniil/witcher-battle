namespace Wastelands.Service.Application.Models.Dto
{
	public class BattleMapDto : BattleMapSummaryDto
	{
		/// <summary>Все гексы карты (ровно Columns × Rows штук), упорядочены по Row, затем по Column.</summary>
		public List<BattleMapHexDto> Hexes { get; set; } = [];
	}
}
