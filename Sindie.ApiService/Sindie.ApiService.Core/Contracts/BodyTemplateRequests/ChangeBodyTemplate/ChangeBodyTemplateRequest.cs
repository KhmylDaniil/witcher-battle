using MediatR;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.BodyTemplateRequests.ChangeBodyTemplate
{
	/// <summary>
	/// Запрос на изменение шаблона тела
	/// </summary>
	public class ChangeBodyTemplateRequest : IRequest
	{
		/// <summary>
		/// Айди шаблона тела
		/// </summary>
		public Guid Id { get; set; }
		
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
		public List<UpdateBodyTemplateRequestItem> BodyTemplateParts { get; set; }
	}
}
