using Sindie.ApiService.Core.Contracts.BattleRequests.MonsterSuffer;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;

namespace Sindie.ApiService.Core.Requests.BattleRequests.MonsterSuffer
{
	/// <summary>
	/// Команда получения монстром урона
	/// </summary>
	public class MonsterSufferCommand: MonsterSufferRequest
	{
		/// <summary>
		/// Конструктор команды получения монстром урона
		/// </summary>
		/// <param name="instanceId">Айди инстанса</param>
		/// <param name="attackerId">Айди атакующего существа</param>
		/// <param name="targetId">Айди существа цели</param>
		/// <param name="abilityId">Айди способности</param>
		/// <param name="damageValue">Значение урона</param>
		/// <param name="successValue">Значение успешности атаки</param>
		/// <param name="creaturePartId">Айди части тела цели</param>

		public MonsterSufferCommand(
			Guid instanceId,
			Guid attackerId,
			Guid targetId,
			Guid abilityId,
			int damageValue,
			int successValue,
			Guid? creaturePartId)
		{
			InstanceId = instanceId;
			AttackerId = attackerId;
			TargetId = targetId;
			AbilityId = abilityId;
			DamageValue = damageValue < 0
				? throw new ExceptionRequestFieldIncorrectData<MonsterSufferRequest>(nameof(DamageValue))
				: damageValue;
			SuccessValue = successValue < 1
				? throw new ExceptionRequestFieldIncorrectData<MonsterSufferRequest>(nameof(SuccessValue))
				: successValue;
			CreaturePartId = creaturePartId;
		}
	}
}
