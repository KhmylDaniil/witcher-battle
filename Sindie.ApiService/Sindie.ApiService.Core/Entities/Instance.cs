using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Экземпляр
	/// </summary>
	public class Instance: EntityBase
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
		protected Instance()
		{ 
		}

		/// <summary>
		/// Конструктор экземпляра
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="name">Название экземпляра</param>
		/// <param name="description">Описание экземпляра</param>
		public Instance(
			Game game,
			ImgFile imgFile,
			string name,
			string description)
		{
			Game = game;
			ImgFile = imgFile;
			Name = name;
			Description = description;
			Bags = new List<Bag>();
			Characters = new List<Character>();
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
		/// Название экземпляра
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание экземпляра
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Время активации экземпляра
		/// </summary>
		public DateTime? ActivationTime { get; set; }

		/// <summary>
		/// Дата проведения игры
		/// </summary>
		public DateTime? DateOfGame { get; set; }

		/// <summary>
		/// Описание правил игры
		/// </summary>
		public string StoryAboutRules { get; set; }

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
		/// Сумки
		/// </summary>
		public List<Bag> Bags { get; set; }

		/// <summary>
		/// Персонажи
		/// </summary>
		public List<Character> Characters { get; set; }

		/// <summary>
		/// Существа
		/// </summary>
		public List<Creature> Creatures { get; set; }

		#endregion navigation properties

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
		/// <param name="dateOfGame">Дата игры</param>
		/// <param name="storyAboutRules">Описание правил</param>
		/// <returns></returns>
		[Obsolete("Только для тестов")]
		public static Instance CreateForTest(
			Guid? id = default,
			Game game = default,
			ImgFile imgFile = default,
			UserGame userGameActivated = default,
			DateTime activationTime = default,
			string name = default,
			string description = default,
			DateTime dateOfGame = default,
			string storyAboutRules = default)
		=> new Instance()
		{
			Id = id ?? Guid.NewGuid(),
			Game = game,
			ImgFile = imgFile,
			UserGameActivated = userGameActivated,
			ActivationTime = activationTime,
			Name = name ?? "Экземпляр",
			Description = description,
			DateOfGame = dateOfGame,
			StoryAboutRules = storyAboutRules
		};
	}
}
