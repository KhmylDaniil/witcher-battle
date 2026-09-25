namespace Wastelands.Service.Application.Models.Dto
{
	/// <summary>Один гекс, до которого участник может дойти прямо сейчас, и сколько это стоит очков движения.</summary>
	public class MovementRangeHexDto
	{
		public int Column { get; set; }

		public int Row { get; set; }

		public int Cost { get; set; }
	}
}
