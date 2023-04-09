using Witcher.Core.Abstractions;
using Witcher.Core.Entities;
using System;
using System.Linq;
using Witcher.Core.Exceptions;

namespace Witcher.Core.Logic
{
	/// <summary>
	/// Данные для расчета атаки
	/// </summary>
	internal class AttackData
	{
		/// <summary>
		/// Атакующее существо
		/// </summary>
		internal Creature Attacker { get; private set; }

		/// <summary>
		/// Существо цель
		/// </summary>
		internal Creature Target { get; private set; }

		/// <summary>
		/// Атакующая способность
		/// </summary>
		internal IAttackFormula AttackFormula { get; private set; }

		/// <summary>
		/// Часть тела цели
		/// </summary>
		internal CreaturePart AimedPart { get; private set; }

		/// <summary>
		/// База защиты
		/// </summary>
		internal int DefenseBase { get; private set; }

		/// <summary>
		/// Специальный бонус к попаданию
		/// </summary>
		internal int ToHit { get; private set; }

		/// <summary>
		/// Специальный бонус к урону
		/// </summary>
		internal int ToDamage { get; private set; }

		/// <summary>
		/// Флаг сильной атаки оружием
		/// </summary>
		internal bool? isStrongWeaponAttack { get; private set; }

		/// <summary>
		/// Создание данных для расчета атаки способностью
		/// </summary>
		/// <param name="attacker">Атакующее существо</param>
		/// <param name="target">Существо цель</param>
		/// <param name="aimedPart">Часть тела цель</param>
		/// <param name="attackFormula">Интерфейс формулы атаки</param>
		/// <param name="defensiveSkill">Навык защиты</param>
		/// <param name="specialToHit">Специальный бонус к попаданию</param>
		/// <param name="specialToDamage">Специальный бонус к урону</param>
		/// <returns>Данные для расчета атаки</returns>
		internal static AttackData CreateData(
			Creature attacker,
			Creature target,
			CreaturePart aimedPart,
			IAttackFormula attackFormula,
			CreatureSkill defensiveSkill,
			int specialToHit,
			int specialToDamage,
			bool? isStrongAttack = default)
		{
			var defenseBase = defensiveSkill is null ? target.DefaultDefenseBase(attackFormula) : target.SkillBase(defensiveSkill.Skill);

			specialToHit = DefineSpecialToHit(attacker, aimedPart, specialToHit);

			aimedPart = DefineCreaturePartIfNotAimed(target, aimedPart);

			if (attacker.Turn.MuitiattackAttackFormulaId is not null && attacker.Turn.MuitiattackAttackFormulaId.Value != attackFormula.Id)
				throw new LogicBaseException("Должна быть проведена мультиатака, но выбрана другая способность");

			return new AttackData()
			{
				Attacker = attacker,
				Target = target,
				AttackFormula = attackFormula,
				AimedPart = aimedPart,
				DefenseBase = defenseBase,
				ToHit = specialToHit,
				ToDamage = specialToDamage,
				isStrongWeaponAttack = isStrongAttack
			};
		}

		private static CreaturePart DefineCreaturePartIfNotAimed(Creature target, CreaturePart aimedPart)
		{
			return AimedPartIsNullOrDismembered() ? DefaultCreaturePart() : aimedPart;

			bool AimedPartIsNullOrDismembered()
			{
				if (aimedPart is null)
					return true;

				return target.Effects.Any(x => x is CritEffect crit && crit.CreaturePartId == aimedPart.Id
					&& crit is ISharedPenaltyCrit limbCrit && limbCrit.Severity >= BaseData.Enums.Severity.Deadly);
			}

			CreaturePart DefaultCreaturePart()
			{
				Random random = new();
				int roll;

				do
				{
					roll = random.Next(1, 10);
					aimedPart = target.CreatureParts.First(x => x.MinToHit <= roll && x.MaxToHit >= roll);
				}
				while (AimedPartIsNullOrDismembered());

				return aimedPart;
			}
		}

		private static int DefineSpecialToHit(Creature attacker, CreaturePart aimedPart, int specialToHit)
		{
			specialToHit = aimedPart is null ? specialToHit : specialToHit - aimedPart.HitPenalty;

			if (attacker.Turn.TurnState >= BaseData.Enums.TurnState.BaseActionIsDone)
				specialToHit -= 3;
			return specialToHit;
		}
	}
}
