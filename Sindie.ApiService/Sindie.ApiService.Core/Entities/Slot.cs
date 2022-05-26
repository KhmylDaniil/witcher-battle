using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Слот
	/// </summary>
	public class Slot : Prerequisite
	{
		// <summary>
		/// Поле для <see cref="_game"/>
		/// </summary>
		public const string GameField = nameof(_game);

		private Game _game;

		/// <summary>
		/// Конструктор для EF
		/// </summary>
		protected Slot()
		{
		}

		/// <summary>
		/// Конструктор класса Слот
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="name">Имя</param>
		/// <param name="description">Описание</param>
		public Slot(
			Game game,
			ImgFile imgFile,
			string name,
			string description)
			: base(
				 imgFile,
				 name,
				 description)
		{
			Game = game;
			CharacterTemplateSlots = new List<CharacterTemplateSlot>();
			Items = new List<Item>();
		}

		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; protected set; }

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
		/// Слоты шаблонов персонажа
		/// </summary>
		public List<CharacterTemplateSlot> CharacterTemplateSlots { get; set; }

		/// <summary>
		/// Предметы
		/// </summary>
		public List<Item> Items { get; set; }

		/// <summary>
		/// Тела
		/// </summary>
		public List<Body> Bodies { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="game"></param>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="slotImgFile"></param>
		/// <param name="description"></param>
		/// <returns></returns>
		[Obsolete("Только для тестов")]
		public static Slot CreateForTest(
			Game game = default,
			Guid? id = default,
			string name = default,
			ImgFile slotImgFile = default,
			string description = default)
		=> new Slot()
		{
			Id = id ?? default,
			Name = name ?? "Slot",
			ImgFile = slotImgFile,
			Description = description,
			_game = game
		};
	}
}
