using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Аккаунт пользователя
	/// </summary>
	public class UserAccount : EntityBase
	{
		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected UserAccount()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="login">Логин</param>
		/// <param name="passwordHash">Пароль</param>
		/// <param name="user">Пользователь</param>
		public UserAccount(
			string login,
			string passwordHash,
			User user,
			HasUsersWithLogin hasUsersWithLogin)
		{
			SetLogin(login, hasUsersWithLogin);
			UserId = user.Id;
			Login = login;
			PasswordHash = passwordHash;
			User = user;
		}

		/// <summary>
		/// Проверить, есть ли пользак с таким же логином
		/// </summary>
		/// <param name="account">Пользак</param>
		/// <param name="login">Новый логин</param>
		/// <returns>Есть ли еще такие</returns>
		public delegate bool HasUsersWithLogin(UserAccount account, string login);

		/// <summary>
		/// Айди пользователя
		/// </summary>
		public Guid UserId { get; set; }

		/// <summary>
		/// Логин
		/// </summary>
		public string Login { get; set; }

		/// <summary>
		/// Пароль
		/// </summary>
		public string PasswordHash { get; set; }

		#region navigation properties

		/// <summary>
		/// Пользователь
		/// </summary>
		public User User { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Установить логин
		/// </summary>
		/// <param name="login">Логин</param>
		/// <param name="hasUsersWithLogin">Делегат проверки уникальности</param>
		protected void SetLogin(string login, HasUsersWithLogin hasUsersWithEmail)
		{
			if (hasUsersWithEmail is null)
				throw new ArgumentNullException(nameof(hasUsersWithEmail));

			var isDuplicate = hasUsersWithEmail(this, login);
			if (isDuplicate)
				throw new ExceptionRequestNotUniq<UserAccount>("Аккаунт с таким Login уже существует");

			Login = login;
		}

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="login">Логин</param>
		/// <param name="passwordHash">Пароль</param>
		/// <param name="user">Пользователь</param>
		[Obsolete("Только для тестов")]
		public static UserAccount CreateForTest(
			string login = default,
			string passwordHash = default,
			User user = default)
			=> new UserAccount()
			{
				Login = login ?? "login",
				PasswordHash = passwordHash ?? "passwordHash",
				User = user
			};
	}
}
