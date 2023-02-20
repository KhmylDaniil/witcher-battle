﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Sindie.ApiService.Core.Exceptions.RequestExceptions
{
	/// <summary>
	/// Исключение не уникальное имя в запросе
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class RequestNameNotUniqException<T> : RequestValidationException
	{
		/// <summary>
		/// Конструктор исключения не уникального имени в запросе
		/// </summary>
		public RequestNameNotUniqException()
			: base($"{typeof(T)} должен быть уникальным")
		{
		}

		/// <summary>
		/// Конструктор исключения не уникального имени в запросе
		/// </summary>
		/// <param name="name">Имя</param>
		public RequestNameNotUniqException(string name)
			: base($"{typeof(T)} с именем {name} уже существует.")
		{
		}
	}
}