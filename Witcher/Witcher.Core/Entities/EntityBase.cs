using System;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Базовые сущности
	/// </summary>
	public abstract class EntityBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		protected EntityBase()
		{
		}

		/// <summary>
		/// Конструктор для создания сущности в конфигурациях
		/// Использовать только в конфигурациях
		/// </summary>
		/// <param name="id">Ид</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="сreatedByUserId">Айди создавшего пользователя</param>
		/// <param name="modifiedByUserId">Айди изменившего пользователя</param>
		/// <param name="roleCreatedUser">Роль создавшего пользователя</param>
		/// <param name="roleModifiedUser">Роль изменившего пользователя</param>
		protected EntityBase(
			Guid id,
			DateTime createdOn,
			DateTime modifiedOn,
			Guid сreatedByUserId,
			Guid modifiedByUserId,
			string roleCreatedUser = "Default",
			string roleModifiedUser = "Default")
		{
			Id = id;
			CreatedOn = DateTime.SpecifyKind(createdOn, DateTimeKind.Utc);
			ModifiedOn = DateTime.SpecifyKind(modifiedOn, DateTimeKind.Utc);
			CreatedByUserId = сreatedByUserId;
			ModifiedByUserId = modifiedByUserId;
			RoleCreatedUser = roleCreatedUser;
			RoleModifiedUser = roleModifiedUser;
		}

		/// <summary>
		/// Базовый конструктор для создания и изменения сущности
		/// </summary>
		/// <param name="createdUser">Cоздавший пользователь</param>
		/// <param name="modifiedUser">Изменивший пользователь</param>
		/// <param name="roleCreatedUser">Роль создавшего пользователя</param>
		/// <param name="roleModifiedUser">Роль изменившего пользователя</param>
		protected EntityBase(
			User createdUser = default,
			User modifiedUser = default,
			SystemRole roleCreatedUser = default,
			SystemRole roleModifiedUser = default)
		{
			CreatedByUserId = createdUser.Id;
			ModifiedByUserId = modifiedUser.Id;
			RoleCreatedUser = roleCreatedUser.Name;
			RoleModifiedUser = roleModifiedUser.Name;
		}

		/// <summary>
		/// Айди
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Дата создания
		/// </summary>
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Дата изменения
		/// </summary>
		public DateTime ModifiedOn { get; set; }

		/// <summary>
		/// Айди создавшего пользователя
		/// </summary>
		public Guid CreatedByUserId { get; set; }

		/// <summary>
		/// Роль создавшего пользователя
		/// </summary>
		public string RoleCreatedUser { get; set; }

		/// <summary>
		/// Айди изменившего пользователя
		/// </summary>
		public Guid ModifiedByUserId { get; set; }

		/// <summary>
		/// Роль изменившего пользователя
		/// </summary>
		public string RoleModifiedUser { get; set; }
	}
}
