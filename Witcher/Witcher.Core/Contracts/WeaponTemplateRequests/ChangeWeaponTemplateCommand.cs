using System;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Contracts.ItemTemplateBase;
using MediatR;

namespace Witcher.Core.Contracts.WeaponTemplateRequests
{
	public sealed class ChangeWeaponTemplateCommand : CreateOrUpdateItemTemplateCommandBase, IRequest
	{
		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }
		
		/// <summary>
		/// Навык атаки
		/// </summary>
		public Skill AttackSkill { get; set; }

		/// <summary>
		/// Количество кубов атаки
		/// </summary>
		public int AttackDiceQuantity { get; set; }

		/// <summary>
		/// Модификатор атаки
		/// </summary>
		public int DamageModifier { get; set; }

		/// <summary>
		/// Точность атаки
		/// </summary>
		public int Accuracy { get; set; }

		/// <summary>
		/// Тип урона
		/// </summary>
		public DamageType DamageType { get; set; }

		/// <summary>
		/// Прочность
		/// </summary>
		public int Durability { get; set; }

		/// <summary>
		/// Дальность
		/// </summary>
		public int? Range { get; set; }
	}
}
