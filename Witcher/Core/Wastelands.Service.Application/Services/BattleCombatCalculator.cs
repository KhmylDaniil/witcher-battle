using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Models;
using Wastelands.Service.Domain.Drafts;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Services
{
	/// <summary>Итог встречного броска — для MarkHitResolved и для подробного лога боя.</summary>
	internal readonly record struct HitResult(
		bool Succeeded,
		long? ResolvedCreaturePartId,
		HumanBodyPart? ResolvedHumanBodyPart,
		int AttackRoll,
		int AttackTotal,
		int DefenseRoll,
		int DefenseTotal);

	/// <summary>
	/// Итог расчёта урона. PartName заполнено при попадании в часть тела — у существа всегда (если
	/// удар прошёл), у персонажа тоже всегда (фиксированная анатомия — см. HumanBodyPartCatalog).
	/// ArmorBeforeHit/ArmorAfterHit/ArmorAbsorbed — 0, если часть ничем не защищена (нет шаблонной
	/// брони у существа / нет экипированной брони на этой части у персонажа). WornArmorItemId
	/// заполнено, только если урон пришёлся по части, покрытой экипированной бронёй персонажа —
	/// используется, чтобы отдельно изнашивать именно этот экземпляр (см. BattleCombatService).
	/// </summary>
	internal readonly record struct DamageResult(
		int FinalDamage,
		string? PartName,
		int RawDamage,
		int ArmorBeforeHit,
		int ArmorAbsorbed,
		int ArmorAfterHit,
		long? WornArmorItemId = null);

	/// <summary>Итог проверки на критический эффект — см. BattleCombatCalculator.TryResolveCriticalHit.</summary>
	internal readonly record struct CriticalHitResult(CriticalWoundSeverity Severity, Condition Wound, string SlotKey, int BonusDamage);

	/// <summary>
	/// Чистая боевая математика (попадание, урон) — без загрузки данных и без сохранения. Вынесена
	/// из BattleCombatService, чтобы там остался только оркестрация (авторизация/загрузка/лог/сохранение).
	/// </summary>
	internal static class BattleCombatCalculator
	{
		private static readonly Random Random = new();

		public static int RollDie(int sides) => Random.Next(1, sides + 1);

		/// <summary>Урон — сумма обычных (не взрывающихся) d6, поэтому диапазон ограничен: от всех единиц до всех шестёрок.</summary>
		public static void ValidateDamageRoll(int roll, Ability ability)
		{
			if (roll < ability.DamageDiceCount || roll > ability.DamageDiceCount * 6)
			{
				throw new InvalidArgumentException(
					ErrorCode.InvalidDamageRoll,
					$"Бросок урона должен быть от {ability.DamageDiceCount} до {ability.DamageDiceCount * 6} (сумма {ability.DamageDiceCount}к6).");
			}
		}

		/// <summary>
		/// Встречный бросок: (характеристика+навык атаки [-модификатор части тела]) + d10, против
		/// (характеристика+навык защиты) + d10. Часть тела — выбранная атакующим (у обоих типов
		/// защитника это вычитает HitPenalty этой части из итога атаки — плата за прицеливание),
		/// либо, если не выбрана, случайная (у существа — по d10 в диапазон MinToHit..MaxToHit, у
		/// персонажа — по HumanBodyPartCatalog.ResolveByRoll).
		/// Оглушённый защитник не бросает защиту вовсе — его итог фиксирован на 10 (см. defenderIsStunned).
		/// </summary>
		public static HitResult ResolveHit(
			ParticipantCombatContext attackerContext,
			ParticipantCombatContext defenderContext,
			Ability ability,
			BattleAttack attack,
			bool defenderIsStunned = false)
		{
			long? resolvedPartId = null;
			HumanBodyPart? resolvedHumanBodyPart = null;
			var hitPenalty = 0;

			if (attack.DefenderKind == ParticipantKind.Creature)
			{
				var parts = defenderContext.Template!.Parts;
				if (attack.TargetedCreaturePartId is { } chosenPartId)
				{
					resolvedPartId = chosenPartId;
					hitPenalty = parts.First(p => p.Id == chosenPartId).HitPenalty;
				}
				else
				{
					var hitRoll = RollDie(10);
					var part = parts.FirstOrDefault(p => hitRoll >= p.MinToHit && hitRoll <= p.MaxToHit) ?? parts.First();
					resolvedPartId = part.Id;
				}
			}
			else
			{
				if (attack.TargetedHumanBodyPart is { } chosenHumanPart)
				{
					resolvedHumanBodyPart = chosenHumanPart;
					hitPenalty = HumanBodyPartCatalog.Get(chosenHumanPart).HitPenalty;
				}
				else
				{
					resolvedHumanBodyPart = HumanBodyPartCatalog.ResolveByRoll(RollDie(10));
				}
			}

			var attackRollUsed = attack.AttackRoll ?? RollDie(10);
			var attackTotal = attackerContext.GetSkillValue(ability.AttackSkill) - hitPenalty + attackRollUsed + ability.AttackModifier;

			int defenseRollUsed;
			int defenseTotal;
			if (defenderIsStunned)
			{
				defenseRollUsed = 0;
				defenseTotal = 10;
			}
			else
			{
				defenseRollUsed = attack.DefenseRoll ?? RollDie(10);
				defenseTotal = defenderContext.GetSkillValue(attack.DefensiveSkill!.Value) + defenseRollUsed;
			}

			var succeeded = attackTotal > defenseTotal;
			return new HitResult(
				succeeded,
				succeeded ? resolvedPartId : null,
				succeeded ? resolvedHumanBodyPart : null,
				attackRollUsed,
				attackTotal,
				defenseRollUsed,
				defenseTotal);
		}

		/// <summary>
		/// Бросок урона способности + модификатор, затем поглощение бронёй пробитой части, модификатор
		/// части тела и последним — модификатор типа урона (Vulnerability×2/Resistance÷2/Immunity×0).
		/// У существа броня и модификаторы — на шаблоне (CalculateCreatureDamage); у персонажа — на
		/// экипированном предмете, покрывающем пробитую часть, если он есть (CalculateCharacterDamage).
		/// </summary>
		public static DamageResult CalculateDamage(
			ParticipantCombatContext attackerContext,
			ParticipantCombatContext defenderContext,
			ParticipantKind defenderKind,
			Ability ability,
			BattleAttack attack)
		{
			var damageRoll = attack.DamageRoll ?? Enumerable.Range(0, ability.DamageDiceCount).Sum(_ => RollDie(6));
			double raw = damageRoll + ability.DamageModifier;
			var rawDamage = Math.Max(0, (int)Math.Round(raw));

			return defenderKind == ParticipantKind.Creature
				? CalculateCreatureDamage(defenderContext, ability, attack, rawDamage)
				: CalculateCharacterDamage(defenderContext, ability, attack, rawDamage);
		}

		private static DamageResult CalculateCreatureDamage(ParticipantCombatContext defenderContext, Ability ability, BattleAttack attack, int rawDamage)
		{
			var part = defenderContext.Template!.Parts.First(p => p.Id == attack.ResolvedCreaturePartId);

			var armorReduction = defenderContext.Creature!.GetArmorReduction(part.Id);
			var armorBeforeHit = Math.Max(0, part.Armor - armorReduction);
			var armorAfterHit = Math.Max(0, part.Armor - (armorReduction + 1));
			var armorAbsorbed = Math.Min(rawDamage, armorBeforeHit);
			var damageAfterArmor = Math.Max(0, rawDamage - armorBeforeHit);

			double afterPartModifier = damageAfterArmor * part.DamageModifier;
			afterPartModifier = ApplyDamageTypeModifier(afterPartModifier, defenderContext.Template.DamageTypeModifiers, ability.DamageType);

			var finalDamage = Math.Max(0, (int)Math.Round(afterPartModifier));

			return new DamageResult(finalDamage, part.Name, rawDamage, armorBeforeHit, armorAbsorbed, armorAfterHit);
		}

		/// <summary>
		/// Броня персонажа приходит не из шаблона (как у существ), а из экипированного предмета,
		/// покрывающего пробитую часть тела (не более одного — оверлап запрещён при экипировке, см.
		/// CharacterItemService.EquipAsync). Прочность части — это и есть её текущее значение брони:
		/// поглощает урон, изнашивается на 1 после каждого попадания (Item.WearArmor, вызывается
		/// BattleCombatService после этого расчёта), и пока не изношена до нуля — действуют
		/// модификаторы типа урона брони.
		/// </summary>
		private static DamageResult CalculateCharacterDamage(ParticipantCombatContext defenderContext, Ability ability, BattleAttack attack, int rawDamage)
		{
			var humanPart = attack.ResolvedHumanBodyPart!.Value;
			var partInfo = HumanBodyPartCatalog.Get(humanPart);

			var armorItem = defenderContext.Character!.Items.FirstOrDefault(i =>
				i.IsEquipped && i.ItemType == ItemType.Armor && i.ArmorParts.Any(p => p.Part == humanPart));
			var armorPart = armorItem?.ArmorParts.First(p => p.Part == humanPart);

			var armorBeforeHit = armorPart?.CurrentDurability ?? 0;
			var armorAfterHit = Math.Max(0, armorBeforeHit - 1);
			var armorAbsorbed = Math.Min(rawDamage, armorBeforeHit);
			var damageAfterArmor = Math.Max(0, rawDamage - armorBeforeHit);

			double afterPartModifier = damageAfterArmor * partInfo.DamageModifier;
			if (armorBeforeHit > 0)
			{
				afterPartModifier = ApplyDamageTypeModifier(afterPartModifier, armorItem!.DamageTypeModifiers, ability.DamageType);
			}

			var finalDamage = Math.Max(0, (int)Math.Round(afterPartModifier));

			return new DamageResult(finalDamage, partInfo.Name, rawDamage, armorBeforeHit, armorAbsorbed, armorAfterHit, armorItem?.Id);
		}

		private static double ApplyDamageTypeModifier(double damage, Dictionary<DamageType, DamageTypeModifier> modifiers, DamageType damageType)
		{
			if (!modifiers.TryGetValue(damageType, out var modifier))
			{
				return damage;
			}

			return modifier switch
			{
				DamageTypeModifier.Vulnerability => damage * 2,
				DamageTypeModifier.Resistance => damage / 2,
				DamageTypeModifier.Immunity => 0,
				_ => damage,
			};
		}

		/// <summary>
		/// Критический эффект: если бросок атаки превысил бросок защиты на 7/10/13 и более — Simple/
		/// Medium/Difficult ранение (см. CriticalWoundCatalog) той части тела, в которую пришёлся удар,
		/// и того типа урона, которым он нанесён. Null, если превышение меньше 7 (то есть, критического
		/// эффекта нет вовсе) — вызывающий код уже гарантирует, что удар нанёс хоть 1 урон.
		/// </summary>
		public static CriticalHitResult? TryResolveCriticalHit(BattleAttack attack, ParticipantCombatContext defenderContext, DamageType damageType)
		{
			var excess = attack.AttackTotal - attack.DefenseTotal;
			if (CriticalWoundCatalog.GetSeverity(excess) is not { } severity)
			{
				return null;
			}

			BodyPartType bodyPartType;
			string slotKey;
			if (attack.DefenderKind == ParticipantKind.Creature)
			{
				var part = defenderContext.Template!.Parts.First(p => p.Id == attack.ResolvedCreaturePartId);
				bodyPartType = part.BodyPartType;
				slotKey = CriticalWoundCatalog.SlotKey(part.Id, damageType);
			}
			else
			{
				var humanPart = attack.ResolvedHumanBodyPart!.Value;
				bodyPartType = HumanBodyPartCatalog.GetBodyPartType(humanPart);
				slotKey = CriticalWoundCatalog.SlotKey(humanPart, damageType);
			}

			var wound = CriticalWoundCatalog.GetWound(severity, bodyPartType, damageType);
			return new CriticalHitResult(severity, wound, slotKey, CriticalWoundCatalog.GetBonusDamage(severity));
		}

		/// <summary>Каждое состояние способности накладывается независимо, с вероятностью, равной его ApplyChance (%).</summary>
		public static List<Condition> RollAppliedConditions(Ability ability)
		{
			var applied = new List<Condition>();

			foreach (var appliedCondition in ability.AppliedConditions)
			{
				if (RollDie(100) <= appliedCondition.ApplyChance)
				{
					applied.Add(appliedCondition.Condition);
				}
			}

			return applied;
		}
	}
}
