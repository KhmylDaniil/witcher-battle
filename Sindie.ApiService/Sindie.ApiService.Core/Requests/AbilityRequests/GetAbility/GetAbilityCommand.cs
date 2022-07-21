using Sindie.ApiService.Core.Contracts.AbilityRequests.GetAbility;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.AbilityRequests.GetAbility
{
	/// <summary>
	/// Команда на предоставление списка способностей
	/// </summary>
	public class GetAbilityCommand: GetAbilityQuery
	{
		/// <summary>
		/// Конструктор команды на предоставление списка способностей
		/// </summary>
		/// <param name="gameId">Айди игры</param>
		/// <param name="name">Название</param>
		/// <param name="attackSkillId">Айди навыка атаки</param>
		/// <param name="damageTypeId">Айди типа атаки</param>
		/// <param name="conditionId">Айди состояния</param>
		/// <param name="minAttackDiceQuantity">Минимальное количество кубов атаки</param>
		/// <param name="maxAttackDiceQuantity">Максимальное количество кубов атаки</param>
		/// <param name="userName">Имя Автора</param>
		/// <param name="creationMinTime">Минимальное время создания</param>
		/// <param name="creationMaxTime">Максимальное время создания</param>
		/// <param name="modificationMinTime">Минимальное время модификации</param>
		/// <param name="modificationMaxTime">Максимальное время модификации</param>
		/// <param name="pageSize">Размер страницы</param>
		/// <param name="pageNumber">Номер страниицы</param>
		/// <param name="orderBy">Сортировка по полю</param>
		/// <param name="isAscending">Сортировка по возрастанию</param>
		public GetAbilityCommand(
			Guid gameId,
			string name,
			Guid? attackSkillId,
			Guid? damageTypeId,
			Guid? conditionId,
			int minAttackDiceQuantity,
			int maxAttackDiceQuantity,
			string userName,
			DateTime creationMinTime,
			DateTime creationMaxTime,
			DateTime modificationMinTime,
			DateTime modificationMaxTime,
			int pageSize,
			int pageNumber,
			string orderBy,
			bool isAscending)
		{
			GameId = gameId;
			Name = name;
			AttackSkillId = attackSkillId;
			DamageTypeId = damageTypeId;
			ConditionId = conditionId;
			MinAttackDiceQuantity = minAttackDiceQuantity < 0 
				? throw new ExceptionRequestFieldIncorrectData<GetAbilityQuery>(nameof(MinAttackDiceQuantity))
				: minAttackDiceQuantity;
			MaxAttackDiceQuantity = maxAttackDiceQuantity < minAttackDiceQuantity
				? throw new ExceptionRequestFieldIncorrectData<GetAbilityQuery>(nameof(MaxAttackDiceQuantity))
				: maxAttackDiceQuantity;
			MaxAttackDiceQuantity = MaxAttackDiceQuantity == 0 ? 10 : MaxAttackDiceQuantity;
			UserName = userName;
			CreationMinTime = creationMinTime;
			CreationMaxTime = creationMaxTime;
			ModificationMinTime = modificationMinTime;
			ModificationMaxTime = modificationMaxTime;
			PageSize = pageSize == default ? 10 : pageSize;
			PageNumber = pageNumber == default ? 1 : pageNumber;
			OrderBy = orderBy;
			IsAscending = isAscending;
		}
	}
}
