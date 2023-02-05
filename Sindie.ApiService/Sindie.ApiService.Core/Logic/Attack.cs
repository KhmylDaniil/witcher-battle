using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Entities.Effects;
using Sindie.ApiService.Core.Requests.BattleRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Logic
{
	/// <summary>
	/// Действие атаки
	/// </summary>
	public sealed class Attack
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
		/// Монстр атакует с учетом защиты извне БД
		/// </summary>
		/// <param name="data">Данные для расчета атаки</param>
		/// <param name="defenseValue">Значение защиты</param>
		/// <returns></returns>
		internal string MonsterAttack(
			AttackData data,
			int defenseValue)
		{
			var message = new StringBuilder($"{data.Attacker.Name} атакует существо {data.Target.Name} способностью {data.Ability.Name} в {data.AimedPart.Name}. ");

			var successValue = _rollService.BeatDifficultyWithFumble(
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

			ApplyDamage(data, ref message, successValue);

			if (!CheckDead(data, ref message))
				ApplyConditions(data, ref message);

			return message.ToString();
		}

		/// <summary>
		/// Монстр получает урон извне БД
		/// </summary>
		/// <param name="data">Данные атаки</param>
		/// <param name="damage">Значение урона</param>
		/// <param name="successValue">Успешность атаки</param>
		/// <returns>Сообщение о результате атаки</returns>
		internal string MonsterSuffer(
			AttackData data,
			int damage,
			int successValue)
		{
			var message = new StringBuilder($"{data.Attacker.Name} атакует существо {data.Target.Name} способностью {data.Ability.Name} в {data.AimedPart.Name}.");

			ApplyDamage(data, ref message, successValue, damage);

			if (!CheckDead(data, ref message))
				ApplyConditions(data, ref message);

			return message.ToString();
		}

		/// <summary>
		/// Существо атакует полностью из БД в БД
		/// </summary>
		/// <param name="data">Данные для расчета атаки</param>
		/// <returns></returns>
		internal string CreatureAttack(AttackData data)
		{
			var message = new StringBuilder($"{data.Attacker.Name} атакует существо {data.Target.Name} способностью {data.Ability.Name} в {data.AimedPart.Name}.");

			var successValue = _rollService.ContestRollWithFumble(
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

			ApplyDamage(data, ref message, successValue);

			if (!CheckDead(data, ref message))
				ApplyConditions(data, ref message);

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
			var staggeredModifier = attacker.Effects.FirstOrDefault(x => x is StaggeredEffect) is null
				? 0
				: StaggeredEffect.AttackAndDefenseModifier;

			var blindedModifier = attacker.Effects.FirstOrDefault(x => x is BlindedEffect) is null
				? 0
				: BlindedEffect.AttackAndDefenseModifier;

			var result = attacker.SkillBase(ability.AttackSkill) + ability.Accuracy + toHit + staggeredModifier + blindedModifier;

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
			var staggeredModifier = defender.Effects.FirstOrDefault(x => x is StaggeredEffect) is null
				? 0
				: StaggeredEffect.AttackAndDefenseModifier;

			var blindedModifier = defender.Effects.FirstOrDefault(x => x is BlindedEffect) is null
				? 0
				: BlindedEffect.AttackAndDefenseModifier;

			var result = defender.SkillBase(defensiveParameter.Skill) + staggeredModifier + blindedModifier;

			if (defender.Effects.Any(x => x is StunEffect))
				result = 10;

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
				var condition = data.Conditions.FirstOrDefault(x => x.Name.Equals(critName));

				var effect = CritEffect.CreateCritEffect<Effect>(data.Target, data.AimedPart, condition);

				if (effect != null)
					data.Target.Effects.Add(effect);

				StunCheck(data.Target, ref message);
				
				message.AppendLine($"Нанесено критическое повреждение {effect.Name}.");
			}

			static void SetCritSeverity(int successValue, ref int bonusDamage, out string critSeverity)
			{
				if (successValue < 10)
				{
					critSeverity = Enums.Severity.Simple.ToString();
					bonusDamage += 3;
				}
				else if (successValue < 13)
				{
					critSeverity = Enums.Severity.Complex.ToString();
					bonusDamage += 5;
				}
				else if (successValue < 15)
				{
					critSeverity = Enums.Severity.Difficult.ToString();
					bonusDamage += 8;
				}
				else
				{
					critSeverity = Enums.Severity.Deadly.ToString();
					bonusDamage += 10;
				}
			}

			static string CritName(CreaturePart creaturePart, string critSeverity)
			{
				string critName = critSeverity + Enum.GetName(creaturePart.BodyPartType);
				if (creaturePart.BodyPartType == BodyPartType.Head || creaturePart.BodyPartType == BodyPartType.Torso)
				{
					Random random = new ();
					var suffix = random.Next(1, 6) < 5 ? 1 : 2;
					critName += suffix;
				}

				return critName;
			}

			static bool isCritImmune(CreatureType creatureType, CreaturePart aimedPart)
			{
				return (creatureType == CreatureType.Specter || creatureType == CreatureType.Elementa)
					&& aimedPart.BodyPartType == BodyPartType.Torso;
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

			void StunCheck(Creature creature, ref StringBuilder message)
			{
				Random random = new();
				if (random.Next(1, 10) >= creature.Stun)
				{
					var stun = StunEffect.Create(null, null, target: creature, "Crit-based Stun");

					if (stun is null)
						return;

					creature.Effects.Add(stun);
					message.AppendLine($"Из-за наложения критического эффекта наложен эффект {Conditions.StunName}.");
				}	
			}
		}

		/// <summary>
		/// Удаление мертвых существ
		/// </summary>
		/// <param name="instance"></param>
		internal static void DisposeCorpses(Battle instance)
		{
			instance.Creatures.RemoveAll(x => x is not Character
			&& (x.HP <= 0 || x.Effects.Any(x => x is DeadEffect)));
		}

		private static string CritMissMessage(int attackerFumble)
		{
			return $"Критический промах {attackerFumble}.";
		}

		/// <summary>
		/// Применить урон
		/// </summary>
		/// <param name="data">Данные для расчета атаки</param>
		/// <param name="message">Сообщение</param>
		/// <param name="successValue">Успешность атаки</param>
		private static void ApplyDamage(AttackData data, ref StringBuilder message, int successValue)
		{
			message.AppendLine($"Попадание с превышением на {successValue}.");
			RemoveStunEffect(data);

			int damage = RollDamage(data.Ability, data.ToDamage);

			ArmorMutigation(data, ref damage, ref message);

			CheckModifiers(data, ref damage);

			CheckCrit(data, ref message, successValue, ref damage);

			data.Target.HP -= damage;
			message.AppendLine($"Нанеcено {damage} урона.");

			if (damage > 0)
				CheckDying(data.Target, ref message);
		}

		private static void RemoveStunEffect(AttackData data)
		{
			var stun = data.Target.Effects.FirstOrDefault(x => x is StunEffect);
			if (stun != null)
				data.Target.Effects.Remove(stun);
		}

		/// <summary>
		/// Применить урон
		/// </summary>
		/// <param name="data">Данные для расчета атаки</param>
		/// <param name="message">Сообщение</param>
		/// <param name="successValue">Успешность атаки</param>
		/// <param name="damage">Урон</param>
		private static void ApplyDamage(AttackData data, ref StringBuilder message, int successValue, int damage)
		{
			message.AppendLine($"Попадание с превышением на {successValue}.");

			ArmorMutigation(data, ref damage, ref message);

			CheckModifiers(data, ref damage);

			CheckCrit(data, ref message, successValue, ref damage);

			message.AppendLine($"Нанеcено {damage} урона.");
			data.Target.HP -= damage;

			if (damage > 0)
				CheckDying(data.Target, ref message);
		}

		/// <summary>
		/// Проверка смерти
		/// </summary>
		/// <param name="data">Данные</param>
		/// <param name="message">Сообщение</param>
		private static bool CheckDead(AttackData data, ref StringBuilder message)
		{
			if (data.Target.HP >= 0)
				return false;

			if (data.Target is Character)
			{
				data.Target.Effects.Add(DyingEffect.Create(null, null, data.Target, Conditions.DyingName));
				message.AppendLine($"Существо {data.Target.Name} при смерти");
				return false;
			}

			message.AppendLine($"Существо {data.Target.Name} погибает");
			return true;
		}

		private static void ArmorMutigation(AttackData data, ref int damage, ref StringBuilder message)
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
		}

		private static void CheckModifiers(AttackData data, ref int damage)
		{
			damage = (int)Math.Truncate(damage * data.AimedPart.DamageModifier);

			var damageTypeModifier = data.Target.DamageTypeModifiers.FirstOrDefault(x => x.DamageType == data.Ability.DamageType);

			if (damageTypeModifier is null) return;

			damage = damageTypeModifier.DamageTypeModifier switch
			{
				DamageTypeModifier.Vulnerability => damage * 2,
				DamageTypeModifier.Resistance => damage / 2,
				DamageTypeModifier.Immunity => 0,
				DamageTypeModifier.Normal => damage,
				_ => throw new ArgumentException("DamageType enum value is not defined"),
			};
		}

		/// <summary>
		/// Расчет урона от атаки
		/// </summary>
		/// <returns>Нанесенный урон</returns>
		private static int RollDamage(Ability ability, int specialBonus = default)
		{
			Random random = new ();
			var result = ability.DamageModifier + specialBonus;
			for (int i = 0; i < ability.AttackDiceQuantity; i++)
				result += random.Next(1, 6);
			return result < 0 ? 0 : result;
		}

		/// <summary>
		/// Применение состояний
		/// </summary>
		/// <param name="data">Данные для атаки</param>
		/// <param name="message">Сообщение</param>
		private void ApplyConditions(AttackData data, ref StringBuilder message)
		{
			foreach (var condition in RollConditions(data.Ability))
			{
				var effect = Effect.CreateEffect<Effect>(rollService: _rollService, data.Attacker, data.Target, condition);

				if (effect == null)
					continue;

				data.Target.Effects.Add(effect);

				message.AppendLine($"Наложен {effect.Name}");
			}
		}

		/// <summary>
		/// Расчет применения состояний
		/// </summary>
		/// <returns>Наложенные состояния</returns>
		private static List<Condition> RollConditions(Ability ability)
		{
			var result = new List<Condition>();
			Random random = new ();

			if (!ability.AppliedConditions.Any())
				return result;

			foreach (var condition in ability.AppliedConditions)
				if (random.Next(1, 100) <= condition.ApplyChance)
					result.Add(condition.Condition);

			return result;
		}

		/// <summary>
		/// Проверка атаки по умирающим
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		private static void CheckDying(Creature creature, ref StringBuilder message)
		{
			var dying = creature.Effects.FirstOrDefault(x => x is DyingEffect) as DyingEffect;

			dying?.Run(creature, ref message);
		}
	}
}
