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
			BodyTemplatePart aimedPart = default,
			Ability ability = default,
			int specialToHit = default,
			int specialToDamage = default)
		{
			var hitPenalty = aimedPart == default ? 0 : aimedPart.HitPenalty;

			aimedPart = aimedPart ?? target.BodyTemplate.DefaultBodyTemplatePart();

			ability = ability ?? monster.DefaultAbility();
			var attackValue = monster.ParameterBase(ability.AttackParameterId) + ability.Accuracy - hitPenalty + specialToHit;

			var successValue = _rollService.RollAttack(
				attackValue: attackValue < 0 ? 0 : attackValue,
				defenseValue: defenseValue);

			var message = new StringBuilder($"{monster.Name} атакует способностью {ability.Name} в {aimedPart.Name}.");
			if (successValue > 0)
			{
				message.AppendLine($"Попадание с превышением на {successValue}.");
				message.AppendLine($"Нанеcено {ability.RollDamage(specialToDamage)}. Модификатор урона после поглощения броней составляет {aimedPart.DamageModifier}.");
				foreach (var condition in ability.RollConditions())
					message.AppendLine($"Наложено состояние {condition.Name}.");
				if (successValue > 6)
					CheckCrit(message, successValue, aimedPart, target.CreatureType);
			}
			else if (successValue < -5)
				message.AppendLine($"Критический промах {successValue}.");
			else
				message.AppendLine("Промах.");

			return message.ToString();
		}

		private void CheckCrit(StringBuilder message, int successValue, BodyTemplatePart bodyTemplatePart, CreatureType creatureType = default)
		{
			int bonusDamage;
			string critSeverity;
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

			string critName = critSeverity + bodyTemplatePart.BodyPartType.Name;
			if (bodyTemplatePart.BodyPartType.Name == BodyPartTypes.HeadName || bodyTemplatePart.BodyPartType.Name == BodyPartTypes.TorsoName)
			{
				Random random = new Random();
				var suffix = random.Next(1, 6) < 5 ? 1 : 2;
				critName += suffix;
			}

			//Призраки и элементали не могут получать некоторые криты
			if ((creatureType?.Id == CreatureTypes.SpecterId || creatureType?.Id == CreatureTypes.ElementaId) && critName.Contains("Torso"))
			{
				if (critName.Contains("SimpleTorso1"))
					bonusDamage += 5;
				else if (critName.Contains("ComplexTorso2"))
					bonusDamage += 10;
				else if (critName.Contains("DifficultTorso"))
					bonusDamage += 15;
				else if (critName.Contains("DeadlyTorso1"))
					bonusDamage += 20;

				message.AppendLine($"Критическое повреждение не может быть нанесено из-за особенностей существа. Бонусный урон равен {bonusDamage}.");
				return;
			}

			var name = typeof(Crit).GetField(critName).GetValue(critName);
			message.AppendLine($"Нанесено критическое повреждение {name}. Бонусный урон равен {bonusDamage}.");
		}
	}
}
