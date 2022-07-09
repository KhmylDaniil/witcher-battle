using Sindie.ApiService.Core.Contracts.BattleRequests.CreatureAttack;
using System;

namespace Sindie.ApiService.Core.Requests.BattleRequests.CreatureAttack
{
	/// <summary>
	/// Команда атаки существа
	/// </summary>
	public class CreatureAttackCommand: CreatureAttackRequest
	{
		/// <summary>
		/// Конструктор команды атаки героя
		/// </summary>
		/// <param name="instanceId">Айди инстанса</param>
		/// <param name="attackerId">Айди атакующего существа</param>
		/// <param name="abilityId">Айди способности</param>
		/// <param name="targetCreatureId">Айди существа цели</param>
		/// <param name="creaturePartId">Айди части тела при прицеливании</param>
		/// <param name="defensiveParameter">Способ защиты</param>
		/// <param name="specialToHit">Специальный бонус к попаданию</param>
		/// <param name="specialToDamage">Специальный бонус к урону</param>
		public CreatureAttackCommand(
			Guid instanceId,
			Guid attackerId,
			Guid? abilityId,
			Guid targetCreatureId,
			Guid? creaturePartId,
			Guid? defensiveParameter,
			int? specialToHit,
			int? specialToDamage)
		{
			InstanceId = instanceId;
			AttackerId = attackerId;
			AbilityId = abilityId;
			TargetCreatureId = targetCreatureId;
			CreaturePartId = creaturePartId;
			DefensiveParameter = defensiveParameter;
			SpecialToHit = specialToHit ?? default;
			SpecialToDamage = specialToDamage ?? default;
		}
	}
}
