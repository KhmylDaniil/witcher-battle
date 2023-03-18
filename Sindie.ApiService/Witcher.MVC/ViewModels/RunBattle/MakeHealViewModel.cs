using Witcher.Core.Contracts.RunBattleRequests;

namespace Witcher.MVC.ViewModels.RunBattle
{
	public class MakeHealViewModel : HealEffectCommand
	{
		public Dictionary<Guid, string> EffectsOnTarget { get; set; } = new();
	}
}
