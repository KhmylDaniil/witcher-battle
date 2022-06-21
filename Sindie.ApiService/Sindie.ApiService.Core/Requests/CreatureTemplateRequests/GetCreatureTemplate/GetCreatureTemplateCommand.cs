using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		/// <param name="creatureTypeId">Айди типа существа</param>
		/// <param name="userName">Автор</param>
		/// <param name="creationMinTime">Минимальное время создания</param>
		/// <param name="creationMaxTime">Максимальное время создания</param>
		/// <param name="modificationMinTime">Минимальное время модификации</param>
		/// <param name="modificationMaxTime">Максимальное время модификации</param>
		/// <param name="bodyTemplateName">Название шаблона тела</param>
		/// <param name="bodyPartTypeId">Айди типа части тела</param>
		/// <param name="conditionName">Название накладываемого состояния</param>
		/// <param name="pageSize">Размер страницы</param>
		/// <param name="pageNumber">Номер страницы</param>
		public GetCreatureTemplateCommand(
			Guid gameId,
			string name,
			Guid? creatureTypeId,
			string userName,
			DateTime creationMinTime,
			DateTime creationMaxTime,
			DateTime modificationMinTime,
			DateTime modificationMaxTime,
			string bodyTemplateName,
			Guid? bodyPartTypeId,
			string conditionName,
			int pageSize,
			int pageNumber,
			string orderBy,
			bool isAscending)
		{
			GameId = gameId;
			Name = name;
			CreatureTypeId = creatureTypeId;
			UserName = userName;
			CreationMinTime = creationMinTime;
			CreationMaxTime = creationMaxTime;
			ModificationMinTime = modificationMinTime;
			ModificationMaxTime = modificationMaxTime;
			BodyTemplateName = bodyTemplateName;
			BodyPartTypeId = bodyPartTypeId;
			ConditionName = conditionName;
			PageSize = pageSize == default ? 10 : pageSize;
			PageNumber = pageNumber == default ? 1 : pageNumber;
			OrderBy = orderBy;
			IsAscending = isAscending;
		}
	}
}
