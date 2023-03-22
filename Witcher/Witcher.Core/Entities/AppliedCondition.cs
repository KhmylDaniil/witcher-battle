using Witcher.Core.BaseData;
using System;
using Witcher.Core.Exceptions.EntityExceptions;

namespace Witcher.Core.Entities
{
	public class AppliedCondition: EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_ability"/>
		/// </summary>
		public const string AbilityField = nameof(_ability);

		private Ability _ability;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected AppliedCondition()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="ability">Способность</param>
		/// <param name="condition">Состояние</param>
		/// <param name="applyChance">Шанс применения</param>
		public AppliedCondition(Ability ability, Condition condition, int applyChance)
		{
			Ability = ability;
			Condition = condition;
			ApplyChance = applyChance;
		}

		/// <summary>
		/// Айди способности
		/// </summary>
		public Guid AbilityId { get; set; }

		/// <summary>
		/// Шанс применения
		/// </summary>
		public int ApplyChance { get; set; }

		/// <summary>
		/// Тип состояния
		/// </summary>
		public Condition Condition { get; set; }

		#region navigation properties

		/// <summary>
		/// Способность
		/// </summary>
		public Ability Ability
		{
			get => _ability;
			set
			{
				_ability = value ?? throw new EntityNotIncludedException<AppliedCondition>(nameof(Ability));
				AbilityId = value.Id;
			}
		}

		#endregion navigation properties

		/// <summary>
		/// Создание накладываемого состояния
		/// </summary>
		/// <param name="ability"></param>
		/// <param name="condition"></param>
		/// <param name="applyChance"></param>
		/// <returns></returns>
		public static AppliedCondition CreateAppliedCondition(Ability ability, Condition condition, int applyChance)
		=> new AppliedCondition(ability, condition, applyChance);

		/// <summary>
		/// Изменение накладываемого состояния
		/// </summary>
		/// <param name="condition">Состояние</param>
		/// <param name="applyChance">Шанс применения</param>
		public void ChangeAppliedCondition(Condition condition, int applyChance)
		{
			Condition = condition;
			ApplyChance = applyChance;
		}
	}
}
