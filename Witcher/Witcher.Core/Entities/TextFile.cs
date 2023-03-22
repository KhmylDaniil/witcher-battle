using System;
using System.Collections.Generic;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Текстовый файл
	/// </summary>
	public class TextFile : EntityBase
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
		public static TextFile CreateForTest(
				Guid? id = default,
				string name = default,
				string extension = default,
				int size = default)
				=> new()
				{
					Id = id ?? Guid.NewGuid(),
					Name = name ?? "ImgFile",
					Extension = extension ?? "Extension",
					Size = size,
				};
	}
}
