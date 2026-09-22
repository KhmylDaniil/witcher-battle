using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Drafts
{
	/// <summary>Правило снятия состояния броском навыка — какой навык, какая сложность, можно ли применить к другому участнику.</summary>
	public sealed record ConditionRemovalRule(Skill Skill, int Difficulty, bool SelfOnly);

	/// <summary>
	/// Правила снятия состояний действием в бою (BattleCombatService.AttemptRemoveConditionAsync).
	/// Кровотечение и Отравление снимаются броском навыка ≥ сложности; Огонь снимается автоматически
	/// (обычным действием, без броска — не входит в этот каталог); Удушье снимает только мастер вручную
	/// (RemoveCreatureCondition/RemoveCharacterCondition); Ошеломление/Ослепление спадают сами.
	/// </summary>
	public static class ConditionRemovalCatalog
	{
		private static readonly Dictionary<Condition, List<ConditionRemovalRule>> Rules = new()
		{
			[Condition.Poison] =
			[
				new ConditionRemovalRule(Skill.Endurance, 15, SelfOnly: true),
				new ConditionRemovalRule(Skill.FirstAid, 14, SelfOnly: false),
			],
			[Condition.Bleed] =
			[
				new ConditionRemovalRule(Skill.FirstAid, 14, SelfOnly: false),
			],
		};

		public static ConditionRemovalRule? FindRule(Condition condition, Skill skill)
			=> Rules.TryGetValue(condition, out var rules) ? rules.Find(r => r.Skill == skill) : null;
	}
}
