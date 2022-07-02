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
		/// <param name="monsterId">Айди монстра</param>
		/// <param name="damageValue">Значение урона</param>
		/// <param name="successValue">Значение успешности атаки</param>
		/// <param name="creaturePartId">Айди части тела цели</param>
		/// <param name="isResistant">Сопротивление урону</param>
		/// <param name="isVulnerable">Уязвимость урону</param>
		public MonsterSufferCommand(
			Guid instanceId,
			Guid monsterId,
			int damageValue,
			int successValue,
			Guid? creaturePartId,
			bool isResistant,
			bool isVulnerable)
		{
			InstanceId = instanceId;
			MonsterId = monsterId;
			DamageValue = damageValue < 0
				? throw new ExceptionRequestFieldIncorrectData<MonsterSufferRequest>(nameof(DamageValue))
				: damageValue;
			SuccessValue = successValue < 1
				? throw new ExceptionRequestFieldIncorrectData<MonsterSufferRequest>(nameof(SuccessValue))
				: successValue;
			CreaturePartId = creaturePartId;
			IsResistant = isResistant;
			IsVulnerable = isVulnerable;
		}
	}
}
