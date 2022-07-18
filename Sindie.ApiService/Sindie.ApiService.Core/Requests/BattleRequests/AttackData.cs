using Sindie.ApiService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		/// Часть тела цель
		/// </summary>
		internal CreaturePart AimedPart { get; private set; }

		/// <summary>
		/// Параметр защиты
		/// </summary>
		internal CreatureParameter DefensiveParameter { get; private set; }

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
		/// <param name="defensiveParameter">Параметр защиты</param>
		/// <param name="specialToHit">Специальный бонус к попаданию</param>
		/// <param name="specialToDamage">Специальный бонус к урону</param>
		/// <returns>Данные для расчета атаки</returns>
		internal static AttackData CreateData(
			Creature attacker,
			Creature target,
			CreaturePart aimedPart,
			Ability ability,
			CreatureParameter defensiveParameter,
			int specialToHit,
			int specialToDamage)
		{
			ability = ability is null ? attacker.DefaultAbility() : ability;
			
			defensiveParameter = defensiveParameter is null ? target.DefaultDefensiveParameter(ability) : defensiveParameter;

			specialToHit = aimedPart is null ? specialToHit : specialToHit - aimedPart.HitPenalty;
			
			aimedPart = aimedPart is null ? target.DefaultCreaturePart() : aimedPart;
			
			return new AttackData()
			{
				Attacker = attacker,
				Target = target,
				Ability = ability,
				AimedPart = aimedPart,
				DefensiveParameter = defensiveParameter,
				ToHit = specialToHit,
				ToDamage = specialToDamage
			};
		}
	}
}
