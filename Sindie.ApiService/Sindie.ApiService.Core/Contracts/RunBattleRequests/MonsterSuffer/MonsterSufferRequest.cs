using MediatR;
using System;

namespace Sindie.ApiService.Core.Contracts.BattleRequests.MonsterSuffer
{
	/// <summary>
	/// Запрос получения монстром урона
	/// </summary>
	public class MonsterSufferRequest: IRequest<MonsterSufferResponse>
	{
		/// <summary>
		/// Айди боя
		/// </summary>
		public Guid BattleId { get; set; }

		/// <summary>
		/// Айди атакующего существа
		/// </summary>
		public Guid AttackerId { get; set; }

		/// <summary>
		/// Айди существа цели
		/// </summary>
		public Guid TargetId { get; set; }

		/// <summary>
		/// Айди способности
		/// </summary>
		public Guid AbilityId { get; set; }
		
		/// <summary>
		/// Значение урона
		/// </summary>
		public int DamageValue { get; set; }

		/// <summary>
		/// Айди части тела цели
		/// </summary>
		public Guid? CreaturePartId { get; set; }

		/// <summary>
		/// Превышение попадания
		/// </summary>
		public int SuccessValue { get; set; }
	}
}
