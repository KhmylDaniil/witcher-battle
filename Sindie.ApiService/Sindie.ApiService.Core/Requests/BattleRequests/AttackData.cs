﻿using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sindie.ApiService.Core.Requests.BattleRequests
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
		/// Навык защиты
		/// </summary>
		internal CreatureSkill DefensiveSkill { get; private set; }

		/// <summary>
		/// Специальный бонус к попаданию
		/// </summary>
		internal int ToHit { get; private set; }

		/// <summary>
		/// Специальный бонус к урону
		/// </summary>
		internal int ToDamage { get; private set; }

		/// <summary>
		/// Состояния
		/// </summary>
		internal List<Condition> Conditions { get; private set; }

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
		/// <param name="conditions">Состояния</param>
		/// <returns>Данные для расчета атаки</returns>
		internal static AttackData CreateData(
			Creature attacker,
			Creature target,
			CreaturePart aimedPart,
			Ability ability,
			CreatureSkill defensiveSkill,
			int specialToHit,
			int specialToDamage,
			List<Condition> conditions)
		{
			ability = ability is null ? attacker.DefaultAbility() : ability;

			defensiveSkill = defensiveSkill is null ? target.DefaultDefensiveSkill(ability) : defensiveSkill;

			specialToHit = aimedPart is null ? specialToHit : specialToHit - aimedPart.HitPenalty;
			
			aimedPart = AimedPartIsNullOrDismembered() ? DefaultCreaturePart() : aimedPart;
			
			return new AttackData()
			{
				Attacker = attacker,
				Target = target,
				Ability = ability,
				AimedPart = aimedPart,
				DefensiveSkill = defensiveSkill,
				ToHit = specialToHit,
				ToDamage = specialToDamage,
				Conditions = conditions
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
		}
	}
}