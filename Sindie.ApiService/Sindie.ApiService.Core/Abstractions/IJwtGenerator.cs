using System;

namespace Sindie.ApiService.Core.Abstractions
{
	/// <summary>
	/// Интерфейс генератора токенов
	/// </summary>
	public interface IJwtGenerator
	{
		/// <summary>
		/// Создание токенов
		/// </summary>
		/// <param name="login">Логин</param>
		/// <returns>Токен</returns>
		string CreateToken(Guid id, string Role);
	}
}
