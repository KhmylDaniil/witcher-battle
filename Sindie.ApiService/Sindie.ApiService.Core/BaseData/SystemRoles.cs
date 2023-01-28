using System;

namespace Sindie.ApiService.Core.BaseData
{
	/// <summary>
	/// Роли справочник
	/// </summary>
	public static class SystemRoles
	{
		/// <summary>
		/// Гуид роли админ
		/// </summary>
		public static readonly Guid AdminRoleId = new("8094e0d0-3147-4791-9053-9667cbe107d7");

		/// <summary>
		/// Название роли админ
		/// </summary>
		public static readonly string AdminRoleName = "AdminRole";

		/// <summary>
		/// Гуид роли пользователь
		/// </summary>
		public static readonly Guid UserRoleId = new("8094e0d0-3148-4791-9053-9667cbe107d8");

		/// <summary>
		/// Название роли пользователь
		/// </summary>
		public static readonly string UserRoleName = "UserRole";
	}
}
