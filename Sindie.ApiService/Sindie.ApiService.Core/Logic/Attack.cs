using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using System;
using System.Text;

namespace Sindie.ApiService.Core.Logic
{
	/// <summary>
	/// Атака
	/// </summary>
	public class Attack
	{
		private readonly IRollService _rollService;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="rollService"></param>
		public Attack(IRollService rollService)
		{
			_rollService = rollService;
		}

		/// <summary>
		/// Атака монсстра
		/// </summary>
		/// <param name="monster">Монстр</param>
		/// <param name="target">Цель</param>
		/// <param name="defenseValue">Значение защиты</param>
		/// <param name="aimedPart">Прицел в часть тела</param>
		/// <param name="ability">Способность</param>
		/// <param name="specialToHit">Специальный бонус к попаданию</param>
		/// <param name="specialToDamage">Специальный бонус к урону</param>
		/// <returns></returns>
		public string MonsterAttack(
			Creature monster,
			Creature target,
			int defenseValue,
			CreaturePart aimedPart,
			Ability ability,
			int specialToHit,
			int specialToDamage)
		{
			var hitPenalty = aimedPart == default ? 0 : aimedPart.HitPenalty;

			aimedPart = aimedPart ?? target.DefaultCreaturePart();

			ability = ability ?? monster.DefaultAbility();

			var successValue = _rollService.RollAttack(
				attackValue: AttackValue(monster, ability, specialToHit),
				defenseValue: defenseValue);

			var message = new StringBuilder($"{monster.Name} атакует существо {target.Name} способностью {ability.Name} в {aimedPart.Name}.");
			if (successValue > 0)
			{
				message.AppendLine($"Попадание с превышением на {successValue}.");
				message.Append($"Нанеcено {ability.RollDamage(specialToDamage)}. Модификатор урона после поглощения броней составляет {aimedPart.DamageModifier}.\n");
				foreach (var condition in ability.RollConditions())
					message.Append($"Наложено состояние {condition.Name}.\n");

				CheckCrit(message, successValue, aimedPart, out int bonusDamage, target.CreatureType);
			}
			else if (successValue < -5)
				message.Append($"Критический промах {successValue}.");
			else
				message.Append("Промах.");

			return message.ToString();
		}

		/// <summary>
		/// Расчет базы атаки
		/// </summary>
		/// <param name="monster">Атакующий</param>
		/// <param name="ability">Способность</param>
		/// <param name="specialToHit">Специальный бонус к попаданию</param>
		/// <param name="hitPenalty">Пенальти к попаданию</param>
		/// <returns>База атаки</returns>
		private static int AttackValue(Creature attacker, Ability ability, int toHit)
		{
			var result = attacker.ParameterBase(ability.AttackParameterId) + ability.Accuracy + toHit;
			return result < 0 ? 0 : result;
		}

