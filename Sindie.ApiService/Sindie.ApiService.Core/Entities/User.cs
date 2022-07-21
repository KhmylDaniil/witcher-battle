using Sindie.ApiService.Core.Abstractions;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Пользователь
	/// </summary>
	public class User : EntityBase, IEntityWithFiles
	{
		/// <summary>
		/// Конструктор
		/// </summary>
		protected User()
		{
		}

		/// <summary>
		/// Конструктор сущности пользователь для создания пользователя по умолчанию
		/// Использовать только в конфигурациях
		/// </summary>
		/// <param name="name">Имя</param>
		/// <param name="email">Емайл</param>
		/// <param name="phone">Телефон</param>
		/// <param name="id">Ид</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Айди создавшего пользователя</param>
		/// <param name="modifiedByUserId">Айди изменившего пользователя</param>
		/// <param name="interfaceId">Айди интерфейса</param>
		public User(
			Guid createdByUserId,
			string name,
			Guid id,
			DateTime createdOn,
			DateTime modifiedOn,
			Guid modifiedByUserId,
			Guid interfaceId,
			string email = default,
			string phone = default)
			: base(
				id,
				createdOn,
				modifiedOn,
				createdByUserId,
				modifiedByUserId)
		{
			Name = name;
			Email = email;
			Phone = phone;
			UserAccounts = new List<UserAccount>();
			UserRoles = new List<UserRole>();
			UserGames = new List<UserGame>();
			TextFiles = new List<TextFile>();
			ImgFiles = new List<ImgFile>();
			InterfaceId = interfaceId;
		}

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="name">Имя</param>
		/// <param name="email">Емайл</param>
		/// <param name="phone">Телефон</param>
		public User(
			string name,
			Interface @interface,
			string email = default,
			string phone = default)
		{
			Name = name;
			Email = email;
			Phone = phone;
			UserAccounts = new List<UserAccount>();
			UserRoles = new List<UserRole>();
			UserGames = new List<UserGame>();
			Interface = @interface;
			InterfaceId = @interface.Id;
			Avatar = default;
		}

		/// <summary>
		/// Имя пользователя
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Емайл пользователя
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Телефон пользователя
		/// </summary>
		public string Phone { get; set; }

		/// <summary>
		/// Интерфейс пользователя
		/// </summary>
		public Guid InterfaceId { get; set; }

		/// <summary>
		/// Айди аватара
		/// </summary>
		public Guid? AvatarId { get; set; }

		#region navigation properties

		/// <summary>
		/// Аккаунты пользователей
		/// </summary>
		public List<UserAccount> UserAccounts { get; set; }

		/// <summary>
		/// Роли пользователей
		/// </summary>
		public List<UserRole> UserRoles { get; set; }

		/// <summary>
		/// Пользователи игры(мастера)
		/// </summary>
		public List<UserGame> UserGames { get; set; }

		/// <summary>
		/// Интерфейс
		/// </summary>
		public Interface Interface { get; set; }

		/// <summary>
		/// Текстовые файлы
		/// </summary>
		public List<TextFile> TextFiles { get; set; }

		/// <summary>
		/// Графические файлы
		/// </summary>
		public List<ImgFile> ImgFiles { get; set; }

		/// <summary>
		/// Графический файл (аватар)
		/// </summary>
		public ImgFile Avatar { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="id">Ид</param>
		/// <param name="name">Имя пользователя</param>
		/// <param name="interface">Интерфейс</param>
		/// <param name="avatar">Аватар</param>
		/// <param name="email">Емайл пользователя</param>
		/// <param name="phone">Телефон пользователя</param>
		/// <returns></returns>
		[Obsolete("Только для тестов")]
		public static User CreateForTest(
			Guid? id = default,
			string name = default,
			Interface @interface = default,
			ImgFile avatar = default,
			string email = default,
			string phone = default,
			string login = default,
			string password = default,
			SystemRole role = default)
		=> new User()
		{
			Id = id ?? default,
			Name = name ?? "User",
			InterfaceId = @interface?.Id ?? default,
			Interface = @interface,
			Email = email,
			Phone = phone,
			AvatarId = avatar?.Id ?? default,
			Avatar = avatar,
			UserAccounts = new List<UserAccount>() { UserAccount.CreateForTest(login, password) },
			UserRoles = new List<UserRole>() { UserRole.CreateForTest(role: role) }
		};
	}
}
