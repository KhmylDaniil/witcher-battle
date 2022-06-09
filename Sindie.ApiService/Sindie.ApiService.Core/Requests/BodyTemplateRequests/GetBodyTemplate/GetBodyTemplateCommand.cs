using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplate;
using System;

namespace Sindie.ApiService.Core.Requests.BodyTemplateRequests.GetBodyTemplate
{
	/// <summary>
	/// Команда на предоставление списка шаблонов тела
	/// </summary>
	public class GetBodyTemplateCommand: GetBodyTemplateQuery
	{
		/// <summary>
		/// Конструктор для <see cref="GetBodyTemplateCommand"/>
		/// </summary>
		/// <param name="gameId">Айди игры</param>
		/// <param name="name">Название</param>
		/// <param name="userName">Имя Автора</param>
		/// <param name="creationMinTime">Минимальное время создания</param>
		/// <param name="creationMaxTime">Максимальное время создания</param>
		/// <param name="modificationMinTime">Минимальное время модификации</param>
		/// <param name="modificationMaxTime">Максимальное время модификации</param>
		/// <param name="bodyPartName">Название части тела</param>
		/// <param name="pageSize">Размер страницы</param>
		/// <param name="pageNumber">Номер страниицы</param>
		/// <param name="orderBy">Сортировка по полю</param>
		/// <param name="isAscending">Сортировка по возрастанию</param>
		public GetBodyTemplateCommand(
			Guid gameId,
			string name,
			string userName,
			DateTime creationMinTime,
			DateTime creationMaxTime,
			DateTime modificationMinTime,
			DateTime modificationMaxTime,
			string bodyPartName,
			int pageSize,
			int pageNumber,
			string orderBy,
			bool isAscending)
		{
			GameId = gameId;
			Name = name;
			UserName = userName;
			CreationMinTime = creationMaxTime;
			CreationMaxTime = creationMinTime;
			ModificationMinTime = modificationMaxTime;
			ModificationMaxTime = modificationMinTime;
			BodyPartName = bodyPartName;
			PageSize = pageSize == default ? 10 : pageSize;
			PageNumber = pageNumber == default ? 1 : pageNumber;
			OrderBy = orderBy;
			IsAscending = isAscending;
		}
	}
}
