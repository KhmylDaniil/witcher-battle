using Sindie.ApiService.Core.Entities;
using System;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Abstractions
{
	/// <summary>
	/// Интерфейс критического эффекта
	/// </summary>
	public interface ICrit
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; }

		/// <summary>
		/// Тяжесть критического эффекта
		/// </summary>
		public Severity Severity { get; }

		/// <summary>
		/// Тип части тела
		/// </summary>
		public BodyPartType BodyPartLocation { get; }

		/// <summary>
		/// Применить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void ApplyStatChanges(Creature creature);

		/// <summary>
		/// Отменить изменения характеристик
		/// </summary>
		/// <param name="creature">Существо</param>
		public void RevertStatChanges(Creature creature);

		/// <summary>
		/// Стабилизировать критический эффект
		/// </summary>
		/// <param name="creature">Существо</param>
		public void Stabilize(Creature creature);
	}
}
