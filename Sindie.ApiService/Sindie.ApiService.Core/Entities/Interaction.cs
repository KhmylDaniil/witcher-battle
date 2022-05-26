using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Взаимодействие
	/// </summary>
	public class Interaction : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_game"/>
		/// </summary>
		public const string GameField = nameof(_game);

		private Game _game;

		/// <summary>
		/// Поле для <see cref="_imgFile"/>
		/// </summary>
		public const string ImgFileField = nameof(_imgFile);

		private ImgFile _imgFile;

		/// <summary>
		/// Поле для <see cref="_textFile"/>
		/// </summary>
		public const string TextFileField = nameof(_textFile);

		private TextFile _textFile;

		/// <summary>
		/// Поле для <see cref="_scenarioReturn"/>
		/// </summary>
		public const string ScenarioReturnField = nameof(_scenarioReturn);

		private Script _scenarioReturn;

		/// <summary>
		/// Поле для <see cref="_scenarioVictory"/>
		/// </summary>
		public const string ScenarioVictoryField = nameof(_scenarioVictory);

		private Script _scenarioVictory;

		/// <summary>
		/// Поле для <see cref="_scenarioLoot"/>
		/// </summary>
		public const string ScenarioLootField = nameof(_scenarioLoot);

		private Script _scenarioLoot;

		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; protected set; }

		/// <summary>
		/// Айди графического файла
		/// </summary>
		public Guid? ImgFileId { get; protected set; }

		/// <summary>
		/// Айди текстового файла
		/// </summary>
		public Guid? TextFileId { get; protected set; }

		/// <summary>
		/// Айди сценария завершения взаимодействия
		/// </summary>
		public Guid? ScenarioReturnId { get; protected set; }

		/// <summary>
		/// Айди сценария победы во взаимодействии
		/// </summary>
		public Guid? ScenarioVictoryId { get; protected set; }

		/// <summary>
		/// Айди сценария лута во взаимодействии
		/// </summary>
		public Guid? ScenarioLootId { get; protected set; }

		/// <summary>
		/// Название взаимодействия
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание взаимодействия
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Можно ли выйти из взаимодействия
		/// </summary>
		public bool CanGiveUp { get; set; }

		/// <summary>
		/// Максимальное количество раундов взаимодействия
		/// </summary>
		public int RoundLimit { get; set; }

		#region navigation properties

		/// <summary>
		/// Игра
		/// </summary>
		public Game Game
		{
			get => _game;
			set
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
				ImgFileId = value.Id;
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
				TextFileId = value.Id;
			}
		}

		/// <summary>
		/// Сценарий завершения взаимодействия
		/// </summary>
		public Script ScenarioReturn
		{
			get => _scenarioReturn;
			set
			{
				_scenarioReturn = value;
				ScenarioReturnId = value.Id;
			}
		}

		/// <summary>
		/// Сценарий победы во взаимодействии
		/// </summary>
		public Script ScenarioVictory
		{
			get => _scenarioVictory;
			set
			{
				_scenarioVictory = value;
				ScenarioVictoryId = value.Id;
			}
		}

		/// <summary>
		/// Сценарий лута во взаимодействии
		/// </summary>
		public Script ScenarioLoot
		{
			get => _scenarioLoot;
			set
			{
				_scenarioLoot = value;
				ScenarioLootId = value.Id;
			}
		}

		/// <summary>
		/// Характеристики
		/// </summary>
		public List<Characteristic> Characteristics { get; set; }

		/// <summary>
		/// Деятельности
		/// </summary>
		public List<Activity> Activities { get; set; }

		/// <summary>
		/// Действия
		/// </summary>
		public List<Action> Actions { get; set; }

		/// <summary>
		/// Стороны
		/// </summary>
		public List<Party> Parties { get; set; }

		/// <summary>
		/// Роли
		/// </summary>
		public List<InteractionsRole> InteractionsRoles { get; set; }

		#endregion navigation properties
	}
}
