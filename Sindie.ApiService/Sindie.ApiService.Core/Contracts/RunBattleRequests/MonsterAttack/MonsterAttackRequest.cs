using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.BattleRequests.MonsterAttack
{
	/// <summary>
	/// Запрос атаки монстра
	/// </summary>
	public class MonsterAttackRequest: IRequest<MonsterAttackResponse>
	{
		/// <summary>
		/// Айди боя
		/// </summary>
		public Guid BattleId { get; set; }

		/// <summary>
		/// Айди монстра
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
		public Guid? CreaturePartId { get; set; }

		/// <summary>
		/// Значение защиты
		/// </summary>
		public int DefenseValue { get; set; }

		/// <summary>
		/// Специальный бонус к попаданию
		/// </summary>
		public int SpecialToHit { get; set; }

		/// <summary>
		/// Специальный бонус к урону
		/// </summary>
		public int SpecialToDamage { get; set; }
	}
}
