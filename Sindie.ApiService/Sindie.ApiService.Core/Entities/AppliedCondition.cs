using Sindie.ApiService.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Entities
{
	public class AppliedCondition: EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_condition"/>
		/// </summary>
		public const string ConditionField = nameof(_condition);

		/// <summary>
		/// Поле для <see cref="_ability"/>
		/// </summary>
		public const string AbilityField = nameof(_ability);


		private Ability _ability;
		private Condition _condition;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected AppliedCondition()
		{
		}


		/// <summary>
		/// Айди способности
		/// </summary>
		public Guid AbilityId { get; set; }

		/// <summary>
		/// Шанс применения
		/// </summary>
		public double ApplyChance { get; set; }

		/// <summary>
		/// Айди состояния
		/// </summary>
		public Guid ConditionId { get; set; }

		#region navigation properties

		/// <summary>
		/// Способность
		/// </summary>
		public Ability Ability
		{
			get => _ability;
			set
			{
				_ability = value ?? throw new ApplicationException("Необходимо передать способность");
				ConditionId = value.Id;
			}
		}
		/// <summary>
		/// Состояние
		/// </summary>
		public Condition Condition
		{
			get => _condition;
			set
			{
				_condition = value ?? throw new ApplicationException("Необходимо передать состояние");
				ConditionId = value.Id;
			}
		}

		#endregion navigation properties
	}
}
