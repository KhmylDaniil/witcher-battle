//using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.ChangeBodyTemplate;
//using Sindie.ApiService.Core.Exceptions.RequestExceptions;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Sindie.ApiService.Core.Requests.BodyTemplateRequests.ChangeBodyTemplate
//{
//	/// <summary>
//	/// Команда на изменение шаблона тела
//	/// </summary>
//	public class ChangeBodyTemplateCommand : ChangeBodyTemplateRequest
//	{
//		/// <summary>
//		/// Конструктор создания команды изменения шаблона тела
//		/// </summary>
//		/// <param name="id">Айди</param>
//		/// <param name="gameId">Айди игры</param>
//		/// <param name="name">Название</param>
//		/// <param name="description">Описание</param>
//		/// <param name="bodyTemplateParts">Список частей тела</param>
//		public ChangeBodyTemplateCommand(
//			Guid id,
//			Guid gameId,
//			string name,
//			string description,
//			List<ChangeBodyTemplateRequestItem> bodyTemplateParts)
//		{
//			Id = id;
//			GameId = gameId;
//			Name = string.IsNullOrWhiteSpace(name)
//				? throw new ExceptionRequestFieldNull<ChangeBodyTemplateRequest>(nameof(Name))
//				: name;
//			Description = description;
//			BodyTemplateParts = bodyTemplateParts == null || !bodyTemplateParts.Any()
//					? throw new ExceptionRequestFieldNull<ChangeBodyTemplateRequest>(nameof(BodyTemplateParts))
//					: bodyTemplateParts;
//		}
//	}
//}
