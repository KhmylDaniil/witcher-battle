using System;
using Witcher.Core.Exceptions.RequestExceptions;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	public abstract class AttackBaseCommand
	{
		/// <summary>
		/// Айди боя
		/// </summary>
		public Guid BattleId { get; set; }

		/// <summary>
		/// Айди атакующего существа
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Айди цели
		/// </summary>
		public Guid TargetCreatureId { get; set; }

		/// <summary>
		/// Айди части тела при прицельной атаке
		/// </summary>
		public Guid? CreaturePartId { get; set; }

		/// <summary>
		/// Способ защиты
		/// </summary>
		public Skill? DefensiveSkill { get; set; }

		/// <summary>
		/// Значение защиты
		/// </summary>
		public int? DefenseValue { get; set; }

		/// <summary>
		/// Значение урона
		/// </summary>
		public int? DamageValue { get; set; }

		/// <summary>
		/// Значение атаки
		/// </summary>
		public int? AttackValue { get; set; }

		/// <summary>
		/// Специальный бонус к попаданию
		/// </summary>
		public int SpecialToHit { get; set; }

		/// <summary>
		/// Специальный бонус к урону
		/// </summary>
		public int SpecialToDamage { get; set; }

		/// <summary>
		/// Валидация
		/// </summary>
		public virtual void Validate()
		{
			if (DamageValue is not null && DamageValue.Value < 0)
				throw new RequestFieldIncorrectDataException<AttackWithAbilityCommand>(nameof(DamageValue), "Значение не может быть отрицательным.");

			if (AttackValue is not null && AttackValue.Value < 0)
				throw new RequestFieldIncorrectDataException<AttackWithAbilityCommand>(nameof(AttackValue), "Значение не может быть отрицательным.");

			if (DefenseValue is not null && DefenseValue.Value < 0)
				throw new RequestFieldIncorrectDataException<AttackWithAbilityCommand>(nameof(DefenseValue), "Значение не может быть отрицательным.");

			if (DefensiveSkill is not null && !Enum.IsDefined(DefensiveSkill.Value))
				throw new RequestFieldIncorrectDataException<AttackWithAbilityCommand>(nameof(DefensiveSkill));
		}
	}
}
