using MediatR;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.CharacterTemplateRequests.CreateCharacterTemplate
{
	/// <summary>
	/// Команда создания шаблона персонажа
	/// </summary>
	public class CreateCharacterTemplateCommand: IRequest<Unit>
	{
		/// <summary>
		/// Название шаблона персонажа
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание шаблона персонажа
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Айди графического файла
		/// </summary>
		public Guid? ImgFileId { get; set; }

		/// <summary>
		/// Айди интерфейса
		/// </summary>
		public Guid? InterfaceId { get; set; }

		/// <summary>
		/// Модификаторы
		/// </summary>
		public List<Guid> Modifiers { get; set; }
	}
}
