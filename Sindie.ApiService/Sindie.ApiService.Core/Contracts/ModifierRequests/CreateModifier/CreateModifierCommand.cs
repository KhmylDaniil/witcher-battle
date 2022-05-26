using MediatR;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Contracts.ModifierRequests.CreateModifier
{
	/// <summary>
	/// Команда создания модификатора
	/// </summary>
	public class CreateModifierCommand: IRequest<Unit>
	{
		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }
		
		/// <summary>
		/// Айди графического файла
		/// </summary>
		public Guid? ImgFileId { get; set; }

		/// <summary>
		/// Айди шаблонов персонажа
		/// </summary>
		public List<Guid> CharacterTemplates { get; set; }

		/// <summary>
		/// Айди шаблонов предмета
		/// </summary>
		public List<Guid> ItemTemplates { get; set; }

		/// <summary>
		/// Скрипты модификатора
		/// </summary>
		public List<CreateModifierCommandItem> ModifierScripts { get; set; }
	}
}
