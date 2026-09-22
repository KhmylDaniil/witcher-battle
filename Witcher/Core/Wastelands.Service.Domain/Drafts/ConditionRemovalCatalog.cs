using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Drafts
{
	/// <summary>Правило снятия состояния броском навыка — какой навык, какая сложность, можно ли применить к другому участнику.</summary>
	public sealed record ConditionRemovalRule(Skill Skill, int Difficulty, bool SelfOnly);

	/// <summary>
	/// Правила снятия состояний действием в бою. Кровотечение и Отравление снимаются броском навыка ≥
	/// сложности (BattleCombatService.AttemptRemoveConditionAsync, можно только с себя или, кроме
	/// self-only навыков, с другого участника); Огонь и Падение снимаются обычным действием без
	/// броска, всегда успешно и только с себя (BattleCombatService.ClearConditionAsync); Удушье
	/// снимает только мастер вручную (RemoveCreatureCondition/RemoveCharacterCondition); Ошеломление/
	/// Ослепление спадают сами.
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

		private static readonly HashSet<Condition> AutoClearable = [Condition.Fire, Condition.Prone];

		public static ConditionRemovalRule? FindRule(Condition condition, Skill skill)
			=> Rules.TryGetValue(condition, out var rules) ? rules.Find(r => r.Skill == skill) : null;

		public static bool IsAutoClearable(Condition condition) => AutoClearable.Contains(condition);
	}
}
