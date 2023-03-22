using Witcher.Core.BaseData;
using System;
using System.Collections.Generic;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Роль в системе
	/// </summary>
	public class SystemRole : EntityBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		protected SystemRole()
		{
		}

		/// <summary>
		/// Конструктор сущности роль в системе
		/// Использовать только в конфигурациях
		/// </summary>
		/// <param name="name">Название роли</param>
		/// <param name="id">Ид</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Айди создавшего пользователя</param>
		/// <param name="modifiedByUserId">Айди изменившего пользователя</param>
		/// <param name="roleCreatedUser">Роль создавшего пользователя</param>
		/// <param name="roleModifiedUser">Роль изменившего пользователя</param>
		public SystemRole(
			string name,
			Guid id,
			DateTime createdOn,
			DateTime modifiedOn,
			Guid createdByUserId,
			Guid modifiedByUserId,
			string roleCreatedUser = "Default",
			string roleModifiedUser = "Default")
			: base(
				  id,
				  createdOn,
				  modifiedOn,
				  createdByUserId,
				  modifiedByUserId,
				  roleCreatedUser,
				  roleModifiedUser)
		{
			Name = name;
		}

		/// <summary>
		/// Название роли в системе
		/// </summary>
		public string Name { get; set; }

		#region navigation properties

		/// <summary>
		/// Роли пользователей
		/// </summary>
		public List<UserRole> UserRoles { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="name">Название интерфейса</param>
		/// <param name="id">Ид</param>
		[Obsolete("Только для тестов")]
		public static SystemRole CreateForTest(
			string name = default,
			Guid? id = default)
		=> new()
		{
			Name = name ?? SystemRoles.UserRoleName,
			Id = id ?? SystemRoles.UserRoleId,
		};
	}
}
