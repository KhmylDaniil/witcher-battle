using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Модификатор
	/// </summary>
	public class Modifier : Prerequisite
	{
		/// <summary>
		/// Поле для <see cref="_game"/>
		/// </summary>
		public const string GameField = nameof(_game);

		private Game _game;

		/// <summary>
		/// Конструктор для EF
		/// </summary>
		private Modifier()
		{
		}

		/// <summary>
		/// Конструктор для класса Модификатор
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="name">Имя</param>
		/// <param name="description">Описание</param>
		public Modifier(
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
			ModifierScripts = new List<ModifierScript>();
			ItemTemplateModifiers = new List<ItemTemplateModifier>();
			CharacterTemplateModifiers = new List<CharacterTemplateModifier>();
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
		/// Модификаторы шаблонов персонажа
		/// </summary>
		public List<CharacterTemplateModifier> CharacterTemplateModifiers { get; set; }

		/// <summary>
		/// Скрипты модификатора
		/// </summary>
		public List<ModifierScript> ModifierScripts { get; set; }

		/// <summary>
		/// Модификаторы шаблонов предметов
		/// </summary>
		public List<ItemTemplateModifier> ItemTemplateModifiers { get; set; }

		/// <summary>
		/// Модификаторы персонажа
		/// </summary>
		public List<CharacterModifier> CharacterModifiers { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создание модификатора
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <returns>Модификатор</returns>
		public static Modifier CreateModifier(
			Game game,
			ImgFile imgFile,
			string name,
			string description)
		{
			var newModifier = new Modifier(
				game: game,
				name: name,
				description: description,
				imgFile: imgFile);

			return newModifier;
		}

		/// <summary>
		/// Изменение модификатора
		/// </summary>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="imgFile">Графический файл</param>
		public void ChangeModifier(
			string name,
			string description,
			ImgFile imgFile)
		{
			Name = name;
			Description = description;
			ImgFile = imgFile;
		}

		/// <summary>
		/// Создание тестовой сущности с заполненными полями
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="game">Игра</param>
		/// <returns>Модификатор</returns>
		[Obsolete("Только для тестов")]
		public static Modifier CreateForTest(
			Guid? id = default,
			ImgFile imgFile = default,
			string name = default,
			string description = default,
			Game game = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new Modifier()
		{
			Id = id ?? Guid.NewGuid(),
			Name = name ?? "Modifier",
			ImgFileId = imgFile?.Id ?? default,
			ImgFile = imgFile,
			Description = description,
			Game = game,
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId,
			ModifierScripts = new List<ModifierScript>(),
			ItemTemplateModifiers = new List<ItemTemplateModifier>(),
			CharacterTemplateModifiers = new List<CharacterTemplateModifier>()
		};

		/// <summary>
		/// Создать список скриптов модификатора
		/// </summary>
		/// <param name="modifierScriptData">Данные для создания списка скриптов модификатора</param>
		internal void CreateModifierScriptsList(
			IEnumerable<(Script Script,
				Event Event,
				DateTime ActivationTime,
				int PeriodOfActivity,
				int PeriodOfInactivity,
				int NumberOfRepetitions)
				> modifierScriptData)
		{
			if (modifierScriptData == null)
				throw new ExceptionEntityNotFound<IEnumerable>("Данные для создания списка скриптов модификатора");

			if (ModifierScripts == null)
				throw new ExceptionEntityNotFound<List<ModifierScript>>("Скрипты модификатора");

			if (!modifierScriptData.Any())
				return;

			foreach (var dataItem in modifierScriptData)
				ModifierScripts.Add(
					ModifierScript.CreateModifierScript(
					script: dataItem.Script,
					@event: dataItem.Event,
					modifier: this,
					activationTime: dataItem.ActivationTime,
					periodOfActivity: dataItem.PeriodOfActivity,
					periodOfInactivity: dataItem.PeriodOfInactivity,
					numberOfRepetitions: dataItem.NumberOfRepetitions));
		}

		/// <summary>
		/// Обновить список скриптов модификатора
		/// </summary>
		/// <param name="modifierScriptData">Данные для обновления списка скриптов модификатора</param>
		internal void ChangeModifierScriptsList(
			IEnumerable<(Script Script,
				Event Event,
				Guid Id,
				DateTime ActivationTime,
				int PeriodOfActivity,
				int PeriodOfInactivity,
				int NumberOfRepetitions)
				> modifierScriptData)
		{
			if (modifierScriptData == null)
				throw new ExceptionEntityNotFound<IEnumerable>("Данные для обновления списка скриптов модификатора");

			if (ModifierScripts == null)
				throw new ExceptionEntityNotFound<List<ModifierScript>>("Скрипты модификатора");

			var entitiesToDelete = ModifierScripts.Where(x => !modifierScriptData
				.Any(y => y.Id == x.Id)).ToList();

			if (entitiesToDelete.Any())
				foreach (var entity in entitiesToDelete)
					ModifierScripts.Remove(entity);

			if (!modifierScriptData.Any())
				return;

			foreach (var dataItem in modifierScriptData)
			{
				var modifierScript = ModifierScripts.FirstOrDefault(x => x.Id == dataItem.Id);
				if (modifierScript == null)
					ModifierScripts.Add(
						ModifierScript.CreateModifierScript(
						script: dataItem.Script,
						@event: dataItem.Event,
						modifier: this,
						activationTime: dataItem.ActivationTime,
						periodOfActivity: dataItem.PeriodOfActivity,
						periodOfInactivity: dataItem.PeriodOfInactivity,
						numberOfRepetitions: dataItem.NumberOfRepetitions));
				else
					modifierScript.ChangeModifierScript(
						script: dataItem.Script,
						@event: dataItem.Event,
						activationTime: dataItem.ActivationTime,
						periodOfActivity: dataItem.PeriodOfActivity,
						periodOfInactivity: dataItem.PeriodOfInactivity,
						numberOfRepetitions: dataItem.NumberOfRepetitions);
			}	
		}

		/// <summary>
		/// Метод изменения списка модификаторов шаблонов предметов
		/// </summary>
		/// <param name="request">Запрос</param>
		internal void ChangeItemTemplateModifiersList(IEnumerable<ItemTemplate> request)
		{
			if (ItemTemplateModifiers == null)
				throw new ExceptionEntityNotFound<List<ItemTemplateModifier>>("Модификаторы шаблонов предметов");

			if (request != null || ItemTemplateModifiers.Any())
			{
				var entitiesToDelete = ItemTemplateModifiers
					.Where(x => !request.Any(y => y.Id == x.ItemTemplateId)).ToList();

				var entitiesToAdd = request.Where(x => !ItemTemplateModifiers
					.Any(y => y.ItemTemplateId == x.Id)).ToList();

				if (entitiesToAdd.Any())
					foreach (var entity in entitiesToAdd)
						ItemTemplateModifiers.Add(
							new ItemTemplateModifier(this, entity));

				if (entitiesToDelete.Any())
					foreach (var entity in entitiesToDelete)
						ItemTemplateModifiers.Remove(entity);
			}
		}

		/// <summary>
		/// Метод изменения списка модификаторов шаблонов персонажей
		/// </summary>
		/// <param name="request">Запрос</param>
		internal void ChangeCharacterTemplateModifiersList(IEnumerable<CharacterTemplate> request)
		{
			if (CharacterTemplateModifiers == null)
				throw new ExceptionEntityNotFound<List<CharacterTemplateModifier>>("Модификаторы шаблонов персонажа");

			if (request != null || CharacterTemplateModifiers.Any())
			{
				var entitiesToDelete = CharacterTemplateModifiers
					.Where(x => !request.Any(y => y.Id == x.CharacterTemplateId)).ToList();

				var entitiesToAdd = request.Where(x => !CharacterTemplateModifiers
					.Any(y => y.CharacterTemplateId == x.Id)).ToList();

				if (entitiesToAdd.Any())
					foreach (var entity in entitiesToAdd)
						CharacterTemplateModifiers.Add(
							new CharacterTemplateModifier(this, entity));

				if (entitiesToDelete.Any())
					foreach (var entity in entitiesToDelete)
						CharacterTemplateModifiers.Remove(entity);
			}
		}
	}
}
