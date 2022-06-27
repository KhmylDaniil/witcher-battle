using Sindie.ApiService.Core.Contracts.BattleRequests.HeroAttack;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BattleRequests.HeroAttack
{
	/// <summary>
	/// Команда атаки героя
	/// </summary>
	public class HeroAttackCommand: HeroAttackRequest
	{
		/// <summary>
		/// Конструктор команды атаки героя
		/// </summary>
		/// <param name="instanceId">Айди инстанса</param>
		/// <param name="id">Айди существа героя</param>
		/// <param name="abilityId">Айди способности</param>
		/// <param name="targetCreatureId">Айди существа цели</param>
		/// <param name="bodyTemplatePartId">Айди части тела при прицеливании</param>
		/// <param name="attackValue">Значение атаки</param>
		/// <param name="damageValue">Значение урона</param>
		/// <param name="specialToHit">Специальный бонус к попаданию</param>
		/// <param name="specialToDamage">Специальный бонус к урону</param>
		public HeroAttackCommand(
			Guid instanceId,
			Guid id,
			Guid? abilityId,
			Guid targetCreatureId,
			Guid? bodyTemplatePartId,
			int attackValue,
			int? damageValue,
			int? specialToHit,
			int? specialToDamage)
		{
			InstanceId = instanceId;
			Id = id;
			AbilityId = abilityId;
			TargetCreatureId = targetCreatureId;
			BodyTemplatePartId = bodyTemplatePartId;
			AttackValue = attackValue < 1
				? throw new ExceptionRequestFieldIncorrectData<HeroAttackRequest>(nameof(AttackValue))
				: attackValue;
			DamageValue = damageValue < 0
				? throw new ExceptionRequestFieldIncorrectData<HeroAttackRequest>(nameof(DamageValue))
				: damageValue;
			SpecialToHit = specialToHit ?? default;
			SpecialToDamage = specialToDamage ?? default;
		}
	}
}
