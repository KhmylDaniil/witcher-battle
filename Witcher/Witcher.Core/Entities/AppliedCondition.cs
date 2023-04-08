using Witcher.Core.BaseData;
using Witcher.Core.Exceptions.EntityExceptions;

namespace Witcher.Core.Entities
{
	public class AppliedCondition: EntityBase
	{
		private int _applyChance;
		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected AppliedCondition()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>>
		/// <param name="condition">Состояние</param>
		/// <param name="applyChance">Шанс применения</param>
		public AppliedCondition(Condition condition, int applyChance)
		{
			Condition = condition;
			ApplyChance = applyChance;
		}
		/// <summary>
		/// Шанс применения
		/// </summary>
		public int ApplyChance
		{
			get => _applyChance;
			private set
			{
				if (value < 0 || value > 100)
					throw new FieldOutOfRangeException<AppliedCondition>(nameof(ApplyChance));
				_applyChance = value;
			}
		}

		/// <summary>
		/// Тип состояния
		/// </summary>
		public Condition Condition { get; set; }

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
