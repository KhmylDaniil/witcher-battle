namespace Wastelands.Service.Application.Models.Dto
{
	public class BattleLogEntryDto : BaseDto
	{
		public string Message { get; set; }

		public DateTime CreatedAt { get; set; }
	}
}
