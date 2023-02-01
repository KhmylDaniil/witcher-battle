using MediatR;
using System;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Contracts.BattleRequests.CreatureAttack
{
	/// <summary>
	/// Запрос атаки существа
	/// </summary>
	public class CreatureAttackRequest: IRequest<CreatureAttackResponse>
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
		/// Специальный бонус к попаданию
		/// </summary>
		public int SpecialToHit { get; set; }

		/// <summary>
		/// Специальный бонус к урону
		/// </summary>
		public int SpecialToDamage { get; set; }
	}
}
