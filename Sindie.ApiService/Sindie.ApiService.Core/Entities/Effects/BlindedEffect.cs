using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Logic;
using System;
using System.Linq;
using System.Text;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Эффект ослепления
	/// </summary>
	public sealed class BlindedEffect : Effect
	{
		private const int AwarenessModifier = -5;

		public const int AttackAndDefenseModifier = -3;

		private BlindedEffect() { }

		/// <summary>
		/// Конструктор эффекта ослепления
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		private BlindedEffect(Creature creature, string name) : base(creature, name)
			=> ApplyStatChanges(creature);

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="attacker">Атакующий</param>
		/// <param name="target">Цель</param>
		/// <param name="name">Название</param>
		/// <returns>Эффект</returns>
		public static BlindedEffect Create(IRollService rollService, Creature attacker, Creature target, string name)
			=> target.Effects.Any(x => x is BlindedEffect)
				? null
				: new BlindedEffect(target, name);

		/// <summary>
		/// Попробовать снять эффект
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="healer">Лекарь</param>
		/// <param name="patient">Цель</param>
		/// <param name="message">Сообщение</param>
		public override void Treat(IRollService rollService, Creature healer, Creature patient, ref StringBuilder message)
		{
			message.AppendFormat($"Эффект {Name} снят.");
			RevertStatChanges(patient);
			patient.Effects.Remove(this);
		}


		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		private void ApplyStatChanges(Creature creature)
		{
			var awareness = creature.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Awareness);

			if (awareness != null)
				awareness.SkillValue = awareness.GetValue() + AwarenessModifier;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		private void RevertStatChanges(Creature creature)
		{
			var awareness = creature.CreatureSkills.FirstOrDefault(x => x.Skill == Skill.Awareness);

			if (awareness != null)
				awareness.SkillValue = awareness.GetValue() - AwarenessModifier;
		}
	}
}
