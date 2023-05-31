using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Exceptions.EntityExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Exceptions.SystemExceptions;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Шаблон существа
	/// </summary>
	public class CreatureTemplate: EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_game"/>
		/// </summary>
		public const string GameField = nameof(_game);

		/// <summary>
		/// Поле для <see cref="_imgFile"/>
		/// </summary>
		public const string ImgFileField = nameof(_imgFile);

		/// <summary>
		/// Поле для <see cref="_bodyTemplate"/>
		/// </summary>
		public const string BodyTemplateField = nameof(_bodyTemplate);

		private Game _game;
		private ImgFile _imgFile;
		private BodyTemplate _bodyTemplate;

		private int _hp;
		private int _sta;
		private int _int;
		private int _ref;
		private int _dex;
		private int _body;
		private int _emp;
		private int _cra;
		private int _will;
		private int _speed;
		private int _luck;

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		protected CreatureTemplate()
		{
		}

		/// <summary>
		/// Конструктор создания шаблона существа
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="bodyTemplate">Шаблон тела</param>
		/// <param name="creatureType">Тип существа</param>
		/// <param name="hp">Хиты</param>
		/// <param name="sta">Стамина</param>
		/// <param name="int">Интеллект</param>
		/// <param name="ref">Рефлексы</param>
		/// <param name="dex">Ловкость</param>
		/// <param name="body">Телосложение</param>
		/// <param name="emp">Эмпатия</param>
		/// <param name="cra">Крафт</param>
		/// <param name="will">Воля</param>
		/// <param name="speed">Скорость</param>
		/// <param name="luck">Удача</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="armorList">Броня существа</param>
		public CreatureTemplate(
			Game game,
			ImgFile imgFile,
			BodyTemplate bodyTemplate,
			CreatureType creatureType,
			int hp,
			int sta,
			int @int,
			int @ref,
			int dex,
			int body,
			int emp,
			int cra,
			int will,
			int speed,
			int luck,
			string name, 
			string description,
			List<(BodyTemplatePart BodyTemplatePart, int Armor)> armorList)
		{
			Game = game;
			ImgFile = imgFile;
			BodyTemplate = bodyTemplate;
			CreatureType = creatureType;
			_hp = hp;
			Sta = sta;
			Int = @int;
			Ref = @ref;
			Dex = dex;
			Body = body;
			Emp = emp;
			Cra = cra;
			Will = will;
			Speed = speed;
			Luck = luck;
			Name = name;
			Description = description;
			Abilities = new List<Ability>();
			CreatureTemplateSkills = new List<CreatureTemplateSkill>();
			Creatures = new List<Creature>();
			CreatureTemplateParts = UpdateBody(armorList);
		}

		/// <summary>
		/// Айди игры
		/// </summary>
		public Guid GameId { get; protected set; }

		/// <summary>
		/// Айди графического файла
		/// </summary>
		public Guid? ImgFileId { get; protected set; }

		/// <summary>
		/// Айди шаблона тела
		/// </summary>
		public Guid BodyTemplateId { get; protected set; }

		/// <summary>
		/// Тип существа
		/// </summary>
		public CreatureType CreatureType { get; set; }

		/// <summary>
		/// Название шаблона
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание шаблона
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Хиты
		/// </summary>
		public int HP
		{
			get => _hp;
			set
			{
				if (value < 1)
					throw new FieldOutOfRangeException<Creature>(nameof(HP));
				_hp = value;
			}
		}

		/// <summary>
		/// Стамина
		/// </summary>
		public int Sta
		{
			get => _sta;
			set
			{
				if (value < 1)
					throw new FieldOutOfRangeException<Creature>(nameof(Sta));
				_sta = value;
			}
		}

		/// <summary>
		/// Интеллект
		/// </summary>
		public int Int
		{
			get => _int;
			set
			{
				if (value < 1)
					throw new FieldOutOfRangeException<Creature>(nameof(Int));
				_int = value;
			}
		}

		/// <summary>
		/// Рефлексы
		/// </summary>
		public int Ref
		{
			get => _ref;
			set
			{
				if (value < 1)
					throw new FieldOutOfRangeException<Creature>(nameof(Ref));
				_ref = value;
			}
		}

		/// <summary>
		/// Ловкость
		/// </summary>
		public int Dex
		{
			get => _dex;
			set
			{
				if (value < 1)
					throw new FieldOutOfRangeException<Creature>(nameof(Dex));
				_dex = value;
			}
		}

		/// <summary>
		/// Телосложение
		/// </summary>
		public int Body
		{
			get => _body;
			set
			{
				if (value < 1)
					throw new FieldOutOfRangeException<Creature>(nameof(Body));
				_body = value;
			}
		}

		/// <summary>
		/// Эмпатия
		/// </summary>
		public int Emp
		{
			get => _emp;
			set
			{
				if (value < 1)
					throw new FieldOutOfRangeException<Creature>(nameof(Emp));
				_emp = value;
			}
		}

		/// <summary>
		/// Крафт
		/// </summary>
		public int Cra
		{
			get => _cra;
			set
			{
				if (value < 1)
					throw new FieldOutOfRangeException<Creature>(nameof(Cra));
				_cra = value;
			}
		}

		/// <summary>
		/// Воля
		/// </summary>
		public int Will
		{
			get => _will;
			set
			{
				if (value < 1)
					throw new FieldOutOfRangeException<Creature>(nameof(Will));
				_will = value;
			}
		}

		/// <summary>
		/// Удача
		/// </summary>
		public int Luck
		{
			get => _luck;
			set
			{
				if (value < 0)
					throw new FieldOutOfRangeException<Creature>(nameof(Luck));
				_luck = value;
			}
		}

		/// <summary>
		/// Скорость
		/// </summary>
		public int Speed
		{
			get => _speed;
			set
			{
				if (value < 0)
					throw new FieldOutOfRangeException<Creature>(nameof(Speed));
				_speed = value;
			}
		}

		#region navigation properties

		/// <summary>
		/// Игра
		/// </summary>
		public Game Game
		{
			get => _game;
			protected set
			{
				_game = value ?? throw new EntityNotIncludedException<CreatureTemplate>(nameof(Game));
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
				ImgFileId = value?.Id;
			}
		}

		/// <summary>
		/// Шаблон тела
		/// </summary>
		public BodyTemplate BodyTemplate
		{
			get => _bodyTemplate;
			set
			{
				_bodyTemplate = value ?? throw new EntityNotIncludedException<CreatureTemplate>(nameof(BodyTemplate));
				BodyTemplateId = value.Id;
			}
		}

		/// <summary>
		/// Способности
		/// </summary>
		public List<Ability> Abilities { get; protected set; }

		/// <summary>
		/// Навыки шаблона существа
		/// </summary>
		public List<CreatureTemplateSkill> CreatureTemplateSkills { get; protected set; }

		/// <summary>
		/// Части шаблона существа
		/// </summary>
		public List<CreatureTemplatePart> CreatureTemplateParts { get; protected set; }

		/// <summary>
		/// Существа
		/// </summary>
		public List<Creature> Creatures { get; set; }

		/// <summary>
		/// Модификаторы типа урона
		/// </summary>
		public List<EntityDamageTypeModifier> DamageTypeModifiers { get; protected set; }

		#endregion navigation properties

		/// <summary>
		/// Обновление списка способностей
		/// </summary>
		/// <param name="data">Данные</param>
		internal void UpdateAbililities(List<Ability> request)
		{
			if (request == null)
				throw new ApplicationSystemNullException<CreatureTemplate>(nameof(request));

			if (Abilities == null)
				throw new ApplicationSystemNullException<CreatureTemplate>(nameof(Abilities));

			var entitiesToDelete = Abilities.Where(x => !request
				.Any(y => y.Id == x.Id)).ToList();

			if (entitiesToDelete.Any())
				foreach (var entity in entitiesToDelete)
					Abilities.Remove(entity);

			if (!request.Any())
				return;

			var entitiesToAdd = request.Where(x => !Abilities
				.Any(y => y.Id == x.Id)).ToList();

			foreach (var entity in entitiesToAdd)
				Abilities.Add(entity);
		}

		/// <summary>
		/// Метод изменения списка навыков шаблона существа
		/// </summary>
		/// <param name="data">Данные</param>
		internal void UpdateCreatureTemplateSkills(IEnumerable<UpdateCreatureTemplateRequestSkill> data)
		{
			if (CreatureTemplateSkills == null)
				throw new ApplicationSystemNullException<CreatureTemplate>(nameof(CreatureTemplateSkills));

			if (data == null)
				throw new ApplicationSystemNullException<CreatureTemplate>(nameof(data));

			var entitiesToDelete = CreatureTemplateSkills
					.Where(x => !data.Any(y => y.Skill == x.Skill)).ToList();

			if (entitiesToDelete.Any())
				foreach (var entity in entitiesToDelete)
					CreatureTemplateSkills.Remove(entity);

			if (!data.Any())
				return;

			foreach (var dataItem in data)
			{
				var creatureTemplateSkill = CreatureTemplateSkills.FirstOrDefault(x => x.Id == dataItem.Id);
				if (creatureTemplateSkill == null)
					CreatureTemplateSkills.Add(
						new CreatureTemplateSkill(
								skill: dataItem.Skill,
								skillValue: dataItem.Value,
								creatureTemplate: this));
				else
					creatureTemplateSkill.ChangeValue(dataItem.Value);
			}
		}

		/// <summary>
		/// изменение тела шаблона существа
		/// </summary>
		/// <param name="armorList">Броня</param>
		/// <returns>Список частей шаблона тела</returns>
		private List<CreatureTemplatePart> UpdateBody(List<(BodyTemplatePart BodyTemplatePart, int Armor)> armorList)
		{
			var bodyParts = new List<CreatureTemplatePart>();

			foreach (var part in armorList)
				bodyParts.Add(new CreatureTemplatePart(
					creatureTemplate: this,
					bodyPartType: part.BodyTemplatePart.BodyPartType,
					name: part.BodyTemplatePart.Name,
					hitPenalty: part.BodyTemplatePart.HitPenalty,
					damageModifier: part.BodyTemplatePart.DamageModifier,
					minToHit: part.BodyTemplatePart.MinToHit,
					maxToHit: part.BodyTemplatePart.MaxToHit,
					armor: part.Armor));
			return bodyParts;
		}

		/// <summary>
		/// Изменение шаблона существа
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="bodyTemplate">Шаблон тела</param>
		/// <param name="creatureType">Тип существа</param>
		/// <param name="hp">Хиты</param>
		/// <param name="sta">Стамина</param>
		/// <param name="int">Интеллект</param>
		/// <param name="ref">Рефлексы</param>
		/// <param name="dex">Ловкость</param>
		/// <param name="body">Телосложение</param>
		/// <param name="emp">Эмпатия</param>
		/// <param name="cra">Крафт</param>
		/// <param name="will">Воля</param>
		/// <param name="speed">Скорость</param>
		/// <param name="luck">Удача</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="armorList">Список брони</param>
		public void ChangeCreatureTemplate(
			Game game,
			ImgFile imgFile,
			BodyTemplate bodyTemplate,
			CreatureType creatureType,
			int hp,
			int sta,
			int @int,
			int @ref,
			int dex,
			int body,
			int emp,
			int cra,
			int will,
			int speed,
			int luck,
			string name,
			string description,
			List<(BodyTemplatePart BodyTemplatePart, int Armor)> armorList)
		{
			Game = game;
			ImgFile = imgFile;
			BodyTemplate = bodyTemplate;
			_hp = hp;
			Sta = sta;
			Int = @int;
			Ref = @ref;
			Dex = dex;
			Body = body;
			Emp = emp;
			Cra = cra;
			Will = will;
			Speed = speed;
			Luck = luck;
			Name = name;
			Description = description;
			CreatureType = creatureType;

			if (armorList is not null)
				CreatureTemplateParts = UpdateBody(armorList);
		}

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="game">Игра</param>
		/// <param name="bodyTemplate">Шаблон тела</param>
		/// <param name="creatureType">Тип существа</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="hp">Хиты</param>
		/// <param name="sta">Стамина</param>
		/// <param name="int">Интеллект</param>
		/// <param name="ref">Рефлексы</param>
		/// <param name="dex">Ловкость</param>
		/// <param name="body">Телосложение</param>
		/// <param name="emp">Эмпатия</param>
		/// <param name="cra">Крафт</param>
		/// <param name="will">Воля</param>
		/// <param name="speed">Скорость</param>
		/// <param name="luck">Удача</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns></returns>
		[Obsolete("Только для тестов")]
		public static CreatureTemplate CreateForTest(
			Guid? id = default,
			Game game = default,
			BodyTemplate bodyTemplate = default,
			CreatureType creatureType = default,
			ImgFile imgFile = default,
			string name = default,
			string description = default,
			int hp = default,
			int sta = default,
			int @int = default,
			int @ref = default,
			int dex = default,
			int body = default,
			int emp = default,
			int cra = default,
			int will = default,
			int speed = default,
			int luck = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
			=> new()
			{
				Id = id ?? Guid.NewGuid(),
				Game = game,
				ImgFile = imgFile,
				BodyTemplate = bodyTemplate ?? BodyTemplate.CreateForTest(game: game),
				Name = name ?? Enum.GetName(creatureType),
				Description = description,
				CreatureType = creatureType,
				HP = hp == default ? 10 : hp,
				Sta = sta == default ? 10 : sta,
				Int = @int == default ? 1 : @int,
				Ref = @ref == default ? 1 : @ref,
				Dex = dex == default ? 1 : dex,
				Body = body == default ? 1 : body,
				Emp = emp == default ? 1 : emp,
				Cra = cra == default ? 1 : cra,
				Will = will == default ? 1 : will,
				Speed = speed == default ? 1 : speed,
				Luck = luck == default ? 1 : luck,
				CreatedOn = createdOn,
				ModifiedOn = modifiedOn,
				CreatedByUserId = createdByUserId,
				Creatures = new List<Creature>(),
				CreatureTemplateSkills = new List<CreatureTemplateSkill>(),
				Abilities = new List<Ability>(),
				CreatureTemplateParts = new List<CreatureTemplatePart>(),
				DamageTypeModifiers = new List<EntityDamageTypeModifier>()
			};
	}
}
