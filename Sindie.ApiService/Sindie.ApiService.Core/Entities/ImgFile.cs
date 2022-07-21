using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Графический файл
	/// </summary>
	public class ImgFile : EntityBase
	{
		/// <summary>
		/// Название файла
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Расширение файла
		/// </summary>
		public string Extension { get; set; }

		/// <summary>
		/// Размер файла
		/// </summary>
		public int Size { get; set; }

		#region navigation properties

		/// <summary>
		/// Пользователи
		/// </summary>
		public List<User> Users { get; set; }

		/// <summary>
		/// Игры
		/// </summary>
		public List<Game> Games { get; set; }

		/// <summary>
		/// Пользователь аватара
		/// </summary>
		public User UserAvatar { get; set; }

		/// <summary>
		/// Игра
		/// </summary>
		public Game Game { get; set; }

		/// <summary>
		/// Бой
		/// </summary>
		public Battle Battle { get; set; }


		/// <summary>
		/// Шаблон существа
		/// </summary>
		public CreatureTemplate CreatureTemplate {get; set;}

		/// <summary>
		/// Существо
		/// </summary>
		public Creature Creature { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="id">Ид</param>
		/// <param name="name">Название файла</param>
		/// <param name="extension">Расширение файла</param>
		/// <param name="size">Размер файла</param>
		/// <returns>-</returns>
		[Obsolete("Только для тестов")]
		public static ImgFile CreateForTest(
				Guid? id = default,
				string name = default,
				string extension = default,
				int size = default)
				=> new ImgFile()
				{
					Id = id ?? default,
					Name = name ?? "ImgFile",
					Extension = extension ?? "Extension",
					Size = size,
				};
	}
}
