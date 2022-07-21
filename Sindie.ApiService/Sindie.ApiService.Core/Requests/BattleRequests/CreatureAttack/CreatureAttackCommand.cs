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
		/// <param name="battleId">Айди боя</param>
		/// <param name="attackerId">Айди атакующего существа</param>
		/// <param name="abilityId">Айди способности</param>
		/// <param name="targetCreatureId">Айди существа цели</param>
		/// <param name="creaturePartId">Айди части тела при прицеливании</param>
		/// <param name="defensiveSkillId">Способ защиты</param>
		/// <param name="specialToHit">Специальный бонус к попаданию</param>
		/// <param name="specialToDamage">Специальный бонус к урону</param>
		public CreatureAttackCommand(
			Guid battleId,
			Guid attackerId,
			Guid? abilityId,
			Guid targetCreatureId,
			Guid? creaturePartId,
			Guid? defensiveSkillId,
			int? specialToHit,
			int? specialToDamage)
		{
			BattleId = battleId;
			AttackerId = attackerId;
			AbilityId = abilityId;
			TargetCreatureId = targetCreatureId;
			CreaturePartId = creaturePartId;
			DefensiveSkillId = defensiveSkillId;
			SpecialToHit = specialToHit ?? default;
			SpecialToDamage = specialToDamage ?? default;
		}
	}
}
