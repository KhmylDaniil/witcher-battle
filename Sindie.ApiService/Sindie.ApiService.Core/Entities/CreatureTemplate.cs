using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sindie.ApiService.Core.Entities
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
		/// <param name="type">Тип существа</param>
		/// <param name="armor">Броня существа</param>
		public CreatureTemplate(
			Game game,
			ImgFile imgFile,
			BodyTemplate bodyTemplate,
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
			string type,
			int armor)
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
			Type = type;
			Abilities = new List<Ability>();
			CreatureTemplateParameters = new List<CreatureTemplateParameter>();
			Creatures = new List<Creature>();
			BodyParts = CreateBody(bodyTemplate, armor);
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
		/// Название шаблона
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание шаблона
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Тип существа
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// Хиты
		/// </summary>
		public int HP
		{
			get => _hp;
			set
			{
				if (value < 1)
					throw new ExceptionFieldOutOfRange<Creature>(nameof(HP));
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
					throw new ExceptionFieldOutOfRange<Creature>(nameof(Sta));
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
					throw new ExceptionFieldOutOfRange<Creature>(nameof(Int));
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
					throw new ExceptionFieldOutOfRange<Creature>(nameof(Ref));
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
					throw new ExceptionFieldOutOfRange<Creature>(nameof(Dex));
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
					throw new ExceptionFieldOutOfRange<Creature>(nameof(Body));
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
					throw new ExceptionFieldOutOfRange<Creature>(nameof(Emp));
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
					throw new ExceptionFieldOutOfRange<Creature>(nameof(Cra));
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
					throw new ExceptionFieldOutOfRange<Creature>(nameof(Will));
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
					throw new ExceptionFieldOutOfRange<Creature>(nameof(Luck));
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
					throw new ExceptionFieldOutOfRange<Creature>(nameof(Speed));
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
				_bodyTemplate = value ?? throw new ApplicationException("Необходимо передать шаблон тела");
				BodyTemplateId = value.Id;
			}
		}

		/// <summary>
		/// Способности
		/// </summary>
		public List<Ability> Abilities { get; protected set; }

		/// <summary>
		/// Параметры шаблона существа
		/// </summary>
		public List<CreatureTemplateParameter> CreatureTemplateParameters { get; protected set; }

		/// <summary>
		/// Части тела
		/// </summary>
		public List<BodyPart> BodyParts { get; protected set; }

		/// <summary>
		/// Существа
		/// </summary>
		public List<Creature> Creatures { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Обновление списка способностей
		/// </summary>
		/// <param name="data">Данные</param>
		internal void UpdateAlibilities(List<AbilityData> data)
		{
			if (data == null)
				throw new ExceptionEntityNotFound<AbilityData>(nameof(data));

			if (Abilities == null)
				throw new ExceptionEntityNotFound<List<Ability>>(nameof(Abilities));

			var entitiesToDelete = Abilities.Where(x => !data
				.Any(y => y.Id == x.Id)).ToList();

			if (entitiesToDelete.Any())
				foreach (var entity in entitiesToDelete)
					Abilities.Remove(entity);

			if (!data.Any())
				return;

			foreach (var dataItem in data)
			{
				var ability = Abilities.FirstOrDefault(x => x.Id == dataItem.Id);
				if (ability == null)
					Abilities.Add(
						Ability.CreateAbility(
						creatureTemplate: this,
						name: dataItem.Name,
						description: dataItem.Description,
						attackParameter: dataItem.AttackParameter,
						attackDiceQuantity: dataItem.AttackDiceQuantity,
						damageModifier: dataItem.DamageModifier,
						attackSpeed: dataItem.AttackSpeed,
						accuracy: dataItem.Accuracy,
						appliedConditions: dataItem.AppliedConditions));
				else
					ability.ChangeAbility(
						name: dataItem.Name,
						description: dataItem.Description,
						attackParameter: dataItem.AttackParameter,
						attackDiceQuantity: dataItem.AttackDiceQuantity,
						damageModifier: dataItem.DamageModifier,
						attackSpeed: dataItem.AttackSpeed,
						accuracy: dataItem.Accuracy,
						appliedConditions: dataItem.AppliedConditions);
			}
		}

		/// <summary>
		/// Метод изменения списка параметров шаблона существа
		/// </summary>
		/// <param name="request">Запрос</param>
		internal void UpdateCreatureTemplateParameters(List<(Parameter Parameter, int Value)> request)
		{
			if (CreatureTemplateParameters == null)
				throw new ExceptionEntityNotIncluded<CreatureTemplateParameter>(nameof(CreatureTemplateParameters));

			if (request != null || CreatureTemplateParameters.Any())
			{
				var entitiesToDelete = CreatureTemplateParameters
					.Where(x => !request.Any(y => y.Parameter.Id == x.ParameterId)).ToList();

				var entitiesToAdd = request.Where(x => !CreatureTemplateParameters
					.Any(y => y.ParameterId == x.Parameter.Id)).ToList();

				if (entitiesToAdd.Any())
					foreach (var entity in entitiesToAdd)
						CreatureTemplateParameters.Add(
							new CreatureTemplateParameter(
								parameter: entity.Parameter,
								parameterValue: entity.Value,
								creatureTemplate: this));

				if (entitiesToDelete.Any())
					foreach (var entity in entitiesToDelete)
						CreatureTemplateParameters.Remove(entity);
			}
		}

		/// <summary>
		/// Создание тела шаблона существа
		/// </summary>
		/// <param name="bodyTemplate">Шаблон тела</param>
		/// <param name="armor">Броня</param>
		/// <returns></returns>
		private static List<BodyPart> CreateBody(BodyTemplate bodyTemplate, int armor)
		{
			var bodyParts = new List<BodyPart>();

			foreach (var part in bodyTemplate.BodyTemplateParts)
				bodyParts.Add(new BodyPart()
				{
					Name = part.Name,
					HitPenalty = part.HitPenalty,
					DamageModifier = part.DamageModifier,
					MinToHit = part.MinToHit,
					MaxToHit = part.MaxToHit,
					StartingArmor = armor,
					CurrentArmor = armor
				});
			return bodyParts;
		}

		/// <summary>
		/// изменение тела шаблона существа
		/// </summary>
		/// <param name="bodyTemplate">Шаблон тела</param>
		/// <param name="armorList">Броня</param>
		/// <returns>Список частей шаблона тела</returns>
		private static List<BodyPart> UpdateBody(BodyTemplate bodyTemplate, List<(string BodyPartName, int Armor)> armorList)
		{
			var bodyParts = new List<BodyPart>();

			foreach (var part in bodyTemplate.BodyTemplateParts)
				bodyParts.Add(new BodyPart()
				{
					Name = part.Name,
					HitPenalty = part.HitPenalty,
					DamageModifier = part.DamageModifier,
					MinToHit = part.MinToHit,
					MaxToHit = part.MaxToHit,
					StartingArmor = armorList.FirstOrDefault(x => x.BodyPartName == part.Name).Armor,
					CurrentArmor = armorList.FirstOrDefault(x => x.BodyPartName == part.Name).Armor
				});
			return bodyParts;
		}

		/// <summary>
		/// Изменение шаблона существа
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="bodyTemplate">Шаблон тела</param>
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
		/// <param name="type">Тип существа</param>
		/// <param name="armorList">Список брони</param>
		public void ChangeCreatureTemplate(
			Game game,
			ImgFile imgFile,
			BodyTemplate bodyTemplate,
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
			string type,
			List<(string BodyPartName, int Armor)> armorList)
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
			Type = type;
			BodyParts = UpdateBody(bodyTemplate, armorList);
		}

		//private int Roll(int parameter)
		//{
		//	Random random = new Random();
		//	var roll = random.Next(1, 10);
		//	if (roll == 10)
		//		parameter += roll + AddRoll();
		//	else if (roll == 1)
		//		parameter -= AddRoll();
		//	else
		//		parameter += roll;
		//	return parameter < 1 ? 1 : parameter;
		//}

		//private int AddRoll()
		//{
		//	var result = 0;
		//	Random rand = new Random();
		//	var roll = 0;
		//	do
		//	{
		//		roll = rand.Next(1, 10);
		//		result += roll;
		//	}
		//	while (roll == 10);
		//	return result;
		//}
	}
}
