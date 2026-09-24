namespace Wastelands.Service.Application.Models.Dto
{
	/// <summary>Карта боя без гексов — для списков, где тысячи гексов каждой карты не нужны.</summary>
	public class BattleMapSummaryDto : BaseDto
	{
		public long GameId { get; set; }

		public string Name { get; set; }

		public string? Description { get; set; }

		public int Columns { get; set; }

		public int Rows { get; set; }
	}
}
