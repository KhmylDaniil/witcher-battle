using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Application.Models;
using Wastelands.Service.Domain.Drafts;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Services
{
	/// <summary>
	/// Критический провал (fumble) броска атаки/защиты — три отдельных таблицы последствий в
	/// зависимости от того, чем атаковали/защищались (см. методы Apply*FumbleAsync ниже). Срабатывает
	/// на AttackRoll атакующего и DefenseRoll защитника независимо — оба могут фамблить в одном выпаде.
	/// Урон дальнобойным оружием — заглушка (TODO), правила ещё не определены.
	/// </summary>
	public class BattleFumbleResolver : IBattleFumbleResolver
	{
		private readonly IBattleCombatContextProvider _contextProvider;
		private readonly ICharacterRepository _characterRepository;

		public BattleFumbleResolver(IBattleCombatContextProvider contextProvider, ICharacterRepository characterRepository)
		{
			_contextProvider = contextProvider;
			_characterRepository = characterRepository;
		}

		public async Task<bool> FinalizeSwingAsync(Battle battle, BattleAttack attack)
		{
			if (!attack.AttackerFumbleResolved)
			{
				attack.MarkAttackerFumbleResolved();
				if (await ApplyAttackerFumbleAsync(battle, attack))
				{
					return true;
				}
			}

			if (!attack.DefenderFumbleResolved)
			{
				attack.MarkDefenderFumbleResolved();
				if (await ApplyDefenderFumbleAsync(battle, attack))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>Табл. A (оружие ближнего боя) / Табл. B (способность без оружия, либо атакующее существо) / TODO-заглушка (дальнобойное оружие).</summary>
		private async Task<bool> ApplyAttackerFumbleAsync(Battle battle, BattleAttack attack)
		{
			if (attack.AttackRoll is not { } roll || roll > FumbleRules.Threshold)
			{
				return false;
			}

			var attackerName = BattleParticipants.GetName(battle, attack.AttackerKind, attack.AttackerId);
			battle.AddLogEntry($"{attackerName} допускает критический провал атаки (бросок {roll}).");

			if (FumbleRules.GetTier(roll) is not { } tier)
			{
				return false;
			}

			var attackerContext = await _contextProvider.GetContextAsync(battle, attack.AttackerKind, attack.AttackerId);
			var ability = attackerContext.Abilities.First(a => a.Id == attack.AbilityId);

			var weapon = BattleParticipants.GetEquippedWeapon(attackerContext, ability);

			if (weapon is { WeaponKind: WeaponKind.Ranged })
			{
				// TODO: отдельные правила критического провала для атаки оружием дальнего боя — ещё не определены.
				battle.AddLogEntry($"{attackerName}: правила критического провала для дальнобойного оружия ещё не реализованы.");
				return false;
			}

			if (weapon is not null)
			{
				return await ApplyWeaponAttackFumbleAsync(battle, attackerContext, weapon, ability, tier);
			}

			return await ApplyNonWeaponFumbleAsync(battle, attack, attack.AttackerKind, attack.AttackerId, attackerContext, tier, reason: "критический провал атаки");
		}

		/// <summary>Табл. C (защита оружием ближнего боя — блок или парирование) / Табл. B (базовая защита dodge/acrobatics либо особый защитный навык способности).</summary>
		private async Task<bool> ApplyDefenderFumbleAsync(Battle battle, BattleAttack attack)
		{
			if (attack.DefenseRoll is not { } roll || roll > FumbleRules.Threshold)
			{
				return false;
			}

			var defenderName = BattleParticipants.GetName(battle, attack.DefenderKind, attack.DefenderId);
			battle.AddLogEntry($"{defenderName} допускает критический провал защиты (бросок {roll}).");

			if (FumbleRules.GetTier(roll) is not { } tier)
			{
				return false;
			}

			var defenderContext = await _contextProvider.GetContextAsync(battle, attack.DefenderKind, attack.DefenderId);

			var blockSkill = BattleParticipants.GetEquippedMeleeWeaponSkill(attack.DefenderKind, defenderContext);
			var isWeaponDefense = attack.IsParry || (blockSkill is not null && attack.DefensiveSkill == blockSkill);

			if (isWeaponDefense)
			{
				var weapon = defenderContext.Character!.Items.First(
					i => i.IsEquipped && i.ItemType == ItemType.Weapon && i.WeaponKind == WeaponKind.Melee);
				return await ApplyWeaponDefenseFumbleAsync(battle, attack, defenderContext, weapon, tier);
			}

			return await ApplyNonWeaponFumbleAsync(battle, attack, attack.DefenderKind, attack.DefenderId, defenderContext, tier, reason: "критический провал защиты");
		}

		/// <summary>
		/// 6 — Ошеломление; 7 — оружие снимается; 8 — прочность -1д10; 9+ — самоудар (см. SelfAttackAsync).
		/// Никогда не требует интерактивной проверки Оглушения.
		/// </summary>
		private async Task<bool> ApplyWeaponAttackFumbleAsync(Battle battle, ParticipantCombatContext attackerContext, Item weapon, Ability ability, int tier)
		{
			var character = attackerContext.Character!;

			if (tier == 6)
			{
				BattleParticipants.AddCondition(battle, ParticipantKind.Character, character.Id, Condition.Staggered);
				battle.AddLogEntry($"{character.Name} получает Ошеломление (критический провал атаки).");
			}
			else if (tier == 7)
			{
				await UnequipWeaponAsync(battle, character, weapon, "критический провал атаки");
			}
			else if (tier == 8)
			{
				await WearWeaponAndLogAsync(battle, character, weapon, RollDice(1, 10), "критический провал атаки");
			}
			else
			{
				// >= 9 — атакующий бьёт по себе тем же оружием/способностью, которой атаковал.
				await SelfAttackAsync(
					battle, ParticipantKind.Character, character.Id, attackerContext,
					ability.DamageDiceCount, ability.DamageModifier, ability.DamageType, "критический провал атаки");
			}

			return false;
		}

		/// <summary>
		/// 6 — прочность -1д6; 7 — оружие снимается; 8 — Падение + проверка Оглушения (интерактивно);
		/// 9 — прочность -2д6; >9 — самоудар оружием защитника.
		/// </summary>
		private async Task<bool> ApplyWeaponDefenseFumbleAsync(Battle battle, BattleAttack attack, ParticipantCombatContext defenderContext, Item weapon, int tier)
		{
			var character = defenderContext.Character!;

			if (tier == 6)
			{
				await WearWeaponAndLogAsync(battle, character, weapon, RollDice(1, 6), "критический провал защиты");
				return false;
			}

			if (tier == 7)
			{
				await UnequipWeaponAsync(battle, character, weapon, "критический провал защиты");
				return false;
			}

			if (tier == 8)
			{
				BattleParticipants.AddCondition(battle, attack.DefenderKind, attack.DefenderId, Condition.Prone);
				battle.AddLogEntry($"{character.Name} получает Падение (критический провал защиты).");
				attack.BeginFumbleStunSave(attack.DefenderKind, attack.DefenderId);
				return true;
			}

			if (tier == 9)
			{
				await WearWeaponAndLogAsync(battle, character, weapon, RollDice(2, 6), "критический провал защиты");
				return false;
			}

			// > 9 — оружие, которым защищались, бьёт по самому защитнику.
			await SelfAttackAsync(
				battle, attack.DefenderKind, attack.DefenderId, defenderContext,
				weapon.DamageDiceCount!.Value, weapon.DamageModifier!.Value, weapon.DamageType!.Value, "критический провал защиты");
			return false;
		}

		/// <summary>
		/// Способность без оружия, атакующее существо или базовая защита (dodge/acrobatics/особый
		/// защитный навык способности): 6 — Ошеломление; 7 — Падение; 8 — Падение + проверка Оглушения
		/// (интерактивно); 9+ — то же плюс 1д6 урона в голову (0, если у цели нет головы; смягчается
		/// бронёй).
		/// </summary>
		private async Task<bool> ApplyNonWeaponFumbleAsync(
			Battle battle, BattleAttack attack, ParticipantKind kind, long participantId, ParticipantCombatContext context, int tier, string reason)
		{
			var name = BattleParticipants.GetName(battle, kind, participantId);

			if (tier == 6)
			{
				BattleParticipants.AddCondition(battle, kind, participantId, Condition.Staggered);
				battle.AddLogEntry($"{name} получает Ошеломление ({reason}).");
				return false;
			}

			BattleParticipants.AddCondition(battle, kind, participantId, Condition.Prone);
			battle.AddLogEntry($"{name} получает Падение ({reason}).");

			if (tier == 7)
			{
				return false;
			}

			if (tier >= 9)
			{
				await ApplyHeadDamageAsync(battle, kind, participantId, context);
			}

			attack.BeginFumbleStunSave(kind, participantId);
			return true;
		}

		/// <summary>1д6 урона в голову (0, если у цели нет головы), смягчается бронёй — см. BattleCombatCalculator.CalculateDamageToPart.</summary>
		private async Task ApplyHeadDamageAsync(Battle battle, ParticipantKind kind, long participantId, ParticipantCombatContext context)
		{
			long? creaturePartId = null;
			HumanBodyPart? humanBodyPart = null;

			if (kind == ParticipantKind.Creature)
			{
				var headPart = context.Template!.Parts.FirstOrDefault(p => p.BodyPartType == BodyPartType.Head);
				if (headPart is null)
				{
					return;
				}

				creaturePartId = headPart.Id;
			}
			else
			{
				humanBodyPart = HumanBodyPart.Head;
			}

			var rawDamage = BattleCombatCalculator.RollDie(6);
			var damage = BattleCombatCalculator.CalculateDamageToPart(context, kind, DamageType.Bludgeoning, creaturePartId, humanBodyPart, rawDamage);

			await ApplyResolvedDamageAsync(battle, kind, participantId, context, damage, creaturePartId, humanBodyPart, "падает и бьётся головой");
		}

		/// <summary>
		/// Самоудар при тяжёлом критическом провале — минимальное превышение (условно "едва попадает",
		/// никогда не даёт критическое ранение, см. CriticalWoundCatalog — порог начинается с 7),
		/// случайная часть тела.
		/// </summary>
		private async Task SelfAttackAsync(
			Battle battle, ParticipantKind kind, long participantId, ParticipantCombatContext context,
			int damageDiceCount, int damageModifier, DamageType damageType, string reason)
		{
			var (creaturePartId, humanBodyPart) = BattleCombatCalculator.ResolveRandomBodyPart(context, kind);
			var rawDamage = Math.Max(0, RollDice(damageDiceCount, 6) + damageModifier);
			var damage = BattleCombatCalculator.CalculateDamageToPart(context, kind, damageType, creaturePartId, humanBodyPart, rawDamage);

			await ApplyResolvedDamageAsync(battle, kind, participantId, context, damage, creaturePartId, humanBodyPart, $"наносит удар по себе ({reason})");
		}

		private async Task ApplyResolvedDamageAsync(
			Battle battle, ParticipantKind kind, long participantId, ParticipantCombatContext context,
			DamageResult damage, long? creaturePartId, HumanBodyPart? humanBodyPart, string verb)
		{
			var name = BattleParticipants.GetName(battle, kind, participantId);
			battle.AddLogEntry(
				$"{name} {verb}: {damage.PartName} — {damage.FinalDamage} урона"
					+ (damage.ArmorAbsorbed > 0 ? $" (броня поглотила {damage.ArmorAbsorbed})." : "."));

			BattleParticipants.ApplyPeriodicDamage(battle, kind, participantId, damage.FinalDamage);

			if (kind == ParticipantKind.Creature && creaturePartId is { } partId)
			{
				context.Creature!.WearArmor(partId);
			}
			else if (damage.WornArmorItemId is { } wornItemId)
			{
				context.Character!.Items.First(i => i.Id == wornItemId).WearArmor(humanBodyPart!.Value);
				await _characterRepository.UpdateAsync(context.Character);
			}
		}

		private static int RollDice(int count, int sides) => Enumerable.Range(0, count).Sum(_ => BattleCombatCalculator.RollDie(sides));

		private async Task WearWeaponAndLogAsync(Battle battle, Character character, Item weapon, int amount, string reason)
		{
			weapon.WearWeapon(amount);
			battle.AddLogEntry($"{character.Name}: {weapon.Name} теряет {amount} прочности ({reason}), осталось {weapon.Durability}.");
			UnequipIfBroken(battle, character, weapon);
			await _characterRepository.UpdateAsync(character);
		}

		private async Task UnequipWeaponAsync(Battle battle, Character character, Item weapon, string reason)
		{
			if (!weapon.IsEquipped)
			{
				return;
			}

			CharacterItemService.RemoveGeneratedAbilities(character, weapon);
			weapon.Unequip();
			battle.AddLogEntry($"{character.Name}: {weapon.Name} перестаёт быть экипирован ({reason}).");
			await _characterRepository.UpdateAsync(character);
		}

		private static void UnequipIfBroken(Battle battle, Character character, Item weapon)
		{
			if (weapon.ItemType != ItemType.Weapon || !weapon.IsEquipped || weapon.Durability > 0)
			{
				return;
			}

			CharacterItemService.RemoveGeneratedAbilities(character, weapon);
			weapon.Unequip();
			battle.AddLogEntry($"{character.Name}: {weapon.Name} сломано (прочность 0) и перестаёт быть экипировано.");
		}
	}
}
