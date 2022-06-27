using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.BattleRequests.HeroAttack
{
	/// <summary>
	/// Запрос атаки героя
	/// </summary>
	public class HeroAttackRequest: IRequest<HeroAttackResponse>
	{
		/// <summary>
		/// Айди инстанса
		/// </summary>
		public Guid InstanceId { get; set; }

		/// <summary>
		/// Айди героя
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Айди способности атаки
		/// </summary>
		public Guid? AbilityId { get; set; }

		/// <summary>
		/// Айди цели
		/// </summary>
		public Guid TargetCreatureId { get; set; }

		/// <summary>
		/// Айди части тела при прицельной атаке
		/// </summary>
		public Guid? BodyTemplatePartId { get; set; }

		/// <summary>
		/// Значение атаки
		/// </summary>
		public int AttackValue { get; set; }

		/// <summary>
		/// Значение урона - опционально
		/// </summary>
		public int? DamageValue { get; set; }

		/// <summary>
		/// Специальный бонус к попаданию
		/// </summary>
		public int? SpecialToHit { get; set; }

		/// <summary>
		/// Специальный бонус к урону
		/// </summary>
		public int? SpecialToDamage { get; set; }
	}
}
