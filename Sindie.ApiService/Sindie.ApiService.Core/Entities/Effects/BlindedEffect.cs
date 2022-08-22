﻿using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using System;
using System.Linq;
using System.Text;

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
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Run(Creature creature, ref StringBuilder message) { }

		/// <summary>
		/// Автоматически прекратить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void AutoEnd(Creature creature, ref StringBuilder message) { }

		/// <summary>
		/// Попробовать снять эффект
		/// </summary>
		/// <param name="rollService">Сервис бросков</param>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Treat(IRollService rollService, Creature creature, ref StringBuilder message)
		{
			message.AppendFormat($"Эффект {Conditions.BlindedName} снят.");
			RevertStatChanges(creature);
			creature.Effects.Remove(this);
		}

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		private void ApplyStatChanges(Creature creature)
		{
			var awareness = creature.CreatureSkills.FirstOrDefault(x => x.SkillId == Skills.AwarenessId);

			if (awareness != null)
				awareness.SkillValue = awareness.GetValue() + AwarenessModifier;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		private void RevertStatChanges(Creature creature)
		{
			var awareness = creature.CreatureSkills.FirstOrDefault(x => x.SkillId == Skills.AwarenessId);

			if (awareness != null)
				awareness.SkillValue = awareness.GetValue() - AwarenessModifier;
		}
	}
}