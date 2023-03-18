using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Logic;
using System.Linq;
using System.Text;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - сломанные ребра
	/// </summary>
	public class ComplexTorso1CritEffect : CritEffect, ICrit
	{
		private const int BodyModifier = -2;
		private const int AfterTreatBodyModifier = -1;
		private const int RefAndDexModifier = -1;

		public ComplexTorso1CritEffect() { }

		/// <summary>
		/// Конструктор эффекта сломанных ребер
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private ComplexTorso1CritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			ApplyStatChanges(creature);
			Severity = Severity.Complex | Severity.Unstabilizied;
			BodyPartLocation = Enums.BodyPartType.Torso;
		}

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static ComplexTorso1CritEffect Create(Creature creature, CreaturePart aimedPart, string name)
			=> CheckExistingEffectAndRemoveStabilizedEffect<ComplexTorso1CritEffect>(creature, aimedPart)
				? new ComplexTorso1CritEffect(creature, aimedPart, name)
				: null;

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature)
		{
			creature.Body = creature.GetBody() + BodyModifier;
			creature.Ref = creature.GetRef() + RefAndDexModifier;
			creature.Dex = creature.GetDex() + RefAndDexModifier;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature)
		{
			if (Severity == Severity.Complex)
				creature.Body = creature.GetBody() - AfterTreatBodyModifier;
			else
				creature.Body = creature.GetBody() - BodyModifier;

			creature.Ref = creature.GetRef() - RefAndDexModifier;
			creature.Dex = creature.GetDex() - RefAndDexModifier;
		}

		/// <summary>
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
			if (Severity == Severity.Complex)
				return;

			Severity = Severity.Complex;

			creature.Body = creature.GetBody() - BodyModifier;
			creature.Body = creature.GetBody() + AfterTreatBodyModifier;
		}
	}
}
