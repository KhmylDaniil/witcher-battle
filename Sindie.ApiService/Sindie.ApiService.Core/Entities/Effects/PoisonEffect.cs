using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Эффект отравления
	/// </summary>
	public sealed class PoisonEffect : Effect
	{
		private PoisonEffect() { }

		/// <summary>
		/// Конструктор эффекта отравления
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		private PoisonEffect(Creature creature, string name) : base(creature, name) { }

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="attacker">Атакующий</param>
		/// <param name="target">Цель</param>
		/// <param name="name">Название</param>
		/// <returns>Эффект</returns>
		public static PoisonEffect Create(IRollService rollService, Creature attacker, Creature target, string name)
			=> target.Effects.Any(x => x is PoisonEffect)
				? null
				: new PoisonEffect(target, name);

		/// <summary>
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Run(Creature creature, ref StringBuilder message)
		{
			creature.HP -= 3;
			message.AppendLine($"Существо {creature.Name} потеряло 3 хита из-за отравления. Осталось {creature.HP} хитов.");
		}

		/// <summary>
		/// Попробовать снять эффект
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="healer">Лекарь</param>
		/// <param name="patient">Цель</param>
		/// <param name="message">Сообщение</param>
		public override void Treat(IRollService rollService, Creature healer, Creature patient, ref StringBuilder message)
		{
			if (rollService.BeatDifficulty(patient.SkillBase(Skill.Physique), 15))
			{
				message.AppendFormat($"Эффект {Name} снят.");
				patient.Effects.Remove(this);
			}
			else
				message.AppendLine($"Не удалось снять эффект {Name}.");
		}
	}
}
