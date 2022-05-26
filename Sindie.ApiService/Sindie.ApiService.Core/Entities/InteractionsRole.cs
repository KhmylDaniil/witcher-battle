using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Роль во взаимодействии
	/// </summary>
	public class InteractionsRole : EntityBase
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
		/// Поле для <see cref="_scenarioPrerequisites"/>
		/// </summary>
		public const string ScenarioPrerequisitesField = nameof(_scenarioPrerequisites);

		private Script _scenarioPrerequisites;

		/// <summary>
		/// Поле для <see cref="_scenarioCharacteristics"/>
		/// </summary>
		public const string ScenarioCharacteristicsField = nameof(_scenarioCharacteristics);

		private Script _scenarioCharacteristics;

		/// <summary>
		/// Поле для <see cref="_scenarioInitiative"/>
		/// </summary>
		public const string ScenarioInitiativeField = nameof(_scenarioInitiative);

		private Script _scenarioInitiative;

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
		/// Айди взаимодействия для роли
		/// </summary>
		public Guid InteractionId { get; protected set; }

		/// <summary>
		/// Айди графического файла для роли
		/// </summary>
		public Guid? ImgFileId { get; protected set; }

		/// <summary>
		/// Айди текстового файла для роли
		/// </summary>
		public Guid? TextFileId { get; protected set; }

		/// <summary>
		/// Айди сценария выхода из взаимодействия для роли
		/// </summary>
		public Guid? ScenarioReturnId { get; protected set; }

		/// <summary>
		/// Айди сценария пререквизитов для роли
		/// </summary>
		public Guid ScenarioPrerequisitesId { get; protected set; }

		/// <summary>
		/// Айди сценария характеристик для роли
		/// </summary>
		public Guid ScenarioCharacteristicsId { get; protected set; }

		/// <summary>
		/// Айди сценария инициативы для роли
		/// </summary>
		public Guid ScenarioInitiativeId { get; protected set; }

		/// <summary>
		/// Айди сценария победы во взаимодействии для роли
		/// </summary>
		public Guid ScenarioVictoryId { get; protected set; }

		/// <summary>
		/// Айди сценария лута во взаимодействии для роли
		/// </summary>
		public Guid ScenarioLootId { get; protected set; }

		/// <summary>
		/// Название роли
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание роли
		/// </summary>
		public string Description { get; set; }


		#region navigation properties

		/// <summary>
		/// Взаимодействие для роли
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
		/// Графический файл для роли
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
		/// Текстовый файл для роли
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
		/// Сценарий завершения взаимодействия для роли
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
		/// Сценарий пререквизитов для роли
		/// </summary>
		public Script ScenarioPrerequisites
		{
			get => _scenarioPrerequisites;
			set
			{
				_scenarioPrerequisites = value ?? throw new ApplicationException("Необходимо передать сценарий пререквизитов для роли");
				ScenarioPrerequisitesId = value.Id;
			}
		}

		/// <summary>
		/// Сценарий характеристик для роли
		/// </summary>
		public Script ScenarioCharacteristics
		{
			get => _scenarioCharacteristics;
			set
			{
				_scenarioCharacteristics = value ?? throw new ApplicationException("Необходимо передать сценарий характеристик для роли");
				ScenarioCharacteristicsId = value.Id;
			}
		}

		/// <summary>
		/// Сценарий инициативы для роли
		/// </summary>
		public Script ScenarioInitiative
		{
			get => _scenarioInitiative;
			set
			{
				_scenarioInitiative = value ?? throw new ApplicationException("Необходимо передать сценарий инициативы для роли");
				ScenarioInitiativeId = value.Id;
			}
		}

		/// <summary>
		/// Сценарий победы во взаимодействии для роли
		/// </summary>
		public Script ScenarioVictory
		{
			get => _scenarioVictory;
			set
			{
				_scenarioVictory = value ?? throw new ApplicationException("Необходимо передать сценарий победы во взаимодействии для роли");
				ScenarioVictoryId = value.Id;
			}
		}

		/// <summary>
		/// Сценарий лута во взаимодействии для роли
		/// </summary>
		public Script ScenarioLoot
		{
			get => _scenarioLoot;
			set
			{
				_scenarioLoot = value ?? throw new ApplicationException("Необходимо передать сценарий лута во взаимодействии для роли");
				ScenarioLootId = value.Id;
			}
		}

		/// <summary>
		/// Роли стороны
		/// </summary>
		public List<PartyInteractionsRole> PartyInteractionsRoles { get; set; }

		/// <summary>
		/// Взаимодействия с предметом
		/// </summary>
		public List<InteractionItem> InteractionItems { get; set; }

		/// <summary>
		/// Деятельности роли
		/// </summary>
		public List<InteractionsRoleActivity> InteractionsRoleActivities { get; set; }

		#endregion navigation properties
	}
}