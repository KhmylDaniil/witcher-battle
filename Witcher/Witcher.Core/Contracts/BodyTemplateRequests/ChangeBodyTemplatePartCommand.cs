using System;
using MediatR;

namespace Witcher.Core.Contracts.BodyTemplateRequests
{
	/// <summary>
	/// Команда на изменение части шаблона тела
	/// </summary>
	public class ChangeBodyTemplatePartCommand : UpdateBodyTemplateRequestItem, IRequest
	{
		/// <summary>
		/// Айди шаблона тела
		/// </summary>
		public Guid Id { get; set; }
	}
}
