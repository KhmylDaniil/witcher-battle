using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Logic;
using System.Linq;
using System.Text;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Entities.Effects
{
	/// <summary>
	/// Критический эффект - Инородный объект
	/// </summary>
	public class SimpleTorso1CritEffect : CritEffect, ICrit
	{
		private SimpleTorso1CritEffect() { }

		/// <summary>
		/// Конструктор эффекта инородного объекта
		/// </summary>
		/// <param name="creature">Существо</param>
		/// <param name="name">Название</param>
		/// <param name="aimedPart">Часть тела</param>
		private SimpleTorso1CritEffect(Creature creature, CreaturePart aimedPart, string name) : base(creature, aimedPart, name)
		{
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
		public static SimpleTorso1CritEffect Create(Creature creature, CreaturePart aimedPart, string name)
			=> CheckExistingEffectAndRemoveStabilizedEffect<SimpleTorso1CritEffect>(creature, aimedPart)
				? new SimpleTorso1CritEffect(creature, aimedPart, name)
				: null;

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature) { }

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature) { }

		/// <summary>
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature) { }
	}
}
