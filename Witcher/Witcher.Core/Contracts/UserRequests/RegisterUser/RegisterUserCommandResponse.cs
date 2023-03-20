using System;

namespace Witcher.Core.Contracts.UserRequests.RegisterUser
{
	/// <summary>
	/// Ответ на команду регистрации
	/// </summary>
	public sealed class RegisterUserCommandResponse
	{
		/// <summary>
		/// Айди пользователя
		/// </summary>
		public Guid UserId { get; set; }
	}
}
