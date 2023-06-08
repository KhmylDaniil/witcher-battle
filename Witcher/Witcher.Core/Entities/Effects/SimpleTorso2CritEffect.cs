using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Logic;
using System.Text;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - треснувшие ребра
	/// </summary>
	public class SimpleTorso2CritEffect : CritEffect, ICrit
	{
		private const int BodyModifier = -2;
		private const int AfterTreatBodyModifier = -1;

		public SimpleTorso2CritEffect() { }

		/// <summary>
		/// Конструктор эффекта треснувших ребер
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private SimpleTorso2CritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			ApplyStatChanges(creature);
			Severity = Severity.Simple | Severity.Unstabilizied;
			BodyPartLocation = Enums.BodyPartType.Torso;
		}

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static SimpleTorso2CritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			CheckExistingEffectAndRemoveStabilizedEffect<SimpleTorso2CritEffect>(creature, aimedPart);
			return new SimpleTorso2CritEffect(creature, aimedPart, name);
		}

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature)
			=> creature.Body = creature.GetBody() + BodyModifier;

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature)
		{
			if (Severity == Severity.Simple)
				creature.Body = creature.GetBody() - AfterTreatBodyModifier;
			else
				creature.Body = creature.GetBody() - BodyModifier;
		}

		/// <summary>
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
			creature.Body = creature.GetBody() - BodyModifier;
			creature.Body = creature.GetBody() + AfterTreatBodyModifier;
		}
	}
}
