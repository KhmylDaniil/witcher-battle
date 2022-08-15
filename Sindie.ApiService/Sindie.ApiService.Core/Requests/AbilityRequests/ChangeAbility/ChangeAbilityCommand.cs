using Sindie.ApiService.Core.Contracts.AbilityRequests.ChangeAbility;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Requests.AbilityRequests.ChangeAbility
{
	/// <summary>
	/// Команда изменения способности
	/// </summary>
	public class ChangeAbilityCommand: ChangeAbilityRequest
	{
		/// <summary>
		/// Конструктор команды создания способности
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="gameId">Айди игры</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="attackSkillId">Айди навыка атаки</param>
		/// <param name="attackDiceQuantity">Количество кубов атаки</param>
		/// <param name="damageModifier">Модификатор атаки</param>
		/// <param name="attackSpeed">Скорость атаки</param>
		/// <param name="accuracy">Точность атаки</param>
		/// <param name="defensiveSkills">Навыки для защиты</param>
		/// <param name="damageType">Тип урона</param>
		/// <param name="appliedConditions">Накладываемые состояния</param>
		public ChangeAbilityCommand(
			Guid id,
			Guid gameId,
			string name,
			string description,
			Guid attackSkillId,
			int attackDiceQuantity,
			int damageModifier,
			int attackSpeed,
			int accuracy,
			List<Guid> defensiveSkills,
			Guid damageTypeId,
			List<ChangeAbilityRequestAppliedCondition> appliedConditions
			)
		{
			Id = id;
			GameId = gameId;
			Name = string.IsNullOrEmpty(name)
				? throw new ExceptionRequestFieldNull<ChangeAbilityRequest>(nameof(Name))
				: name;
			Description = description;
			AttackSkillId = attackSkillId;
			AttackDiceQuantity = attackDiceQuantity < 0 ? throw new ExceptionRequestFieldIncorrectData<ChangeAbilityRequest>(nameof(AttackDiceQuantity)) : attackDiceQuantity;
			DamageModifier = damageModifier;
			AttackSpeed = attackSpeed < 1 ? throw new ExceptionRequestFieldIncorrectData<ChangeAbilityRequest>(nameof(AttackSpeed)) : attackSpeed;
			Accuracy = accuracy;
			DefensiveSkills = defensiveSkills == null
				? throw new ExceptionRequestFieldIncorrectData<ChangeAbilityRequest>(nameof(DefensiveSkills))
				: defensiveSkills;
			DamageTypeId = damageTypeId;
			AppliedConditions = appliedConditions == null
				? throw new ExceptionRequestFieldIncorrectData<ChangeAbilityRequest>(nameof(AppliedConditions))
				: appliedConditions;
		}
	}
}
