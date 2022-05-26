using System;

namespace Sindie.ApiService.Core.Abstractions
{
	/// <summary>
	/// Интерфейс получения контекста пользователя из веба
	/// </summary>
	public interface IUserContext
	{
		/// <summary>
		/// Текущий айди пользователя
		/// </summary>
		Guid CurrentUserId { get; }

		/// <summary>
		/// Текущая роль пользователя
		/// </summary>
		string Role { get; }
	}
}
