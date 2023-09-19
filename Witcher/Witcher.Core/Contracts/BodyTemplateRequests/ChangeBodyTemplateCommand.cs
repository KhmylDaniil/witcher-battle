using System;
using MediatR;

namespace Witcher.Core.Contracts.BodyTemplateRequests
{
	/// <summary>
	/// Запрос на изменение шаблона тела
	/// </summary>
	public class ChangeBodyTemplateCommand : IRequest
	{
		/// <summary>
		/// Айди шаблона тела
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Название шаблона тела
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание шаблона тела
		/// </summary>
		public string Description { get; set; }
	}
}
