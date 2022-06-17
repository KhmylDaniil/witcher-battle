using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
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

		private Instance _instance;
		private ImgFile _imgFile;
		private CreatureTemplate _creatureTemplate;
		private BodyTemplate _bodyTemplate;

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
			BodyParts = creatureTemplate.BodyParts;
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
		/// Тип существа
		/// </summary>
		public string Type { get; set; }

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
		public List<BodyPart> BodyParts { get; protected set; }

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
		/// Выбор способности по умолчанию
		/// </summary>
		/// <returns>Способность по умолчанию</returns>
		internal Ability DefaultAbility()
		{
			if (!Abilities.Any())
				throw new ApplicationException($"У существа с айди {Id} отсутствуют способности.");

			if (Abilities.Count == 1)
				return Abilities[0];

			var sortedAbilities = from a in Abilities
								orderby a.Accuracy, a.AttackSpeed, a.AttackDiceQuantity, a.DamageModifier
								select a;
			return sortedAbilities.FirstOrDefault();
		}

		/// <summary>
		/// Расчет атаки
		/// </summary>
		/// <param name="ability">Способность</param>
		/// <param name="bodyTemplatePart">Шабон тела цели</param>
		/// <param name="successValue">Значение успеха</param>
		/// <returns>Результат атаки</returns>
		internal string Attack(Ability ability, BodyTemplatePart bodyTemplatePart, int successValue)
		{
			var message = new StringBuilder($"{Name} атакует способностью {ability.Name} в {bodyTemplatePart.Name}.");
			if (successValue > 0)
			{
				message.AppendLine($"Попадание с превышением на {successValue}");
				message.AppendLine($"Нанеcено {ability.RollDamage()}. Модификатор урона после поглощения броней составляет {bodyTemplatePart.DamageModifier}"
				foreach (var condition in ability.RollConditions())
					message.AppendLine($"Наложено состояние {condition.Name}");
			}
			else if (successValue < -5)
				message.AppendLine($"Критический промах {successValue}.");
			else
				message.AppendLine("Промах.");

			return message.ToString();
		}
	}
}
