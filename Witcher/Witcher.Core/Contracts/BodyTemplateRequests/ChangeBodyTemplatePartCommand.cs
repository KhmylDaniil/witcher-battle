using System;
using MediatR;

namespace Witcher.Core.Contracts.BodyTemplateRequests
{
	/// <summary>
	/// Команда на изменение части шаблона тела
	/// </summary>
	public class ChangeBodyTemplatePartCommand : UpdateBodyTemplateRequestItem, IRequest<Unit>
	{
		/// <summary>
		/// Айди шаблона тела
		/// </summary>
		public Guid Id { get; set; }
	}
}
