using Sindie.ApiService.Core.Contracts.RunBattleRequests;

namespace Witcher.MVC.ViewModels.RunBattle
{
	public class MakeTurnViewModel : MakeTurnResponse
	{
		/// <summary>
		/// Айди цели
		/// </summary>
		public Guid TargetCreatureId { get; set; }

		/// <summary>
		/// Айди способности атаки
		/// </summary>
		public Guid? AbilityId { get; set; }
	}
}
