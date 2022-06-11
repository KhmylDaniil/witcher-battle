using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Параметр
	/// </summary>
	public class Parameter : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_game"/>
		/// </summary>
		public const string GameField = nameof(_game);

		private Game _game;

		/// <summary>
		/// Пустой конструктор для EF
		/// </summary>
		protected Parameter()
		{
		}

		/// <summary>
		/// Конструктор класса Параметр
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="minValueParameter">Минимальное значение параметра</param>
		/// <param name="maxValueParameter">Максимальное значение параметра</param>
		/// <param name="name">Имя</param>
		/// <param name="description">Описание</param>
		public Parameter(
			Game game,
			int minValueParameter,
			int maxValueParameter,
			string name,
			string description)
		{
			Game = game;
			Name = name;
			Description = description;
			ParameterBounds = new ParameterBound(
				minValueParameter,
				maxValueParameter);
			CreatureParameters = new List<CreatureParameter>();
			CreatureTemplateParameters = new List<CreatureTemplateParameter>();
			Abilities = new List<Ability>();
		}

		/// <summary>
		/// Айди Игры
		/// </summary>
		public Guid GameId { get; protected set; }

		/// <summary>
		/// Название параметра
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание параметра
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Границы параметра
		/// </summary>
		public ParameterBound ParameterBounds { get;  protected set; }

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
		/// Параметры персонажа
		/// </summary>
		public List<CharacterParameter> CharacterParameters { get; set; }

		/// <summary>
		/// Параметры шаблона существа
		/// </summary>
		public List<CreatureTemplateParameter> CreatureTemplateParameters { get; set; }

		/// <summary>
		/// Параметры существа
		/// </summary>
		public List<CreatureParameter> CreatureParameters { get; set; }

		/// <summary>
		/// Способности
		/// </summary>
		public List<Ability> Abilities { get; set; } 

		#endregion navigation properties

		/// <summary>
		/// Изменение параметра
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="minValueParameter">МИнимальное значение</param>
		/// <param name="maxValueParameter">Максимальное значение</param>
		public void ChangeParameter(
			Game game,
			string name,
			string description,
			int minValueParameter,
			int maxValueParameter)
		{
			Game = game;
			Name = name;
			Description = description;
			ParameterBounds = new ParameterBound(
				minValueParameter,
				maxValueParameter);
		}

		/// <summary>
		/// Создать тестовую сущность
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="name">Название</param>
		/// <param name="game">Игра</param>
		/// <param name="description">Описание</param>
		/// <param name="minValueParameter">Минимальнок значение</param>
		/// <param name="maxValueParameter">Максимальное значение</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns>Параметр</returns>
		[Obsolete("Только для тестов")]
		public static Parameter CreateForTest(
			Guid? id = default,
			string name = default,
			Game game = default,
			string description = default,
			int minValueParameter = default,
			int maxValueParameter = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new Parameter()
		{
			Id = id ?? Guid.NewGuid(),
			Name = name ?? "Parameter",
			Game = game,
			Description = description,
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId,
			ParameterBounds = (minValueParameter == 0 && maxValueParameter == 0)
			? new ParameterBound(1, 10)
			: new ParameterBound(
				minValueParameter,
				maxValueParameter)
		};
	}
}
