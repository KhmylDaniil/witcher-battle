using MediatR;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sindie.ApiService.Core.Contracts.BodyTemplatePartsRequests
{
	/// <summary>
	/// Команда на изменение части шаблона тела
	/// </summary>
	public class ChangeBodyTemplatePartCommand : UpdateBodyTemplateRequestItem, IRequest
	{
		/// <summary>
		/// Айди шаблона тела
		/// </summary>
		[Required]
		public Guid BodyTemplateId { get; set; }

		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }
	}
}
