using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.CreateBodyTemplate;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BodyTemplateRequests.CreateBodyTemplate
{
	/// <summary>
	/// Команда на создание шаблона тела
	/// </summary>
	public class CreateBodyTemplateCommand: CreateBodyTemplateRequest
	{
		/// <summary>
		/// Конструктор создания команды создания шаблона тела
		/// </summary>
		/// <param name="gameId">Айди игры</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="bodyTemplateParts">Список частей тела</param>
		public CreateBodyTemplateCommand(
			Guid gameId,
			string name,
			string description,
			List<CreateBodyTemplateRequestItem> bodyTemplateParts)
		{
			GameId = gameId;
			Name = string.IsNullOrEmpty(name)
				? throw new ExceptionRequestFieldNull<CreateBodyTemplateRequest>(nameof(Name))
				: name;
			Description = description;
			BodyTemplateParts = bodyTemplateParts == null || !bodyTemplateParts.Any()
					? throw new ExceptionRequestFieldNull<CreateBodyTemplateRequest>(nameof(BodyTemplateParts))
					: bodyTemplateParts;
		}
	}
}
