namespace Wastelands.Service.Application.Models.Requests
{
	public class AttachBattleMapRequest : BaseRequest
	{
		public long BattleId { get; set; }

		/// <summary>Карта игры, которую подключаем к бою; null — отключить карту.</summary>
		public long? BattleMapId { get; set; }
	}
}
