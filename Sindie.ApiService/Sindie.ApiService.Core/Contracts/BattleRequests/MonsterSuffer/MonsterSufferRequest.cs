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
		/// Айди инстанса
		/// </summary>
		public Guid InstanceId { get; set; }

		/// <summary>
		/// Айди существа
		/// </summary>
		public Guid MonsterId { get; set; }
		
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

		/// <summary>
		/// Сопротивляемость урону
		/// </summary>
		public bool IsResistant { get; set; }

		/// <summary>
		/// Уязвимость урону
		/// </summary>
		public bool IsVulnerable { get; set; }
	}
}
