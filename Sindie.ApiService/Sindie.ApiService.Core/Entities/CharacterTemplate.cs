using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Шаблон персонажа
	/// </summary>
	public class CharacterTemplate : EntityBase
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
		/// Поле для <see cref="_interface"/>
		/// </summary>
		public const string InterfaceField = nameof(_interface);

		private Game _game;
		private ImgFile _imgFile;
		private Interface _interface;

		/// <summary>
		/// Конструктор для EF
		/// </summary>
		protected CharacterTemplate()
		{
		}

		/// <summary>
		/// Конструктор класса Шаблон персонажа
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="interface">Интерфейс</param>
		/// <param name="name">Название шаблона</param>
		/// <param name="description">Описание шаблона</param>
		public CharacterTemplate(
			Game game,
			ImgFile imgFile,
			Interface @interface,
			string name,
			string description)
		{
			Game = game;
			ImgFile = imgFile;
			Interface = @interface;
			Name = name;
			Description = description;
			CharacterTemplateModifiers = new List<CharacterTemplateModifier>();
			CharacterTemplateSlots = new List<CharacterTemplateSlot>();
			Characters = new List<Character>();
		}

		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; protected set; }

		/// <summary>
		/// Айди графического файла (аватарки)
		/// </summary>
		public Guid? ImgFileId { get; protected set; }

		/// <summary>
		/// Айди интерфейса
		/// </summary>
		public Guid? InterfaceId { get; protected set; }

		/// <summary>
		/// Название шаблона
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание шаблона
		/// </summary>
		public string Description { get; set; }

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
		/// Интерфейс
		/// </summary>
		public Interface Interface
		{
			get => _interface;
			set
			{
				_interface = value;
				InterfaceId = value?.Id;
			}
		}

		/// <summary>
		/// Модификаторы шаблонов персонажа
		/// </summary>
		public List<CharacterTemplateModifier> CharacterTemplateModifiers { get; set; }

		/// <summary>
		/// Слоты шаблонов персонажа
		/// </summary>
		public List<CharacterTemplateSlot> CharacterTemplateSlots { get; set; }

		/// <summary>
		/// Персонажи
		/// </summary>
		public List<Character> Characters { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="game">Игра</param>
		/// <param name="interface">Интерфейс</param>
		/// <returns>Шаблон персонажа</returns>
		[Obsolete("Только для тестов")]
		public static CharacterTemplate CreateForTest(
			Guid? id = default,
			string name = default,
			string description = default,
			ImgFile imgFile = default,
			Game game = default,
			Interface @interface = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new CharacterTemplate()
		{
			Id = id ?? Guid.NewGuid(),
			Name = name ?? "CharacterTemplate",
			Description = description,
			ImgFile = imgFile,
			Game = game,
			Interface = @interface,
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId,
			CharacterTemplateModifiers = new List<CharacterTemplateModifier>(),
			CharacterTemplateSlots = new List<CharacterTemplateSlot>(),
			Characters = new List<Character>()
		};

		/// <summary>
		/// Создание шаблона персонажа
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="interface">Интерфейс</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <returns>Шаблон персонажа</returns>
		public static CharacterTemplate CreateCharacterTemplate(
			Game game,
			ImgFile imgFile,
			Interface @interface,
			string name,
			string description)
			=> new CharacterTemplate(
				name: name,
				description: description,
				imgFile: imgFile,
				game: game,
				@interface: @interface);

		/// <summary>
		/// Изменение шаблона персонажа
		/// </summary>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="interface">Интерфейс</param>
		public void ChangeCharacterTemplate(
			string name,
			string description,
			ImgFile imgFile,
			Interface @interface)
		{
			Name = name;
			Description = description;
			ImgFile = imgFile;
			Interface = @interface;
		}

		/// <summary>
		/// Метод изменения списка модификаторов шаблонов персонажей
		/// </summary>
		/// <param name="request">Запрос</param>
		internal void ChangeCharacterTemplateModifiersList(IEnumerable<Modifier> request)
		{
			if (CharacterTemplateModifiers == null)
				throw new ExceptionEntityNotFound<List<CharacterTemplateModifier>>("Модификаторы шаблонов персонажа");

			if (request != null || CharacterTemplateModifiers.Any())
			{
				var entitiesToDelete = CharacterTemplateModifiers
					.Where(x => !request.Any(y => y.Id == x.ModifierId)).ToList();

				var entitiesToAdd = request.Where(x => !CharacterTemplateModifiers
					.Any(y => y.ModifierId == x.Id)).ToList();

				if (entitiesToAdd.Any())
					foreach (var entity in entitiesToAdd)
						CharacterTemplateModifiers.Add(
							new CharacterTemplateModifier(entity, this));

				if (entitiesToDelete.Any())
					foreach (var entity in entitiesToDelete)
						CharacterTemplateModifiers.Remove(entity);
			}
		}
	}
}
