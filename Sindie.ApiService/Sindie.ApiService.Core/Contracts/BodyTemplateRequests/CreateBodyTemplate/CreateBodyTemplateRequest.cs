using MediatR;
using Sindie.ApiService.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sindie.ApiService.Core.Contracts.BodyTemplateRequests.CreateBodyTemplate
{
	/// <summary>
	/// Запрос на создание шаблона тела
	/// </summary>
	public class CreateBodyTemplateRequest : IRequest<BodyTemplate>
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		[Required]
		public Guid GameId { get; set; }

		/// <summary>
		/// Название шаблона тела
		/// </summary>
		[Required]
		public string Name { get; set; }

		/// <summary>
		/// Описание шаблона тела
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Список частей тела
		/// </summary>
		[Required]
		public List<UpdateBodyTemplateRequestItem> BodyTemplateParts { get; set; }
	}
}
