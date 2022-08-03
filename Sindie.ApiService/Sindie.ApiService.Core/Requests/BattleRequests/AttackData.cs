using Sindie.ApiService.Core.Entities;
using System.Collections.Generic;

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
			
			aimedPart = aimedPart is null ? target.DefaultCreaturePart() : aimedPart;
			
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
		}
	}
}
