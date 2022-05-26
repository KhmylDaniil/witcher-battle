using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Статья
	/// </summary>
	public class Article : EntityBase
	{

		/// <summary>
		/// Название статьи
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Текст статьи
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; set; }

		#region navigation properties

		/// <summary>
		/// Игра
		/// </summary>
		public Game Game { get; set; }

		#endregion navigation properties
	}
}