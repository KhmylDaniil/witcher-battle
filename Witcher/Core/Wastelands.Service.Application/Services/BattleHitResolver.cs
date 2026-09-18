using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Services
{
	public class BattleHitResolver : IBattleHitResolver
	{
		private readonly IBattleCombatContextProvider _contextProvider;

		public BattleHitResolver(IBattleCombatContextProvider contextProvider)
		{
			_contextProvider = contextProvider;
		}

		public async Task ResolveIfBothConfirmedAsync(Battle battle, BattleAttack attack)
		{
			if (!attack.AttackerConfirmed || !attack.DefenderConfirmed)
			{
				return;
			}

			var attackerContext = await _contextProvider.GetContextAsync(battle, attack.AttackerKind, attack.AttackerId);
			var ability = attackerContext.Abilities.First(a => a.Id == attack.AbilityId);
			var defenderContext = await _contextProvider.GetContextAsync(battle, attack.DefenderKind, attack.DefenderId);

			var hit = BattleCombatCalculator.ResolveHit(attackerContext, defenderContext, ability, attack);
			attack.MarkHitResolved(
				hit.Succeeded, hit.ResolvedCreaturePartId, hit.ResolvedHumanBodyPart, hit.AttackRoll, hit.AttackTotal, hit.DefenseRoll, hit.DefenseTotal);

			if (!hit.Succeeded)
			{
				var attackerName = BattleParticipants.GetName(battle, attack.AttackerKind, attack.AttackerId);
				var defenderName = BattleParticipants.GetName(battle, attack.DefenderKind, attack.DefenderId);
				battle.AddLogEntry(BattleCombatLogFormatter.FormatMiss(attackerName, ability.Name, defenderName, hit));
			}
		}

		public async Task<List<Skill>> GetAvailableDefensiveSkillsAsync(Battle battle, BattleAttack attack)
		{
			var attackerContext = await _contextProvider.GetContextAsync(battle, attack.AttackerKind, attack.AttackerId);
			var ability = attackerContext.Abilities.First(a => a.Id == attack.AbilityId);
			return ability.DefensiveSkills.Count > 0
				? ability.DefensiveSkills.Select(x => x.Skill).ToList()
				: [Skill.Dodge];
		}
	}
}
