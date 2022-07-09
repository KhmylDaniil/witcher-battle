using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Существо
	/// </summary>
	public class Creature: EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_instance"/>
		/// </summary>
		public const string InstanceField = nameof(_instance);

		/// <summary>
		/// Поле для <see cref="_imgFile"/>
		/// </summary>
		public const string ImgFileField = nameof(_imgFile);

		/// <summary>
		/// Поле для <see cref="_creatureTemplate"/>
		/// </summary>
		public const string CreatureTemplateField = nameof(_creatureTemplate);

		/// <summary>
		/// Поле для <see cref="_bodyTemplate"/>
		/// </summary>
		public const string BodyTemplateField = nameof(_bodyTemplate);

		/// <summary>
		/// Поле для <see cref="_creatureType"/>
		/// </summary>
		public const string CreatureTypeField = nameof(_creatureType);

		private Instance _instance;
		private ImgFile _imgFile;
		private CreatureTemplate _creatureTemplate;
		private BodyTemplate _bodyTemplate;
		private CreatureType _creatureType;

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
		protected Creature()
		{
		}

		/// <summary>
		/// Конструктор инстанса
		/// </summary>
		/// <param name="creatureTemplate">Шаблон существа</param>
		/// <param name="instance">интанс</param>
		/// <param name="name">Название существа</param>
		/// <param name="description">Описание существа</param>
		public Creature(
			CreatureTemplate creatureTemplate,
			Instance instance,
			string name,
			string description)
		{
			Instance = instance;
			ImgFile = creatureTemplate.ImgFile;
			CreatureTemplate = creatureTemplate;
			BodyTemplate = creatureTemplate.BodyTemplate;
			CreatureTypeId = creatureTemplate.CreatureTypeId;
			HP = creatureTemplate.HP;
			Sta = creatureTemplate.Sta;
			Int = creatureTemplate.Int;
			Ref = creatureTemplate.Ref;
			Dex = creatureTemplate.Dex;
			Body = creatureTemplate.Body;
			Emp = creatureTemplate.Emp;
			Cra	= creatureTemplate.Cra;
			Will = creatureTemplate.Will;
			Speed = creatureTemplate.Speed;
			Luck = creatureTemplate.Luck;
			Name = name;
			Description = description;
			CreatureParts = CreateParts(creatureTemplate.CreatureTemplateParts);
			Abilities = creatureTemplate.Abilities;
			Conditions = new List<Condition>();
			CreatureParameters = CreateParameters(creatureTemplate.CreatureTemplateParameters);
		}

		/// <summary>
		/// Айди экземпляра
		/// </summary>
		public Guid InstanceId { get; protected set; }

		/// <summary>
		/// Айди графического файла
		/// </summary>
		public Guid? ImgFileId { get; protected set; }

		/// <summary>
		/// Айди шаблона существа
		/// </summary>
		public Guid CreatureTemplateId { get; protected set; }

		/// <summary>
		/// Айди шаблона тела
		/// </summary>
		public Guid BodyTemplateId { get; protected set; }

		/// <summary>
		/// Название существа
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание существа
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Айди типа существа
		/// </summary>
		public Guid CreatureTypeId { get; protected set; }

		/// <summary>
		/// Хиты
		/// </summary>
		public int HP { get; set; }

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
		/// Экземпляр
		/// </summary>
		public Instance Instance
		{
			get => _instance;
			protected set
			{
				_instance = value ?? throw new ApplicationException("Необходимо передать экземпляр");
				InstanceId = value.Id;
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
		/// Тело существа
		/// </summary>
		public CreatureTemplate CreatureTemplate
		{
			get => _creatureTemplate;
			set
			{
				_creatureTemplate = value ?? throw new ApplicationException("Необходимо передать шаблон существа");
				CreatureTemplateId = value.Id;
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
		/// Тип существа
		/// </summary>
		public CreatureType CreatureType
		{
			get => _creatureType;
			set
			{
				_creatureType = value ?? throw new ApplicationException($"Необходимо передать {nameof(CreatureType)}");
				CreatureTypeId = value.Id;
			}
		}

		/// <summary>
		/// Способности
		/// </summary>
		public List<Ability> Abilities { get; protected set; }

		/// <summary>
		/// Параметры шаблона существа
		/// </summary>
		public List<CreatureParameter> CreatureParameters { get; protected set; }

		/// <summary>
		/// Наложенные состояния
		/// </summary>
		public List<Condition> Conditions { get; set; }

		/// <summary>
		/// Части тела
		/// </summary>
		public List<CreaturePart> CreatureParts { get; protected set; }

		/// <summary>
		/// Сопротивления
		/// </summary>
		public List<DamageType> Resistances { get; protected set; }

		/// <summary>
		/// Уязвимости
		/// </summary>
		public List<DamageType> Vulnerables { get; protected set; }

		/// <summary>
		/// Иммунитеты
		/// </summary>
		public List<DamageType> Immunities { get; protected set; }

		#endregion navigation properties

		/// <summary>
		/// Создание списка параметров существа
		/// </summary>
		/// <param name="parameters">Параметры шаблона существа</param>
		/// <returns>Список параметров существа</returns>
		private List<CreatureParameter> CreateParameters(List<CreatureTemplateParameter> parameters)
		{
			var result = new List<CreatureParameter>();

			foreach (var parameter in parameters)
				result.Add(new CreatureParameter(
					creature: this,
					parameter: parameter.Parameter,
					parameterValue: parameter.ParameterValue));
			return result;
		}

		/// <summary>
		/// Создать список частей тела
		/// </summary>
		/// <param name="parts">части шаблона тела</param>
		/// <returns></returns>
		private List<CreaturePart> CreateParts(List<CreatureTemplatePart> parts)
		{
			var result = new List<CreaturePart>();

			foreach (var part in parts)
				result.Add(new CreaturePart(
					creature: this,
					bodyPartType: part.BodyPartType,
					name: part.Name,
					damageModifier: part.DamageModifier,
					hitPenalty: part.HitPenalty,
					minToHit: part.MinToHit,
					maxToHit: part.MaxToHit,
					armor: part.Armor));
			return result;
		}

		/// <summary>
		/// Выбор способности по умолчанию
		/// </summary>
		/// <returns>Способность по умолчанию</returns>
		internal Ability DefaultAbility()
		{
			if (!Abilities.Any())
				throw new ApplicationException($"У существа с айди {Id} отсутствуют способности.");

			var sortedAbilities = from a in Abilities
								orderby a.Accuracy, a.AttackSpeed, a.AttackDiceQuantity, a.DamageModifier
								select a;
			return sortedAbilities.First();
		}

		/// <summary>
		/// Защитный параметр по умолчанию
		/// </summary>
		/// <param name="ability">Способность</param>
		/// <returns>Защитный параметр существа</returns>
		internal CreatureParameter DefaultDefensiveParameter(Ability ability)
		{
			if (!ability.DefensiveParameters.Any())
				throw new ApplicationException($"От способности {ability.Name} c айди {ability.Id} нет защиты.");

			var creatureDefensiveParameters = CreatureParameters.Where(x => ability.DefensiveParameters.Any(a => a.Id == x.ParameterId));

			var sortedList = from x in creatureDefensiveParameters
							 orderby ParameterBase(x.ParameterId)
							 select x;

			return sortedList.First();
		}

		/// <summary>
		/// Расчет базы параметра
		/// </summary>
		/// <param name="parameterId">Айди параметра</param>
		/// <returns></returns>
		public int ParameterBase(Guid parameterId)
		{
			var parameter = CreatureParameters.FirstOrDefault(x => x.ParameterId == parameterId);

			var value = typeof(Creature).GetProperty(parameter.StatName).GetValue(this);

			var statBase = String.IsNullOrEmpty(parameter.StatName)
				? 0
				: (int)value;
			return statBase + parameter.ParameterValue;
		}

		/// <summary>
		/// Выбор места попадания
		/// </summary>
		/// <param name="id">Айди части тела существа</param>
		/// <returns>Часть шаблона тела</returns>
		internal CreaturePart DefaultCreaturePart()
		{
			Random random = new Random();
			var roll = random.Next(1, 10);
			return CreatureParts.First(x => x.MinToHit <= roll && x.MaxToHit >= roll);
		}

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="instance">Инстанс</param>
		/// <param name="creatureTemlpate">Шаблон существа</param>
		/// <param name="bodyTemplate">Шаблон тела</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
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
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns></returns>
		[Obsolete("Только для тестов")]
		public static Creature CreateForTest(
			Guid? id = default,
			Instance instance = default,
			CreatureTemplate creatureTemlpate = default,
			BodyTemplate bodyTemplate = default,
			ImgFile imgFile = default,
			CreatureType creatureType = default, 
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
			=> new Creature
			{
				Id = id ?? Guid.NewGuid(),
				Instance = instance,
				CreatureTemplate = creatureTemlpate,
				ImgFile = imgFile,
				BodyTemplate = bodyTemplate,
				CreatureType = creatureType,
				Name = name ?? creatureType.Name,
				Description = description,
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
				Conditions = new List<Condition>(),
				CreatureParameters = new List<CreatureParameter>(),
				Abilities = new List<Ability>(),
				CreatureParts = new List<CreaturePart>()
			};
	}
}
