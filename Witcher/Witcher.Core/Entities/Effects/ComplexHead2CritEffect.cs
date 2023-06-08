using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Logic;
using System.Linq;
using System.Text;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - Небольшая травма головы
	/// </summary>
	public class ComplexHead2CritEffect : CritEffect, ICrit
	{
		private const int Modifier = -1;

		private ComplexHead2CritEffect() { }

		/// <summary>
		/// Конструктор эффекта небольшой травмы головы
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private ComplexHead2CritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
			ApplyStatChanges(creature);
			Severity = Severity.Complex | Severity.Unstabilizied;
			BodyPartLocation = Enums.BodyPartType.Head;
		}

		/// <summary>
		/// Создание эффекта - синглтон
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		/// <returns>Эффект</returns>
		public static ComplexHead2CritEffect Create(Creature creature, CreaturePart aimedPart, string name)
		{
			CheckExistingEffectAndRemoveStabilizedEffect<ComplexHead2CritEffect>(creature, aimedPart);
			return new ComplexHead2CritEffect(creature, aimedPart, name);
		}			

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature)
		{
			creature.Int = creature.GetInt() + Modifier;
			creature.Will = creature.GetWill() + Modifier;
			creature.Stun  += Modifier;
		}

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature)
		{
			if (IsStabile(Severity))
			{
				creature.Int = creature.GetInt() - Modifier;
				creature.Will = creature.GetWill() - Modifier;
			}
			else
			{
				creature.Int = creature.GetInt() - Modifier;
				creature.Will = creature.GetWill() - Modifier;
				creature.Stun -= Modifier;
			}
		}

		/// <summary>
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature)
		{
			if (IsStabile(Severity))
				return;

			Severity = Severity.Complex;

			creature.Stun -= Modifier;
		}
	}
}
