using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Сторона
	/// </summary>
	public class Party : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_interaction"/>
		/// </summary>
		public const string InteractionField = nameof(_interaction);

		private Interaction _interaction;

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
		/// Айди взаимодействия для стороны
		/// </summary>
		public Guid InteractionId { get; protected set; }

		/// <summary>
		/// Айди графического файла для стороны
		/// </summary>
		public Guid? ImgFileId { get; protected set; }

		/// <summary>
		/// Айди текстового файла для стороны
		/// </summary>
		public Guid? TextFileId { get; protected set; }

		/// <summary>
		/// Айди сценария выхода из взаимодействия  для стороны
		/// </summary>
		public Guid? ScenarioReturnId { get; protected set; }

		/// <summary>
		/// Айди сценария победы во взаимодействии  для стороны
		/// </summary>
		public Guid? ScenarioVictoryId { get; protected set; }

		/// <summary>
		/// Айди сценария лута во взаимодействии  для стороны
		/// </summary>
		public Guid? ScenarioLootId { get; protected set; }

		/// <summary>
		/// Название стороны
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание стороны
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Минимальное количество персонажей
		/// </summary>
		public int MinQuantityCharacters { get; set; }

		/// <summary>
		/// Максимальное количество персонажей
		/// </summary>
		public int MaxQuantityCharacters { get; set; }


		#region navigation properties

		/// <summary>
		/// Взаимодействие
		/// </summary>
		public Interaction Interaction
		{
			get => _interaction;
			set
			{
				_interaction = value ?? throw new ApplicationException("Необходимо передать взаимодействие");
				InteractionId = value.Id;
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
		/// Сценарий завершения взаимодействия для стороны
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
		/// Сценарий победы во взаимодействии для стороны
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
		/// Сценарий лута во взаимодействии для стороны
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
		/// Роли стороны
		/// </summary>
		public List<PartyInteractionsRole> PartyInteractionsRoles { get; set; }

		#endregion navigation properties
	}
}