using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Параметр
	/// </summary>
	public class Parameter : Prerequisite
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
		/// <param name="imgFile">Графический файл</param>
		/// <param name="name">Имя</param>
		/// <param name="description">Описание</param>
		public Parameter(
			Game game,
			double minValueParameter,
			double maxValueParameter,
			ImgFile imgFile,
			string name,
			string description)
			: base(
				  imgFile,
				  name,
				  description)
		{
			Game = game;
			ParameterBounds = new ParameterBound(
				minValueParameter,
				maxValueParameter);
		}

		/// <summary>
		/// Айди Игры
		/// </summary>
		public Guid GameId { get; protected set; }

		/// <summary>
		/// Границы модификатора
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
	}
}
