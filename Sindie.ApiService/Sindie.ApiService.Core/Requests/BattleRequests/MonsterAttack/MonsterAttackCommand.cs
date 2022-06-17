using Sindie.ApiService.Core.Contracts.BattleRequests.MonsterAttack;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;

namespace Sindie.ApiService.Core.Requests.BattleRequests.MonsterAttack
{
	/// <summary>
	/// Команда атаки монстра
	/// </summary>
	public class MonsterAttackCommand: MonsterAttackRequest
	{
		/// <summary>
		/// Конструктор команды атаки монстра
		/// </summary>
		/// <param name="instanceId">Айди инстанса</param>
		/// <param name="id">Айди монстра</param>
		/// <param name="abilityId">Айди способности</param>
		/// <param name="targetBodyTemplateId">Айди шаблона тела цели</param>
		/// <param name="bodyTemplatePartId">Айди части тела при прицеливании</param>
		/// <param name="defenseValue">Защита</param>
		public MonsterAttackCommand(
			Guid instanceId,
			Guid id,
			Guid? abilityId,
			Guid targetBodyTemplateId,
			Guid? bodyTemplatePartId,
			int defenseValue)
		{
			InstanceId = instanceId;
			Id = id;
			AbilityId = abilityId;
			TargetBodyTemplateId = targetBodyTemplateId;
			BodyTemplatePartId = bodyTemplatePartId;
			DefenseValue = defenseValue < 1 
				? throw new ExceptionRequestFieldIncorrectData<MonsterAttackRequest>(nameof(DefenseValue))
				: defenseValue;
		}
	}
}
