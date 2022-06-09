using MediatR;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.BodyTemplateRequests.CreateBodyTemplate
{
	/// <summary>
	/// Запрос на создание шаблона тела
	/// </summary>
	public class CreateBodyTemplateRequest: IRequest
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }
		
		/// <summary>
		/// Название шаблона тела
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание шаблона тела
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Список частей тела
		/// </summary>
		public List<CreateBodyTemplateRequestItem> BodyTemplateParts { get; set; }
	}
}
