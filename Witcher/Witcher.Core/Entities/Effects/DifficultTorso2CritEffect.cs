using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Logic;
using System.Linq;
using System.Text;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - рана в живот
	/// </summary>
	public class DifficultTorso2CritEffect : CritEffect, ICrit
	{
		private const int Modifier = -2;

		public DifficultTorso2CritEffect() { }

		/// <summary>
		/// Конструктор эффекта раны в живот
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private DifficultTorso2CritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			ApplyStatChanges(creature);
			Severity = Severity.Difficult | Severity.Unstabilizied;
			BodyPartLocation = Enums.BodyPartType.Torso;
		}

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static DifficultTorso2CritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			CheckExistingEffectAndRemoveStabilizedEffect<DifficultTorso2CritEffect>(creature, aimedPart);
			return new DifficultTorso2CritEffect(creature, aimedPart, name);
		}

		/// <summary>
		/// Применить эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="message">Сообщение</param>
		public override void Run(Creature creature, ref StringBuilder message)
		{
			if (!IsStabile(Severity))
			{
				creature.HP -= 4;
				message.AppendLine($"{Name} приводит к получению 4 урона от кислоты.");
			}
		}

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature)
		{
			foreach (var skill in creature.CreatureSkills)
				skill.SkillValue = skill.GetValue() + Modifier;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature)
		{
			foreach (var skill in creature.CreatureSkills)
				skill.SkillValue = skill.GetValue() - Modifier;
		}

		/// <summary>
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
			Severity = Severity.Difficult;
		}
	}
}
