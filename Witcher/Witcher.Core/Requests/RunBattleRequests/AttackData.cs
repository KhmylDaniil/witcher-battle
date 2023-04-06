using Witcher.Core.Abstractions;
using Witcher.Core.Entities;
using System;
using System.Linq;
using Witcher.Core.Exceptions;

namespace Witcher.Core.Requests.RunBattleRequests
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
		internal Ability Ability { get; private set; }

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
		/// Создание данных для расчета атаки
		/// </summary>
		/// <param name="attacker">Атакующее существо</param>
		/// <param name="target">Существо цель</param>
		/// <param name="aimedPart">Часть тела цель</param>
		/// <param name="ability">Атакующая способность</param>
		/// <param name="defensiveSkill">Навык защиты</param>
		/// <param name="specialToHit">Специальный бонус к попаданию</param>
		/// <param name="specialToDamage">Специальный бонус к урону</param>
		/// <returns>Данные для расчета атаки</returns>
		internal static AttackData CreateData(
			Creature attacker,
			Creature target,
			CreaturePart aimedPart,
			Ability ability,
			CreatureSkill defensiveSkill,
			int specialToHit,
			int specialToDamage)
		{
			ability = ability is null ? attacker.DefaultAbility() : ability;

			var defenseBase = defensiveSkill is null ? target.DefaultDefenseBase(ability) : target.SkillBase(defensiveSkill.Skill);

			specialToHit = aimedPart is null ? specialToHit : specialToHit - aimedPart.HitPenalty;

			aimedPart = AimedPartIsNullOrDismembered() ? DefaultCreaturePart() : aimedPart;

			CheckHowManyAttackCanBePerformedThisTurn();

			return new AttackData()
			{
				Attacker = attacker,
				Target = target,
				Ability = ability,
				AimedPart = aimedPart,
				DefenseBase = defenseBase,
				ToHit = specialToHit,
				ToDamage = specialToDamage
			};

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

			void CheckHowManyAttackCanBePerformedThisTurn()
			{
				if (attacker.Turn.TurnState >= BaseData.Enums.TurnState.BaseActionIsDone)
					specialToHit -= 3;

				if (ability.AttackSpeed > 1)
				{
					if (attacker.Turn.TurnState == BaseData.Enums.TurnState.ReadyForAction
						|| attacker.Turn.TurnState == BaseData.Enums.TurnState.BaseActionIsDone)
					{
						attacker.Turn.MuitiattackAbilityId = ability.Id;
						attacker.Turn.MultiattackRemainsQuantity = ability.AttackSpeed;
						attacker.Turn.TurnState++;
					}
					else if (ability.Id != attacker.Turn?.MuitiattackAbilityId)
						throw new LogicBaseException("Должна быть проведена мультиатака, но выбрана другая способность");

					attacker.Turn.MultiattackRemainsQuantity--;

					if (attacker.Turn.MultiattackRemainsQuantity > 0)
						return;

					attacker.Turn.MuitiattackAbilityId = null;
				}

				attacker.Turn.TurnState = attacker.Turn.TurnState == BaseData.Enums.TurnState.ReadyForAction
					? BaseData.Enums.TurnState.BaseActionIsDone
					: BaseData.Enums.TurnState.TurnIsDone;
			}
		}
	}
}
