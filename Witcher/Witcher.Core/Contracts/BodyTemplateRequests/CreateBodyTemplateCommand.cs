using Witcher.Core.Entities;
using MediatR;

namespace Witcher.Core.Contracts.BodyTemplateRequests
{
	/// <summary>
	/// Запрос на создание шаблона тела
	/// </summary>
	public class CreateBodyTemplateCommand : IRequest<BodyTemplate>
	{
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