		/// <summary>
		/// Расчет крита
		/// </summary>
		/// <param name="message">Сообщение</param>
		/// <param name="successValue">Успешность атаки</param>
		/// <param name="creaturePart">Часть тела существа</param>
		/// <param name="creatureType">Тип существа</param>
		private void CheckCrit(StringBuilder message, int successValue, CreaturePart creaturePart, out int bonusDamage, CreatureType creatureType = default)
		{
			bonusDamage = 0;
			if (successValue < 7)
				return;

			SetCritSeverity(successValue, out bonusDamage, out string critSeverity);

			string critName = CritName(creaturePart, critSeverity);

			if (isCritImmune(creatureType, creaturePart))
				message.AppendLine($"Критическое повреждение не может быть нанесено из-за особенностей существа. " +
					$"Бонусный урон равен {AddBonusDamage(bonusDamage, critName)}.");
			else
			{
				var name = typeof(Crit).GetField(critName).GetValue(critName);
				message.AppendLine($"Нанесено критическое повреждение {name.ToString()}. Бонусный урон равен {bonusDamage}.");

			}

			static void SetCritSeverity(int successValue, out int bonusDamage, out string critSeverity)
			{
				if (successValue < 10)
				{
					critSeverity = "Simple";
					bonusDamage = 3;
				}
				else if (successValue < 13)
				{
					critSeverity = "Complex";
					bonusDamage = 5;
				}
				else if (successValue < 15)
				{
					critSeverity = "Difficult";
					bonusDamage = 8;
				}
				else
				{
					critSeverity = "Deadly";
					bonusDamage = 10;
				}
			}

			static string CritName(CreaturePart creaturePart, string critSeverity)
			{
				string critName = critSeverity + creaturePart.BodyPartType.Name;
				if (creaturePart.BodyPartType.Name == BodyPartTypes.HeadName || creaturePart.BodyPartType.Name == BodyPartTypes.TorsoName)
				{
					Random random = new Random();
					var suffix = random.Next(1, 6) < 5 ? 1 : 2;
					critName += suffix;
				}

				return critName;
			}

			static bool isCritImmune(CreatureType creatureType, CreaturePart aimedPart)
			{
				return (creatureType?.Id == CreatureTypes.SpecterId || creatureType?.Id == CreatureTypes.ElementaId)
					&& aimedPart.BodyPartTypeId == BodyPartTypes.TorsoId;
			}

			static int AddBonusDamage(int bonusDamage, string critName)
			{
				if (critName.Equals("SimpleTorso1", StringComparison.OrdinalIgnoreCase))
					bonusDamage += 5;
				else if (critName.Equals("ComplexTorso2", StringComparison.OrdinalIgnoreCase))
					bonusDamage += 10;
				else if (critName.Equals("DifficultTorso", StringComparison.OrdinalIgnoreCase))
					bonusDamage += 15;
				else if (critName.Equals("DeadlyTorso1", StringComparison.OrdinalIgnoreCase))
					bonusDamage += 20;
				return bonusDamage;
			}
		}

		/// <summary>
		/// Получение монстром урона
		/// </summary>
		/// <param name="monster">Монстр</param>
		/// <param name="aimedPart">Часть тела существа</param>
		/// <param name="damageValue">Значение урона</param>
		/// <param name="successValue">Успешность атаки</param>
		/// <param name="isResistant">Сопротивление урону</param>
		/// <param name="isVulnerable">Уязвимость к урону</param>
		/// <returns>Сообщение о результате атаки</returns>
		public string MonsterSuffer(
			ref Creature monster,
			CreaturePart aimedPart,
			int damageValue,
			int successValue,
			bool isResistant,
			bool isVulnerable)
		{
			aimedPart = aimedPart ?? monster.DefaultCreaturePart();

			var message = new StringBuilder($"{monster.Name} атакован на {damageValue} в {aimedPart.Name}.");

			if (aimedPart.CurrentArmor > 0)
			{
				if (aimedPart.CurrentArmor < damageValue)
				{
					damageValue -= aimedPart.CurrentArmor--;
					message.AppendLine("Броня повреждена");
				}
				else
				{
					damageValue = 0;
					message.AppendLine("Броня поглотила урон");
				}
			}
		
			if (isResistant)
				damageValue /= 2;
			if (isVulnerable)
				damageValue *= 2;

			CheckCrit(message, successValue, aimedPart, out int bonusDamage);

			damageValue += bonusDamage;
			
			message.Append($"{monster.Name} получает {damageValue} урона.");

			monster.HP = monster.HP > damageValue
				? monster.HP - damageValue
				: 0;

			if (monster.HP == 0)
				message.Append($"{monster.Name} погибает.");
			else
				message.Append($"{monster.Name} остается с {monster.HP} хитов.");

			return message.ToString();
		}













		public string CreatureAttack(
			Creature attacker,
			Creature target,
			CreaturePart aimedPart,
			Ability ability,
			int toHit,
			int specialToDamage = default)
		{

			var successValue = _rollService.RollAttack(
				attackValue: AttackValue(attacker, ability, specialToHit, hitPenalty),
				defenseValue: DefenseValue(target, ability, ));




		}

		/// <summary>
		/// Определение базы защиты
		/// </summary>
		/// <param name="defender">Защищающееся существо</param>
		/// <param name="ability">Способность атаки</param>
		/// <param name="defensiveParameter">Защитный параметр</param>
		/// <returns>База защиты</returns>
		private static int DefenseValue(Creature defender, Ability ability, Parameter defensiveParameter = default)
		{
			defensiveParameter = defensiveParameter == default ? defender.DefaultDefensiveParameter(ability) : defensiveParameter;
			var result = defender.ParameterBase(defensiveParameter.Id);
			return result < 0 ? 0 : result;
		}
	}
}
