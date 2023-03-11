using Sindie.ApiService.Core.Contracts.RunBattleRequests;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Witcher.MVC.ViewModels.RunBattle
{
	public class MakeAttackViewModel : AttackCommand
	{
		public Dictionary<string, Guid?> CreatureParts { get; set; } = new();

		public Dictionary<string, Skill?> DefensiveSkills { get; set; } = new();
	}
}
