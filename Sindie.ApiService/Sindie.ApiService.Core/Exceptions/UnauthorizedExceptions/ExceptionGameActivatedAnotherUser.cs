using System;
using System.Collections.Generic;
using System.Text;

namespace Sindie.ApiService.Core.Exceptions.UnauthorizedExceptions
{
	/// <summary>
	/// Исключение игра запущена другим пользователем
	/// </summary>
	public class ExceptionGameActivatedAnotherUser : ExceptionUnauthorizedBase
	{
		/// <summary>
		/// Конструктор исключения игра запущена другим пользователем
		/// </summary>
		public ExceptionGameActivatedAnotherUser()
			: base($"Игра уже запущена другим пользователем.")
		{
		}

		/// <summary>
		/// Конструктор исключения игра запущена другим пользователем 
		/// </summary>
		/// <param name="name">Название игры</param>
		public ExceptionGameActivatedAnotherUser(string name)
				: base($"Игра с именем {name} уже запущена другим пользователем.")
		{
		}

		/// <summary>
		/// Конструктор исключения игра запущена другим пользователем 
		/// </summary>
		/// <param name="id">ИД игры</param>
		public ExceptionGameActivatedAnotherUser(Guid id)
				: base($"Игра с таким ИД {id} уже запущена другим пользователем.")
		{
		}

	}
}
