using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Contracts.RunBattleRequests
{
	// <summary>
	/// Команда атаки существа
	/// </summary>
	public class AttackCommand : IValidatableCommand<AttackResult>
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
		/// Айди способности атаки
		/// </summary>
		public Guid? AbilityId { get; set; }

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
		public void Validate()
		{
			if (DamageValue is not null && DamageValue.Value < 0)
				throw new RequestFieldIncorrectDataException<AttackCommand>(nameof(DamageValue), "Значение не может быть отрицательным.");

			if (AttackValue is not null && AttackValue.Value < 0)
				throw new RequestFieldIncorrectDataException<AttackCommand>(nameof(AttackValue), "Значение не может быть отрицательным.");

			if (DefenseValue is not null && DefenseValue.Value < 0)
				throw new RequestFieldIncorrectDataException<AttackCommand>(nameof(DefenseValue), "Значение не может быть отрицательным.");

			if (DefensiveSkill is not null && !Enum.IsDefined(DefensiveSkill.Value))
				throw new RequestFieldIncorrectDataException<AttackCommand>(nameof(DefensiveSkill));
		}
	}
}
