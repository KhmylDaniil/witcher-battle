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
		/// <param name="battleId">Айди боя</param>
		/// <param name="id">Айди монстра</param>
		/// <param name="abilityId">Айди способности</param>
		/// <param name="targetCreatureId">Айди цели</param>
		/// <param name="creaturePartId">Айди части тела при прицеливании</param>
		/// <param name="defenseValue">Защита</param>
		/// <param name="specialToHit">Специальный бонус к попаданию</param>
		/// <param name="specialToDamage">Специальный бонус к урону</param>
		public MonsterAttackCommand(
			Guid battleId,
			Guid id,
			Guid? abilityId,
			Guid targetCreatureId,
			Guid? creaturePartId,
			int defenseValue,
			int specialToHit,
			int specialToDamage)
		{
			BattleId = battleId;
			Id = id;
			AbilityId = abilityId;
			TargetCreatureId = targetCreatureId;
			CreaturePartId = creaturePartId;
			DefenseValue = defenseValue < 1 
				? throw new RequestFieldIncorrectDataException<MonsterAttackRequest>(nameof(DefenseValue))
				: defenseValue;
			SpecialToHit = specialToHit;
			SpecialToDamage = specialToDamage;
		}
	}
}
