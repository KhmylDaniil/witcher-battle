using Witcher.Core.Contracts.RunBattleRequests;

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

		public string AttackButtonValue() => TurnState switch
			{
				Core.BaseData.Enums.TurnState.ReadyForAction => "Attack",
				Core.BaseData.Enums.TurnState.InProcessOfBaseAction => "Multiattack",
				Core.BaseData.Enums.TurnState.BaseActionIsDone => "Additional attack",
				Core.BaseData.Enums.TurnState.InProcessOfAdditionAction => "Multiattack",
				_ => "Turn is over",
			};
	}
}
