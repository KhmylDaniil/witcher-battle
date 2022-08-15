using Sindie.ApiService.Core.Contracts.AbilityRequests.CreateAbility;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Requests.AbilityRequests.CreateAbility
{
	/// <summary>
	/// Команда создания способности
	/// </summary>
	public class CreateAbilityCommand: CreateAbilityRequest
	{
		/// <summary>
		/// Конструктор команды создания способности
		/// </summary>
		/// <param name="gameId">Айди игры</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="attackSkillId">Айди навыка атаки</param>
		/// <param name="attackDiceQuantity">Количество кубов атаки</param>
		/// <param name="damageModifier">Модификатор атаки</param>
		/// <param name="attackSpeed">Скорость атаки</param>
		/// <param name="accuracy">Точность атаки</param>
		/// <param name="defensiveSkills">Навыки для защиты</param>
		/// <param name="damageTypeId">Тип урона</param>
		/// <param name="appliedConditions">Накладываемые состояния</param>
		public CreateAbilityCommand(
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
			List<CreateAbilityRequestAppliedCondition> appliedConditions
			)
		{
			GameId = gameId;
			Name = string.IsNullOrEmpty(name)
				? throw new ExceptionRequestFieldNull<CreateAbilityRequest>(nameof(Name))
				: name;
			Description = description;
			AttackSkillId = attackSkillId;
			AttackDiceQuantity = attackDiceQuantity < 0 ? throw new ExceptionRequestFieldIncorrectData<CreateAbilityRequest>(nameof(AttackDiceQuantity)) : attackDiceQuantity;
			DamageModifier = damageModifier;
			AttackSpeed = attackSpeed < 1 ? throw new ExceptionRequestFieldIncorrectData<CreateAbilityRequest>(nameof(AttackSpeed)) : attackSpeed;
			Accuracy = accuracy;
			DefensiveSkills = defensiveSkills == null
				? throw new ExceptionRequestFieldIncorrectData<CreateAbilityRequest>(nameof(DefensiveSkills))
				: defensiveSkills;
			DamageTypeId = damageTypeId;
			AppliedConditions = appliedConditions == null
				? throw new ExceptionRequestFieldIncorrectData<CreateAbilityRequest>(nameof(AppliedConditions))
				: appliedConditions;
		}
	}
}

