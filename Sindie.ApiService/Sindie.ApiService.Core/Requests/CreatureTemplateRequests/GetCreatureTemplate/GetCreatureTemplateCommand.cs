using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplate;
using System;

namespace Sindie.ApiService.Core.Requests.CreatureTemplateRequests.GetCreatureTemplate
{
	/// <summary>
	/// Команда предоставления списка шаблонов существа
	/// </summary>
	public class GetCreatureTemplateCommand : GetCreatureTemplateQuery
	{
		/// <summary>
		/// Конструктор команды предоставления списка шаблонов существа
		/// </summary>
		/// <param name="gameId">Айди игры</param>
		/// <param name="name">Название</param>
		/// <param name="creatureType">Тип существа</param>
		/// <param name="userName">Автор</param>
		/// <param name="creationMinTime">Минимальное время создания</param>
		/// <param name="creationMaxTime">Максимальное время создания</param>
		/// <param name="modificationMinTime">Минимальное время модификации</param>
		/// <param name="modificationMaxTime">Максимальное время модификации</param>
		/// <param name="bodyTemplateName">Название шаблона тела</param>
		/// <param name="bodyPartType">Тип части тела</param>
		/// <param name="conditionName">Название накладываемого состояния</param>
		/// <param name="pageSize">Размер страницы</param>
		/// <param name="pageNumber">Номер страницы</param>
		public GetCreatureTemplateCommand(
			Guid gameId,
			string name,
			string creatureType,
			string userName,
			DateTime creationMinTime,
			DateTime creationMaxTime,
			DateTime modificationMinTime,
			DateTime modificationMaxTime,
			string bodyTemplateName,
			string bodyPartType,
			string conditionName,
			int pageSize,
			int pageNumber,
			string orderBy,
			bool isAscending)
		{
			GameId = gameId;
			Name = name;
			CreatureType = creatureType;
			UserName = userName;
			CreationMinTime = creationMinTime;
			CreationMaxTime = creationMaxTime;
			ModificationMinTime = modificationMinTime;
			ModificationMaxTime = modificationMaxTime;
			BodyTemplateName = bodyTemplateName;
			BodyPartType = bodyPartType;
			ConditionName = conditionName;
			PageSize = pageSize == default ? 10 : pageSize;
			PageNumber = pageNumber == default ? 1 : pageNumber;
			OrderBy = orderBy;
			IsAscending = isAscending;
		}
	}
}
