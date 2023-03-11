using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Бой
	/// </summary>
	public class Battle: EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_game"/>
		/// </summary>
		public const string GameField = nameof(_game);

		/// <summary>
		/// Поле для <see cref="_imgFile"/>
		/// </summary>
		public const string ImgFileField = nameof(_imgFile);

		/// <summary>
		/// Поле для <see cref="_userGameActivated"/>
		/// </summary>
		public const string UserGameActivatedField = nameof(_userGameActivated);

		private Game _game;
		private ImgFile _imgFile;
		private UserGame _userGameActivated;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected Battle()
		{ 
		}

		/// <summary>
		/// Конструктор боя
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="name">Название экземпляра</param>
		/// <param name="description">Описание экземпляра</param>
		public Battle(
			Game game,
			ImgFile imgFile,
			string name,
			string description)
		{
			Game = game;
			ImgFile = imgFile;
			Name = name;
			Description = description;
			Creatures = new List<Creature>();
		}

		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; protected set; }

		/// <summary>
		/// Айди графического файла
		/// </summary>
		public Guid? ImgFileId { get; protected set; }

		/// <summary>
		/// Айди активировашего игру пользователя
		/// </summary>
		public Guid? UserGameActivatedId { get; protected set; }

		/// <summary>
		/// Название боя
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание боя
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Значение инициативы следующего существа
		/// </summary>
		public int NextInitiative { get; set; } = 1;

		#region navigation properties

		/// <summary>
		/// Игра
		/// </summary>
		public Game Game
		{
			get => _game;
			protected set
			{
				_game = value ?? throw new ApplicationException("Необходимо передать игру");
				GameId = value.Id;
			}
		}

		/// <summary>
		/// Графический файл
		/// </summary>
		public ImgFile ImgFile
		{
			get => _imgFile;
			set
			{
				_imgFile = value;
				ImgFileId = value?.Id;
			}
		}

		/// <summary>
		/// Активировавший игру пользователь
		/// </summary>
		public UserGame UserGameActivated
		{
			get => UserGameActivated;
			set
			{
				_userGameActivated = value;
				UserGameActivatedId = value?.Id;
			}
		}

		/// <summary>
		/// Существа
		/// </summary>
		public List<Creature> Creatures { get; set; }

		#endregion navigation properties

		internal void ChangeBattle(string name, string description, ImgFile imgFile)
		{
			Name = name;
			Description = description;
			ImgFile = imgFile;
		}

		/// <summary>
		/// Создание тестовой сущности с заполненными полями
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="game">Игра</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="userGameActivated">Активировавший пользователь</param>
		/// <param name="activationTime">Дата активации</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <returns></returns>
		[Obsolete("Только для тестов")]
		public static Battle CreateForTest(
			Guid? id = default,
			Game game = default,
			ImgFile imgFile = default,
			UserGame userGameActivated = default,
			string name = default,
			string description = default,
			int nextInitiative = default)
		=> new Battle()
		{
			Id = id ?? Guid.NewGuid(),
			Game = game,
			ImgFile = imgFile,
			UserGameActivated = userGameActivated,
			Name = name ?? "Экземпляр",
			Description = description,
			NextInitiative = nextInitiative
		};
	}
}
