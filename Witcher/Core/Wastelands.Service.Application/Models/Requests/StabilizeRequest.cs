namespace Wastelands.Service.Application.Models.Requests
{
	/// <summary>Попытка стабилизировать умирающего персонажа броском FirstAid — см. BattleCombatService.StabilizeAsync.</summary>
	public class StabilizeRequest : BaseRequest
	{
		public long BattleId { get; set; }

		/// <summary>Character.Id умирающего персонажа-цели (не BattleCharacter.Id) — как и остальные target-поля в этом API.</summary>
		public long TargetCharacterId { get; set; }

		public int? Roll { get; set; }
	}
}
