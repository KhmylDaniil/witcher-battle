using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Скрипт
	/// </summary>
	public class Script : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_game"/>
		/// </summary>
		public const string GameField = nameof(_game);

		private Game _game;

		/// <summary>
		/// Конструктор для EF
		/// </summary>
		protected Script()
		{
		}

		/// <summary>
		/// Конструктор скрипта
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="bodyScript">Тело скрипта</param>
		/// <param name="isValid">Валидность</param>
		/// <param name="validationText">Валидационный текст скрипта</param>
		public Script(
			Game game,
			string name,
			string description,
			string bodyScript,
			bool isValid,
			string validationText = default)
		{
			Game = game;
			Name = name;
			Description = description;
			BodyScript = bodyScript;
			IsValid = isValid;
			ValidationText = validationText;
			ScriptPrerequisites = new List<ScriptPrerequisites>();
			Items = new List<Item>();
			ModifierScripts = new List<ModifierScript>();
		}

		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; protected set; }

		/// <summary>
		/// Название 
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Тело скрипта
		/// </summary>
		public string BodyScript { get; set; }

		/// <summary>
		/// Валидность
		/// </summary>
		public bool IsValid { get; set; }

		/// <summary>
		/// Валидационный текст скрипта
		/// </summary>
		public string ValidationText { get; set; }

		#region navigation properties

		/// <summary>
		/// Пререквизиты скриптов
		/// </summary>
		public List<ScriptPrerequisites> ScriptPrerequisites { get; set; }

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
		/// Предметы
		/// </summary>
		public List<Item> Items { get; set; }

		/// <summary>
		/// Скрипты модификатора
		/// </summary>
		public List<ModifierScript> ModifierScripts { get; set; }

		/// <summary>
		/// Сценарии завершения взаимодействия
		/// </summary>
		public List<Interaction> ScenarioReturnInteractions { get; set; }

		/// <summary>
		/// Сценарии победы во взаимодействии
		/// </summary>
		public List<Interaction> ScenarioVictoryInteractions { get; set; }

		/// <summary>
		/// Сценарии лута во взаимодействии
		/// </summary>
		public List<Interaction> ScenarioLootInteractions { get; set; }

		/// <summary>
		/// Сценарии завершения взаимодействия для стороны
		/// </summary>
		public List<Party> ScenarioReturnParties { get; set; }

		/// <summary>
		/// Сценарии победы во взаимодействии для стороны
		/// </summary>
		public List<Party> ScenarioVictoryParties { get; set; }

		/// <summary>
		/// Сценарии лута во взаимодействии для стороны
		/// </summary>
		public List<Party> ScenarioLootParties { get; set; }

		/// <summary>
		/// Сценарии завершения взаимодействия для роли
		/// </summary>
		public List<InteractionsRole> ScenarioReturnRoles { get; set; }

		/// <summary>
		/// Сценарии победы во взаимодействии для роли
		/// </summary>
		public List<InteractionsRole> ScenarioVictoryRoles { get; set; }

		/// <summary>
		/// Сценарии лута во взаимодействии для роли
		/// </summary>
		public List<InteractionsRole> ScenarioLootRoles { get; set; }

		/// <summary>
		/// Сценарии пререквизитов к роли
		/// </summary>
		public List<InteractionsRole> ScenarioPrerequisitesRoles { get; set; }

		/// <summary>
		/// Сценарии характеристик роли
		/// </summary>
		public List<InteractionsRole> ScenarioCharacteristicsRoles { get; set; }

		/// <summary>
		/// Сценарии инициативы роли
		/// </summary>
		public List<InteractionsRole> ScenarioInitiativeRoles { get; set; }

		/// <summary>
		/// Действие
		/// </summary>
		public Action Action { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность с заполненными полями (НЕ ВСЕМИ)
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="bodyScript">Тело скрипта</param>
		/// <param name="isValid">Валидность</param>
		/// <param name="validationText">Валидационный текст</param>
		/// <param name="game">Игра</param>
		/// <returns>Скрипт</returns>
		[Obsolete("Только для тестов")]
		public static Script CreateForTest(
			Guid? id = default,
			string name = default,
			string description = default,
			string bodyScript = default,
			bool isValid = default,
			string validationText = default,
			Game game = default)
		=> new Script()
		{
			Id = id ?? Guid.NewGuid(),
			Name = name ?? "Script",
			Description = description,
			BodyScript = bodyScript ?? "BodyScript",
			IsValid = isValid,
			ValidationText = validationText,
			_game = game,
			ModifierScripts = new List<ModifierScript>()
		};
	}
}
