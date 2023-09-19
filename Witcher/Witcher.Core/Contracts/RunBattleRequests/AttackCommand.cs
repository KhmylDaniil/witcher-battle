using MediatR;
using System;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Contracts.RunBattleRequests
{
	/// <summary>
	/// Команда проведения атаки
	/// </summary>
	public class AttackCommand : IRequest
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
		public Guid TargetId { get; set; }

		/// <summary>
		/// Айди формулы атаки
		/// </summary>
		public Guid AttackFormulaId { get; set; }

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
		/// Флаг сильной атаки оружием
		/// </summary>
		public bool? IsStrongAttack { get; set; }

		/// <summary>
		/// Тип атаки
		/// </summary>
		public AttackType AttackType { get; set; }

		/// <summary>
		/// Флаг части мультиатаки
		/// </summary>
		public bool IsPartOfMultiattack { get; set; }
	}
}
