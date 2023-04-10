using System.Collections.Generic;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Abstractions;

namespace Witcher.Core.Entities
{
	public class WeaponTemplate : ItemTemplate, IAttackFormula
	{
		private int _attackDiceQuantity;
		private int _durability;

		/// <summary>
		/// Навык атаки
		/// </summary>
		public Skill AttackSkill { get; set; }

		/// <summary>
		/// Тип урона
		/// </summary>
		public DamageType DamageType { get; set; }

		/// <summary>
		/// Количество кубов атаки
		/// </summary>
		public int AttackDiceQuantity
		{
			get => _attackDiceQuantity;
			set
			{
				if (value < 0)
					throw new FieldOutOfRangeException<WeaponTemplate>(nameof(AttackDiceQuantity));
				_attackDiceQuantity = value;
			}
		}

		/// <summary>
		/// Модификатор атаки
		/// </summary>
		public int DamageModifier { get; set; }

		/// <summary>
		/// Точность атаки
		/// </summary>
		public int Accuracy { get; set; }

		/// <summary>
		/// Прочность
		/// </summary>
		public int Durability
		{
			get => _durability;
			set
			{
				if (value < 0)
					throw new FieldOutOfRangeException<WeaponTemplate>(nameof(Durability));
				_durability = value;
			}
		}

	#region navigation properties

	/// <summary>
	/// Накладываемые состояния
	/// </summary>
	public List<AppliedCondition> AppliedConditions { get; set; }

		/// <summary>
		/// Защитные навыки
		/// </summary>
		public List<DefensiveSkill> DefensiveSkills { get; set; }

		#endregion navigation properties
	}
}
