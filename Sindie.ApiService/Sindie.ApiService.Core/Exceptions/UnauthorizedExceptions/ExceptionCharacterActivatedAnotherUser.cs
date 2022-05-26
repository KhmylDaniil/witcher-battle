using System;
using System.Collections.Generic;
using System.Text;

namespace Sindie.ApiService.Core.Exceptions.UnauthorizedExceptions
{
	/// <summary>
	/// Исключение персонаж активирован другим пользователем
	/// </summary>
	public class ExceptionCharacterActivatedAnotherUser : ExceptionUnauthorizedBase
	{
		/// <summary>
		/// Конструктор исключения персонаж активирован другим пользователем
		/// </summary>
		public ExceptionCharacterActivatedAnotherUser()
			: base($"Персонаж уже активирован другим пользователем.")
		{
		}

		/// <summary>
		/// Конструктор исключения персонаж активирован другим пользователем
		/// </summary>
		/// <param name="name">Имя персонажа</param>
		public ExceptionCharacterActivatedAnotherUser(string name)
				: base($"Персонаж с именем {name} уже активирован другим пользователем.")
		{
		}

		/// <summary>
		/// Конструктор исключения персонаж активирован другим пользователем
		/// </summary>
		/// <param name="id">ИД персонажа</param>
		public ExceptionCharacterActivatedAnotherUser(Guid id)
				: base($"Персонаж с таким ИД {id} уже активирован другим пользователем.")
		{
		}

	}
}

