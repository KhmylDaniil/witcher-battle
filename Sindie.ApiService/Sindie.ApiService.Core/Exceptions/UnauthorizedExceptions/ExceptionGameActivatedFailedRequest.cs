using System;
using System.Collections.Generic;
using System.Text;

namespace Sindie.ApiService.Core.Exceptions.UnauthorizedExceptions
{
	/// <summary>
	/// Исключение игра запущена, нельзя осуществить действия
	/// </summary>
	public class ExceptionGameActivatedFailedRequest<T> : ExceptionUnauthorizedBase
	{
		/// <summary>
		/// Конструктор исключения игра запущена, нельзя осуществить действия
		/// </summary>
		public ExceptionGameActivatedFailedRequest()
			: base($"Игра уже запущена, нельзя осуществить действия {typeof(T)}.")
		{
		}

		/// <summary>
		/// Конструктор исключения игра запущена, нельзя осуществить действия
		/// </summary>
		/// <param name="name">Название игры</param>
		public ExceptionGameActivatedFailedRequest(string name)
				: base($"Игра с именем {name} уже запущена, нельзя осуществить действия {typeof(T)}.")
		{
		}

		/// <summary>
		/// Конструктор исключения игра запущена, нельзя осуществить действия
		/// </summary>
		/// <param name="id">ИД игры</param>
		public ExceptionGameActivatedFailedRequest(Guid id)
				: base($"Игра с таким ИД {id} уже запущена, нельзя осуществить действия {typeof(T)}.")
		{
		}
		/// <summary>
		/// Конструктор исключения игра запущена, нельзя осуществить действия
		/// </summary>
		/// <param name="gameName"></param>
		/// <param name="actionName"></param>
		public ExceptionGameActivatedFailedRequest(string gameName, string actionName)
				: base($"Игра с именем {gameName} уже запущена, нельзя осуществить действия {actionName}.")
		{
		}

	}
}
