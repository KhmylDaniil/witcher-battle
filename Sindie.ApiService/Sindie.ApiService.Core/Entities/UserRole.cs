using System;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Роль пользователя
	/// </summary>
	public class UserRole : EntityBase
	{
		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected UserRole()
		{
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="user">Пользователь</param>
		/// <param name="role">Роль</param>
		public UserRole(
			User user,
			SystemRole role)
		{
			UserId = user.Id;
			SystemRoleId = role.Id;
			User = user;
			SystemRole = role;
		}

		/// <summary>
		/// Айди пользователя
		/// </summary>
		public Guid UserId { get; set; }

		/// <summary>
		/// Айди роли в системе
		/// </summary>
		public Guid SystemRoleId { get; set; }

		#region navigation properties

		/// <summary>
		/// Пользователь
		/// </summary>
		public User User { get; set; }

		/// <summary>
		/// Роль в системе
		/// </summary>
		public SystemRole SystemRole { get; set; }
		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="role">Роль</param>
		/// <param name="user">Пользователь</param>
		[Obsolete("Только для тестов")]
		public static UserRole CreateForTest(
			SystemRole role = default,
			User user = default)
			=> new()
			{
				SystemRole = role,
				User = user
			};
	}
}
