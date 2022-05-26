using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Событие
	/// </summary>
	public class Event : Prerequisite
	{
		/// <summary>
		/// Поле для <see cref="_game"/>
		/// </summary>
		public const string GameField = nameof(_game);

		private Game _game;

		/// <summary>
		/// Конструктор для EF
		/// </summary>
		protected Event()
		{
		}

		/// <summary>
		/// Конструктор класса события
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="isActive">Событие активно?</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="name">Имя</param>
		/// <param name="description">Описание</param>
		public Event(
			Game game,
			bool isActive,
			ImgFile imgFile,
			string name,
			string description)
			: base(
				  imgFile,
				  name,
				  description)
		{
			Game = game;
			IsActive = isActive;
			ModifierScripts = new List<ModifierScript>();
		}

		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; protected set; }

		/// <summary>
		/// Событие активно ?
		/// </summary>
		public bool IsActive { get; set; }

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
		/// Скрипты модификатора
		/// </summary>
		public List<ModifierScript> ModifierScripts { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="name">Название</param>
		/// <param name="isActive">Событие активно?</param>
		/// <param name="game">Игра</param>
		/// <returns>Событие</returns>
		[Obsolete("Только для тестов")]
		public static Event CreateForTest(
			Guid? id = default,
			string name = default,
			bool isActive = default,
			Game game = default)
		=> new Event()
		{
			Id = id ?? Guid.NewGuid(),
			Name = name ?? "Event",
			_game = game,
			IsActive = isActive,
			ModifierScripts = new List<ModifierScript>()
		};
	}
}
