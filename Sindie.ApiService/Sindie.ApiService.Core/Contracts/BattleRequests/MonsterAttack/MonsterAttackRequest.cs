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
		/// Айди инстанса
		/// </summary>
		public Guid InstanceId { get; set; }
		
		/// <summary>
		/// Айди монстра
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Айди способности атаки
		/// </summary>
		public Guid? AbilityId { get; set; }

		/// <summary>
		/// Айди шаблона цели тела
		/// </summary>
		public Guid TargetBodyTemplateId { get; set; }

		/// <summary>
		/// Айди части тела при прицельной атаке
		/// </summary>
		public Guid? BodyTemplatePartId { get; set; }

		/// <summary>
		/// Значение защиты
		/// </summary>
		public int DefenseValue { get; set; }
	}
}
