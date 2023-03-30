using Witcher.Core.BaseData;
using System;
using System.Collections.Generic;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Интерфейс
	/// </summary>
	public class Interface : EntityBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		protected Interface()
		{
		}

		/// <summary>
		/// Конструктор сущности интерфейс
		/// Использовать только в конфигурациях
		/// </summary>
		/// <param name="name">Название интерфейса</param>
		/// <param name="type">Тип интерфейса</param>
		/// <param name="id">Ид</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Айди создавшего пользователя</param>
		/// <param name="modifiedByUserId">Айди изменившего пользователя</param>
		/// <param name="roleCreatedUser">Роль создавшего пользователя</param>
		/// <param name="roleModifiedUser">Роль изменившего пользователя</param>
		public Interface(
			string name,
			string type,
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
			Type = type;
		}

		/// <summary>
		/// Название интерфейса
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Тип интерфейса
		/// </summary>
		public string Type { get; set; }

		#region navigation properties

		/// <summary>
		/// Пользователи интерфейса
		/// </summary>
		public List<User> Users { get; set; }

		/// <summary>
		/// Игры пользователя
		/// </summary>
		public List<UserGame> UserGames { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="name">Название интерфейса</param>
		/// <param name="id">Ид</param>
		[Obsolete("Только для тестов")]
		public static Interface CreateForTest(
			string name = default,
			string type = default,
			Guid? id = default)
		=> new()
		{
			Name = name ?? SystemInterfaces.SystemLightName,
			Type = type ?? InterfaceType.System,
			Id = id ?? SystemInterfaces.SystemLightId,
			UserGames = new List<UserGame>()
		};
	}
}
