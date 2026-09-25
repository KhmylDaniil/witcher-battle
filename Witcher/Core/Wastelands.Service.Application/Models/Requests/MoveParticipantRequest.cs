namespace Wastelands.Service.Application.Models.Requests
{
	/// <summary>
	/// Передвигаться может только активный сейчас участник (см. BattleCombatService.MoveAsync) — его
	/// самого явно указывать не нужно, только куда: сервер сам считает кратчайший (по стоимости) путь
	/// до этого гекса.
	/// </summary>
	public class MoveParticipantRequest : BaseRequest
	{
		public long BattleId { get; set; }

		public int Column { get; set; }

		public int Row { get; set; }
	}
}
