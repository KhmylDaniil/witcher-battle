using Sindie.ApiService.Core.Abstractions;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Игра
	/// </summary>
	public class Game : EntityBase, IEntityWithFiles
	{
		/// <summary>
		/// Поле для <see cref="_avatar"/>
		/// </summary>
		public const string AvatarField = nameof(_avatar);

		private ImgFile _avatar;

		/// <summary>
		/// Пустой конструктор для игры
		/// </summary>
		protected Game()
		{
		}

		/// <summary>
		/// Конструктор для игры
		/// </summary>
		/// <param name="name">Название игры</param>
		/// <param name="description">Описание игры</param>
		/// <param name="avatar">Аватар игры</param>
		public Game(
			string name,
			string description,
			ImgFile avatar
			)
		{
			Name = name;
			Description = description;
			Avatar = avatar;
			UserGames = new List<UserGame>();
			ImgFiles = new List<ImgFile>();
			TextFiles = new List<TextFile>();
			Battles = new List<Battle>();
		}

		/// <summary>
		/// Название игры
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Айди Аватара
		/// </summary>
		public Guid? AvatarId { get; protected set; }

		/// <summary>
		/// Описание игры
		/// </summary>
		public string Description { get; set; }

		#region navigation properties

		/// <summary>
		/// Пользователи игры
		/// </summary>
		public List<UserGame> UserGames { get; set; }

		/// <summary>
		/// Текстовые файлы
		/// </summary>
		public List<TextFile> TextFiles { get; set; }
		
		/// <summary>
		/// Графические файлы
		/// </summary>
		public List<ImgFile> ImgFiles { get; set;}

		/// <summary>
		/// Аватар игры
		/// </summary>
		public ImgFile Avatar
		{
			get => _avatar;
			set
			{
				_avatar = value;
				AvatarId = value?.Id;
			}
		}

		/// <summary>
		/// Бои
		/// </summary>
		public List<Battle> Battles { get; set; }

		/// <summary>
		/// Шаблоны существ
		/// </summary>
		public List<CreatureTemplate> CreatureTemplates { get; set; }

		/// <summary>
		/// Шаблоны тел
		/// </summary>
		public List<BodyTemplate> BodyTemplates { get; set; }

		/// <summary>
		/// Способности
		/// </summary>
		public List<Ability> Abilities { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="id">Ид</param>
		/// <param name="name">Название игры</param>
		/// <param name="avatar">Аватара</param>
		/// <param name="description">Описание игры</param>
		/// <returns></returns>
		[Obsolete("Только для тестов")]
		public static Game CreateForTest(
			Guid? id = default,
			string name = default,
			ImgFile avatar = default,
			string description = default)
		=> new Game()
		{
			Id = id ?? default,
			Name = name ?? "Game",
			AvatarId = avatar?.Id ?? default,
			_avatar = avatar,
			Description = description,
			UserGames = new List<UserGame>(),
			ImgFiles = new List<ImgFile>(),
			TextFiles = new List<TextFile>(),
			Battles = new List<Battle>(),
		};

		/// <summary>
		/// Создание игры
		/// </summary>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="avatar">Аватар</param>
		/// <param name="user">Пользователь</param>
		/// <param name="interface">Интерфейс</param>
		/// <param name="masterRole">Роль мастера</param>
		/// <param name="mainMasterRole">Роль главного мастера</param>
		/// <returns></returns>
		public static Game CreateGame(
			string name,
			string description,
			ImgFile avatar,
			User user,
			Interface @interface,
			GameRole masterRole,
			GameRole mainMasterRole)
		{
			var newGame = new Game(
				name: name,
				description: description,
				avatar: avatar);
				
			newGame.UserGames.AddRange(new List<UserGame>()
			{
				new UserGame(user, newGame, @interface, masterRole),
				new UserGame(user, newGame, @interface, mainMasterRole)
			});

			return newGame;
		}

		/// <summary>
		/// Изменение игры
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="avatar">Айди аватара</param>
		/// <returns></returns>
		public void ChangeGame(
			string name,
			string description,
			ImgFile avatar)
		{
			Name = name;
			Description = description;
			Avatar = avatar;
		}
	}
}
