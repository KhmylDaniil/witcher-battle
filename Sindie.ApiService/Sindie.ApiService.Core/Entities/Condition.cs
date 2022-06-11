using Sindie.ApiService.Core.Abstractions;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Состояние
	/// </summary>
	public class Condition : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_game"/>
		/// </summary>
		public const string GameField = nameof(_game);

		private Game _game;

		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

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
		/// Применяемые условия
		/// </summary>
		public List<AppliedCondition> AppliedConditions { get; set; }

		/// <summary>
		/// Существа
		/// </summary>
		public List<Creature> Creatures { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="name">Название</param>
		/// <param name="game">Игра</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns>Состояние</returns>
		[Obsolete("Только для тестов")]
		public static Condition CreateForTest(
			Guid? id = default,
			string name = default,
			Game game = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
		=> new Condition()
		{
			Id = id ?? Guid.NewGuid(),
			Name = name ?? "Condition",
			Game = game,
			CreatedOn = createdOn,
			ModifiedOn = modifiedOn,
			CreatedByUserId = createdByUserId,
			Creatures = new List<Creature>(),
			AppliedConditions = new List<AppliedCondition>()
		};
	}
}
