using Witcher.Core.Contracts.RunBattleRequests;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.MVC.ViewModels.RunBattle
{
	public class MakeAttackViewModel : AttackWithAbilityCommand
	{
		public Dictionary<string, Guid?> CreatureParts { get; set; } = new();

		public Dictionary<string, Skill?> DefensiveSkills { get; set; } = new();
	}
}
