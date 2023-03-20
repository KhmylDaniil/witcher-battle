using Witcher.Core.Abstractions;
using Witcher.Core.Requests.BodyTemplateRequests;
using System;
using System.Collections.Generic;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Шаблон тела
	/// </summary>
	public class BodyTemplate: EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_game"/>
		/// </summary>
		public const string GameField = nameof(_game);

		private Game _game;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected BodyTemplate()
		{
		}

		/// <summary>
		/// Конструктор шаблона тела
		/// </summary>
		public BodyTemplate(
			Game game,
			string name,
			string description,
			IEnumerable<IBodyTemplatePartData> bodyTemplateParts)
		{
			Game = game;
			Name = name;
			Description = description;
			CreateBodyTemplateParts(bodyTemplateParts);
		}

		/// <summary>
		/// Название
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание
		/// </summary>
		public string Description { get; set; }

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
		/// Список частей шаблона тела
		/// </summary>
		public List<BodyTemplatePart> BodyTemplateParts { get; set; }

		/// <summary>
		/// Шаблоны существ
		/// </summary>
		public List<CreatureTemplate> CreatureTemplates { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="game">Игра</param>
		/// <returns>Шаблон тела</returns>
		[Obsolete("Только для тестов")]
		public static BodyTemplate CreateForTest(
			Guid? id = default,
			string name = default,
			string description = default,
			Game game = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
			=> new BodyTemplate()
			{
				Id = id ?? Guid.NewGuid(),
				Name = name ?? "BodyTemplate",
				Description = description,
				Game = game,
				CreatedOn = createdOn,
				ModifiedOn = modifiedOn,
				CreatedByUserId = createdByUserId,
				CreatureTemplates = new List<CreatureTemplate>(),
				BodyTemplateParts = new List<BodyTemplatePart>()
			};

		/// <summary>
		/// Создание списка шаблонов частей тела
		/// </summary>
		/// <param name="bodyTemplateParts">Данные для списка шаблонов частей тела</param>
		/// <returns>Список шаблонов частей тела</returns>
		public void CreateBodyTemplateParts(IEnumerable<IBodyTemplatePartData> bodyTemplateParts)
		{
			BodyTemplateParts = new List<BodyTemplatePart>();
			
			foreach (var part in bodyTemplateParts)
				BodyTemplateParts.Add(new BodyTemplatePart(
					bodyTemplate: this,
					bodyPartType: part.BodyPartType,
					name: part.Name,
					damageModifier: part.DamageModifier,
					hitPenalty: part.HitPenalty,
					minToHit: part.MinToHit,
					maxToHit: part.MaxToHit));
		}

		/// <summary>
		/// Изменение шаблона тела
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="bodyTemplateParts">Данные для списка шаблонов частей тела</param>
		public void ChangeBodyTemplate(
			Game game,
			string name,
			string description,
			IEnumerable<IBodyTemplatePartData> bodyTemplateParts)
		{
			Game = game;
			Name = name;
			Description = description;

			if (bodyTemplateParts == null) return;
			
			CreateBodyTemplateParts(bodyTemplateParts);
		}
	}
}
