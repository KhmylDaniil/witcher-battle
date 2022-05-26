using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Персонаж
	/// </summary>
	public class Character : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_instance"/>
		/// </summary>
		public const string InstanceField = nameof(_instance);

		/// <summary>
		/// Поле для <see cref="_imgFile"/>
		/// </summary>
		public const string ImgFileField = nameof(_imgFile);

		/// <summary>
		/// Поле для <see cref="_textFile"/>
		/// </summary>
		public const string TextFileField = nameof(_textFile);

		/// <summary>
		/// Поле для <see cref="_characterTemplate"/>
		/// </summary>
		public const string CharacterTemplateField = nameof(_characterTemplate);

		/// <summary>
		/// Поле для <see cref="_bag"/>
		/// </summary>
		public const string BagField = nameof(_bag);

		/// <summary>
		/// Поле для <see cref="_userGameActivated"/>
		/// </summary>
		public const string UserGameActivatedField = nameof(_userGameActivated);

		private Instance _instance;
		private ImgFile _imgFile;
		private TextFile _textFile;
		private CharacterTemplate _characterTemplate;
		private Bag _bag;
		private UserGameCharacter _userGameActivated;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected Character()
		{
		}

		/// <summary>
		/// Конструктор персонажа
		/// </summary>
		/// <param name="instance">Экземпляр</param>
		/// <param name="characterTemplate">Шабон персонажа</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="textFile">Текстовый файл</param>
		/// <param name="bag">Сумка</param>
		/// <param name="userGameActivated">Активировший персонажа пользователь</param>
		/// <param name="name">Имя</param>
		/// <param name="description">Описание</param>
		public Character(
			Instance instance,
			CharacterTemplate characterTemplate,
			ImgFile imgFile,
			TextFile textFile,
			Bag bag,
			UserGameCharacter userGameActivated,
			string name,
			string description)
		{
			Instance = instance;
			CharacterTemplate = characterTemplate;
			ImgFile = imgFile;
			TextFile = textFile;
			Bag = bag;
			UserGameActivated = userGameActivated;
			Name = name;
			Description = description;
			UserGameCharacters = new List<UserGameCharacter>();
			CharacterModifiers = new List<CharacterModifier>();
			CharacterParameters = new List<CharacterParameter>();
			Bodies = new List<Body>();
		}

		/// <summary>
		/// Айди экземпляра игры
		/// </summary>
		public Guid InstanceId { get; protected set; }

		/// <summary>
		/// Айди шаблона персонажа
		/// </summary>
		public Guid? CharacterTemplateId { get; protected set; }

		/// <summary>
		/// Айди графического файла
		/// </summary>
		public Guid? ImgFileId { get; protected set; }

		/// <summary>
		/// Айди текстового файла
		/// </summary>
		public Guid? TextFileId { get; protected set; }

		/// <summary>
		/// Айди сумки
		/// </summary>
		public Guid? BagId { get; protected set; }

		/// <summary>
		/// Айди активировавшего пользователя
		/// </summary>
		public Guid? UserGameActivatedId { get; protected set; }

		/// <summary>
		/// Время активации экземпляра
		/// </summary>
		public DateTime? ActivationTime { get; set; }

		/// <summary>
		/// Имя персонажа
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание персонажа
		/// </summary>
		public string Description { get; set; }

		#region navigation properties

		/// <summary>
		/// Экземпляр
		/// </summary>
		public Instance Instance
		{
			get => _instance;
			set
			{
				_instance = value ?? throw new ApplicationException("Необходимо передать экземпляр игры");
				InstanceId = value.Id;
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
		/// Текстовый файл
		/// </summary>
		public TextFile TextFile
		{
			get => _textFile;
			set
			{
				_textFile = value;
				TextFileId = value?.Id;
			}
		}

		/// <summary>
		/// Шаблон персонажа
		/// </summary>
		public CharacterTemplate CharacterTemplate
		{
			get => _characterTemplate;
			set
			{
				_characterTemplate = value;
				CharacterTemplateId = value?.Id;
			}
		}

		/// <summary>
		/// Сумка
		/// </summary>
		public Bag Bag
		{
			get => _bag;
			set
			{
				_bag = value;
				BagId = value?.Id;
			}
		}

		/// <summary>
		/// Активировавший персонажа пользователь игры
		/// </summary>
		public UserGameCharacter UserGameActivated
		{
			get => _userGameActivated;
			set
			{
				_userGameActivated = value;
				UserGameActivatedId = value?.Id;
			}
		}

		/// <summary>
		/// Персонажи пользователя игры
		/// </summary>
		public List<UserGameCharacter> UserGameCharacters { get; set; }

		/// <summary>
		/// Параметры персонажа
		/// </summary>
		public List<CharacterParameter> CharacterParameters { get; set; }

		/// <summary>
		/// Модификаторы персонажа
		/// </summary>
		public List<CharacterModifier> CharacterModifiers { get; set; }

		/// <summary>
		/// Тела
		/// </summary>
		public List<Body> Bodies { get; set; }

		/// <summary>
		/// Уведомления о предложении передать предметы
		/// </summary>
		public List<NotificationTradeRequest> NotificationTradeRequests { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создание тестовой сущности с заполненными полями
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="instance">Экземпляр</param>
		/// <param name="characterTemplate">Шаблон персонажа</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="textFile">Текстовый файл</param>
		/// <param name="bag">Сумка</param>
		/// <param name="userGameActivated">Активировавший персонажа пользователь игры</param>
		/// <param name="name">Имя</param>
		/// <param name="description">Описание</param>
		/// <returns></returns>
		[Obsolete("Только для тестов")]
		public static Character CreateForTest(
			Guid? id = default,
			Instance instance = default,
			CharacterTemplate characterTemplate = default,
			ImgFile imgFile = default,
			TextFile textFile = default,
			Bag bag = default,
			UserGameCharacter userGameActivated = default,
			string name = default,
			string description = default)
		=> new Character()
		{
			Id = id ?? Guid.NewGuid(),
			Instance = instance,
			CharacterTemplate = characterTemplate,
			ImgFile = imgFile,
			TextFile = textFile,
			Bag = bag,
			UserGameActivated = userGameActivated,
			Name = name ?? "Character",
			Description = description	
		};
	}
}
