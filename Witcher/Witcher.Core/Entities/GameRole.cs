using System;
using System.Collections.Generic;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Роль в игре
	/// </summary>
	public class GameRole : EntityBase
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		protected GameRole()
		{
		}

		/// <summary>
		/// Конструктор только для конфигурации
		/// </summary>
		/// <param name="id">Ид</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Айди создавшего пользователя</param>
		/// <param name="modifiedByUserId">Айди изменившего пользователя</param>
		/// <param name="roleCreatedUser">Роль создавшего пользователя</param>
		/// <param name="roleModifiedUser">Роль изменившего пользователя</param>
		/// <param name="name">Название роли</param>
		public GameRole(
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
		/// Название роли в игре
		/// </summary>
		public string Name { get; set; }

		#region navigation properties

		/// <summary>
		/// Пользователи игры
		/// </summary>
		public List<UserGame> UserGames { get; set; }

		#endregion navigation properties

		[Obsolete("Только для тестов")]
		public static GameRole CreateForTest(
			Guid? id = default,
			string name = default)
			=> new GameRole()
			{
				Id = id ?? default,
				Name = name ?? "GameRole",
				UserGames = new List<UserGame>()
			};
	}
}
