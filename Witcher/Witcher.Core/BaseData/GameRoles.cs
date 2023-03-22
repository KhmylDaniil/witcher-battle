using System;

namespace Witcher.Core.BaseData
{
	/// <summary>
	/// Роли в игре справочник
	/// </summary>
	public static class GameRoles
	{
		/// <summary>
		/// Гуид роли главмастер
		/// </summary>
		public static readonly Guid MainMasterRoleId = new("8094e0d0-3147-4791-9053-9667cbe127d7");

		/// <summary>
		/// Название роли главмастер
		/// </summary>
		public static readonly string MainMasterRoleName = "MainMaster";

		/// <summary>
		/// Гуид роли мастер
		/// </summary>
		public static readonly Guid MasterRoleId = new("8094e0d0-3147-4791-9053-9667cbe117d7");

		/// <summary>
		/// Название роли мастер
		/// </summary>
		public static readonly string MasterRoleName = "Master";

		/// <summary>
		/// Гуид роли игрок
		/// </summary>
		public static readonly Guid PlayerRoleId = new("8094e0d0-3148-4791-9053-9667cbe137d8");

		/// <summary>
		/// Название роли игрок
		/// </summary>
		public static readonly string PlayerRoleName = "Player";
	}
}
