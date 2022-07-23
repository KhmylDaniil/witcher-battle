using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Requests.BattleRequests;
using System;
using System.Collections.Generic;
using System.Linq;
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
		/// <param name="data">Данные для расчета атаки</param>
		/// <param name="defenseValue">Значение защиты</param>
		/// <returns></returns>
		internal string MonsterAttack(
			AttackData data,
			int defenseValue)
		{
			var message = new StringBuilder($"{data.Attacker.Name} атакует существо {data.Target.Name} способностью {data.Ability.Name} в {data.AimedPart.Name}.");

			var successValue = _rollService.BeatDifficulty(
				skillBase: AttackValue(data.Attacker, data.Ability, data.ToHit),
				difficuty: defenseValue,
				out int attackerFumble);

			if (attackerFumble != 0)
			{
				message.AppendLine(CritMissMessage(attackerFumble)); //TODO
				return message.ToString();
			}
			if (successValue == 0)
			{
				message.AppendLine("Промах.");
				return message.ToString();
			}

			ApplyDamage(ref data, ref message, successValue);

			if (data.Target.HP == 0)
				return message.ToString();

			ApplyConditions(ref data, ref message);

			return message.ToString();
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
		internal string MonsterSuffer(
			ref AttackData data,
			int damage,
			int successValue)
		{
			var message = new StringBuilder($"{data.Attacker.Name} атакует существо {data.Target.Name} способностью {data.Ability.Name} в {data.AimedPart.Name}.");

			ApplyDamage(ref data, ref message, successValue, damage);

			if (data.Target.HP == 0)
				return message.ToString();

			ApplyConditions(ref data, ref message);

			return message.ToString();
		}

		internal string CreatureAttack(ref AttackData data)
		{
			var message = new StringBuilder($"{data.Attacker.Name} атакует существо {data.Target.Name} способностью {data.Ability.Name} в {data.AimedPart.Name}.");

			var successValue = _rollService.ContestRoll(
				attackBase: AttackValue(data.Attacker, data.Ability, data.ToHit),
				defenseBase: DefenseValue(data.Target, data.DefensiveSkill),
				out int attackerFumble,
				out int defenderFumble);

			//ApplyFumble(attackerFumble); TODO

			if (attackerFumble != 0)
			{
				message.AppendLine(CritMissMessage(attackerFumble)); //TODO
				return message.ToString();
			} 
			if (successValue == 0)
			{
				message.AppendLine("Промах.");
				return message.ToString();
			}

			ApplyDamage(ref data, ref message, successValue);

			if (data.Target.HP == 0)
				return message.ToString();

			ApplyConditions(ref data, ref message);

			return message.ToString();
		}

		/// <summary>
		/// Расчет базы атаки
		/// </summary>
		/// <param name="attacker">Атакующий</param>
		/// <param name="ability">Способность</param>
		/// <param name="toHit">Модификатор к попаданию</param>
		/// <returns>База атаки</returns>
		private static int AttackValue(Creature attacker, Ability ability, int toHit)
		{
			var result = attacker.SkillBase(ability.AttackSkillId) + ability.Accuracy + toHit;
			return result < 0 ? 0 : result;
		}

		/// <summary>
		/// Определение базы защиты
		/// </summary>
		/// <param name="defender">Защищающееся существо</param>
		/// <param name="defensiveParameter">Защитный параметр</param>
		/// <returns>База защиты</returns>
		private static int DefenseValue(Creature defender, CreatureSkill defensiveParameter)
		{
			var result = defender.SkillBase(defensiveParameter.SkillId);
			return result < 0 ? 0 : result;
		}

		/// <summary>
		/// Расчет крита
		/// </summary>
		/// <param name="message">Сообщение</param>
		/// <param name="successValue">Успешность атаки</param>
		/// <param name="creaturePart">Часть тела существа</param>
		/// <param name="creatureType">Тип существа</param>
		private static void CheckCrit(AttackData data, ref StringBuilder message, int successValue, ref int damage)
		{
			if (successValue < 7)
				return;

			SetCritSeverity(successValue, ref damage, out string critSeverity);

			string critName = CritName(data.AimedPart, critSeverity);

			if (isCritImmune(data.Target.CreatureType, data.AimedPart))
			{
				message.AppendLine($"Критическое повреждение не может быть нанесено из-за особенностей существа.");
				AddBonusDamage(ref damage, critName);
			}
				
			else
			{
				var name = typeof(Crit).GetField(critName).GetValue(critName);
				message.AppendLine($"Нанесено критическое повреждение {name.ToString()}.");
			}

			static void SetCritSeverity(int successValue, ref int bonusDamage, out string critSeverity)
			{
				if (successValue < 10)
				{
					critSeverity = "Simple";
					bonusDamage += 3;
				}
				else if (successValue < 13)
				{
					critSeverity = "Complex";
					bonusDamage += 5;
				}
				else if (successValue < 15)
				{
					critSeverity = "Difficult";
					bonusDamage += 8;
				}
				else
				{
					critSeverity = "Deadly";
					bonusDamage += 10;
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

			static void AddBonusDamage(ref int bonusDamage, string critName)
			{
				if (critName.Equals("SimpleTorso1", StringComparison.OrdinalIgnoreCase))
					bonusDamage += 5;
				else if (critName.Equals("ComplexTorso2", StringComparison.OrdinalIgnoreCase))
					bonusDamage += 10;
				else if (critName.Equals("DifficultTorso", StringComparison.OrdinalIgnoreCase))
					bonusDamage += 15;
				else if (critName.Equals("DeadlyTorso1", StringComparison.OrdinalIgnoreCase))
					bonusDamage += 20;
			}
		}

		/// <summary>
		/// Удаление мертвых существ
		/// </summary>
		/// <param name="instance"></param>
		internal static void DisposeCorpses(ref Battle instance)
		{
			instance.Creatures.RemoveAll(x => x.HP == 0);
		}

		private string CritMissMessage(int attackerFumble)
		{
			return $"Критический промах {attackerFumble}.";
		}

		/// <summary>
		/// Применить урон
		/// </summary>
		/// <param name="data">Данные для расчета атаки</param>
		/// <param name="message">Сообщение</param>
		/// <param name="successValue">Успешность атаки</param>
		private static void ApplyDamage(ref AttackData data, ref StringBuilder message, int successValue)
		{
			message.AppendLine($"Попадание с превышением на {successValue}.");

			int damage = RollDamage(data.Ability, data.ToDamage);

			ArmorMutigation(ref data, ref damage, ref message);

			CheckModifiers(data, ref damage);

			CheckCrit(data, ref message, successValue, ref damage);

			message.AppendLine($"Нанеcено {damage} урона.");
			CheckDead(ref data, ref message, damage);
		}

		/// <summary>
		/// Применить урон
		/// </summary>
		/// <param name="data">Данные для расчета атаки</param>
		/// <param name="message">Сообщение</param>
		/// <param name="successValue">Успешность атаки</param>
		/// <param name="damage">Урон</param>
		private static void ApplyDamage(ref AttackData data, ref StringBuilder message, int successValue, int damage)
		{
			message.AppendLine($"Попадание с превышением на {successValue}.");

			ArmorMutigation(ref data, ref damage, ref message);

			CheckModifiers(data, ref damage);

			CheckCrit(data, ref message, successValue, ref damage);

			message.AppendLine($"Нанеcено {damage} урона.");
			CheckDead(ref data, ref message, damage);
		}


		/// <summary>
		/// Проверка смерти
		/// </summary>
		/// <param name="data">Данные</param>
		/// <param name="message">Сообщение</param>
		/// <param name="damage">Урон</param>
		private static void CheckDead(ref AttackData data, ref StringBuilder message, int damage)
		{
			data.Target.HP = data.Target.HP - damage > 0
							? data.Target.HP - damage
							: 0;

			if (data.Target.HP == 0)
				message.AppendLine($"Существо {data.Target.Name} погибает");
		}

		private static void ApplyConditions(ref AttackData data, ref StringBuilder message)
		{
			foreach (var condition in RollConditions(data.Ability))
			{
				data.Target.Conditions.Add(condition);
				message.AppendLine($"Наложено состояние {condition.Name}.");
			}
		}
		private static void ArmorMutigation(ref AttackData data, ref int damage, ref StringBuilder message)
		{
			if (data.AimedPart.CurrentArmor == 0)
				return;

			if (data.AimedPart.CurrentArmor > damage)
			{
				damage = 0;
				message.AppendLine("Броня поглотила урон");
				return;
			}

			damage -= data.AimedPart.CurrentArmor--;
			message.AppendLine($"Броня повреждена. Осталось {data.AimedPart.CurrentArmor} брони");
			return;
		}

		private static void CheckModifiers(AttackData data, ref int damage)
		{
			if (data.Target.Resistances.Any(x => data.Ability.DamageTypes.All(y => x.Id == y.Id)))
				damage /= 2;

			if (data.Target.Vulnerables.Any(x => data.Ability.DamageTypes.All(y => x.Id == y.Id)))
				damage *= 2;

			damage = (int)Math.Truncate(damage * data.AimedPart.DamageModifier);
		}

		/// <summary>
		/// Расчет урона от атаки
		/// </summary>
		/// <returns>Нанесенный урон</returns>
		private static int RollDamage(Ability ability, int specialBonus = default)
		{
			Random random = new Random();
			var result = ability.DamageModifier + specialBonus;
			for (int i = 0; i < ability.AttackDiceQuantity; i++)
				result += random.Next(1, 6);
			return result < 0 ? 0 : result;
		}

		/// <summary>
		/// Расчет применения состояний
		/// </summary>
		/// <returns>Наложенные состояния</returns>
		private static List<Condition> RollConditions(Ability ability)
		{
			var result = new List<Condition>();
			Random random = new Random();

			if (!ability.AppliedConditions.Any())
				return result;

			foreach (var condition in ability.AppliedConditions)
				if (random.Next(1, 100) <= condition.ApplyChance)
					result.Add(condition.Condition);

			return result;
		}
	}
}
