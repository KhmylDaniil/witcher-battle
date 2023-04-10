using Witcher.Core.Exceptions;
using Witcher.Core.Exceptions.EntityExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using static Witcher.Core.BaseData.Enums;
using Witcher.Core.Abstractions;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Существо
	/// </summary>
	public class Creature: EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_battle"/>
		/// </summary>
		public const string BattleField = nameof(_battle);

		/// <summary>
		/// Поле для <see cref="_imgFile"/>
		/// </summary>
		public const string ImgFileField = nameof(_imgFile);

		/// <summary>
		/// Поле для <see cref="_creatureTemplate"/>
		/// </summary>
		public const string CreatureTemplateField = nameof(_creatureTemplate);

		private Battle _battle;
		private ImgFile _imgFile;
		private CreatureTemplate _creatureTemplate;
		private Guid? _leadingArmId;

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
		/// Конструктор существа
		/// </summary>
		/// <param name="creatureTemplate">Шаблон существа</param>
		/// <param name="battle">Бой</param>
		/// <param name="name">Название существа</param>
		/// <param name="description">Описание существа</param>
		public Creature(
			CreatureTemplate creatureTemplate,
			Battle battle,
			string name,
			string description)
		{
			Battle = battle;
			ImgFile = creatureTemplate.ImgFile;
			CreatureTemplate = creatureTemplate;
			CreatureType = creatureTemplate.CreatureType;

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

			MaxHP = creatureTemplate.HP;
			MaxSta = creatureTemplate.Sta;
			MaxInt = creatureTemplate.Int;
			MaxRef = creatureTemplate.Ref;
			MaxDex = creatureTemplate.Dex;
			MaxBody = creatureTemplate.Body;
			MaxEmp = creatureTemplate.Emp;
			MaxCra = creatureTemplate.Cra;
			MaxWill = creatureTemplate.Will;
			MaxSpeed = creatureTemplate.Speed;
			MaxLuck = creatureTemplate.Luck;
			Stun = CalculateStun(creatureTemplate);

			Name = name;
			Description = description;
			CreatureParts = CreateParts(creatureTemplate.CreatureTemplateParts);
			Abilities = creatureTemplate.Abilities;
			Effects = new List<Effect>();
			CreatureSkills = CreateSkills(creatureTemplate.CreatureTemplateSkills);
		}

		/// <summary>
		/// Айди боя
		/// </summary>
		public Guid? BattleId { get; protected set; }

		/// <summary>
		/// Айди графического файла
		/// </summary>
		public Guid? ImgFileId { get; protected set; }

		/// <summary>
		/// Айди шаблона существа
		/// </summary>
		public Guid CreatureTemplateId { get; protected set; }

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
		public CreatureType CreatureType { get; set; }

		#region starting stats

		/// <summary>
		/// Максимальные хиты
		/// </summary>
		public int MaxHP { get; protected set; }

		/// <summary>
		/// Максимальная выносливость
		/// </summary>
		public int MaxSta { get; set; }

		/// <summary>
		/// Максимальный интеллект
		/// </summary>
		public int MaxInt { get; protected set; }

		/// <summary>
		/// Максимальные рефлексы
		/// </summary>
		public int MaxRef { get; private set; }

		/// <summary>
		/// Максимальная ловкость
		/// </summary>
		public int MaxDex { get; protected set; }

		/// <summary>
		/// Максимальное телосложение
		/// </summary>
		public int MaxBody { get; protected set; }

		/// <summary>
		/// Максимальная эмпатия
		/// </summary>
		public int MaxEmp { get; protected set; }

		/// <summary>
		/// Максимальное ремесло
		/// </summary>
		public int MaxCra { get; protected set; }

		/// <summary>
		/// Максимальная воля
		/// </summary>
		public int MaxWill { get; protected set; }

		/// <summary>
		/// Максимальная скорость
		/// </summary>
		public int MaxSpeed { get; protected set; }

		/// <summary>
		/// Максимальная удача
		/// </summary>
		public int MaxLuck { get; protected set; }

		/// <summary>
		/// Устойчивость
		/// </summary>
		public int Stun { get; set; }

		#endregion starting stats

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
					throw new FieldOutOfRangeException<Creature>(nameof(Sta));
				_sta = value;
			}
		}

		/// <summary>
		/// Интеллект
		/// </summary>
		public int Int
		{
			get => CheckWoundTreshold(_int);
			set => _int = value;
		}

		/// <summary>
		/// Рефлексы
		/// </summary>
		public int Ref
		{
			get => CheckWoundTreshold(_ref);
			set => _ref = value;
		}

		/// <summary>
		/// Ловкость
		/// </summary>
		public int Dex
		{
			get => CheckWoundTreshold(_dex);
			set => _dex = value;
		}

		/// <summary>
		/// Телосложение
		/// </summary>
		public int Body
		{
			get => _body < 1 ? 1 : _body;
			set => _body = value;
		}

		/// <summary>
		/// Эмпатия
		/// </summary>
		public int Emp
		{
			get => _emp < 1 ? 1 : _emp;
			set => _emp = value;
		}

		/// <summary>
		/// Крафт
		/// </summary>
		public int Cra
		{
			get => _cra < 1 ? 1 : _cra;
			set => _cra = value;
		}

		/// <summary>
		/// Воля
		/// </summary>
		public int Will
		{
			get => CheckWoundTreshold(_will);
			set => _will = value;
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
			get => _speed < 0 ? 0 : _speed;
			set => _speed = value;
		}

		/// <summary>
		/// Ведущая рука
		/// </summary>
		public Guid? LeadingArmId
		{
			get
			{
				if (_leadingArmId == default)
					_leadingArmId = DefineLeadingArm();
				return _leadingArmId;
			}
		}

		/// <summary>
		/// Порядок хода в битве
		/// </summary>
		public int InitiativeInBattle { get; set; }

		#region navigation properties

		/// <summary>
		/// Бой
		/// </summary>
		public Battle Battle
		{
			get => _battle;
			set
			{
				_battle = value;
				BattleId = value?.Id;
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
				_creatureTemplate = value ?? throw new EntityNotIncludedException<Creature>(nameof(CreatureTemplate));
				CreatureTemplateId = value.Id;
			}
		}

		/// <summary>
		/// Способности
		/// </summary>
		public List<Ability> Abilities { get; protected set; }

		/// <summary>
		/// Навыки существа
		/// </summary>
		public List<CreatureSkill> CreatureSkills { get; protected set; }

		/// <summary>
		/// Эффекты
		/// </summary>
		public List<Effect> Effects { get; set; }

		/// <summary>
		/// Части тела
		/// </summary>
		public List<CreaturePart> CreatureParts { get; protected set; }

		/// <summary>
		/// Модификаторы типа урона
		/// </summary>
		public List<CreatureDamageTypeModifier> DamageTypeModifiers { get; protected set; }

		/// <summary>
		/// Ход существа в битве
		/// </summary>
		public Turn Turn { get; set; } = new();

		#endregion navigation properties

		/// <summary>
		/// Изменение существа
		/// </summary>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		internal void ChangeCreature(string name, string description)
		{
			Name = name;
			Description = description;
		}

		/// <summary>
		/// Создание списка навыков существа
		/// </summary>
		/// <param name="skills">Навыки шаблона существа</param>
		/// <returns>Список навыков существа</returns>
		private List<CreatureSkill> CreateSkills(List<CreatureTemplateSkill> skills)
		{
			var result = new List<CreatureSkill>();

			foreach (var skill in skills)
				result.Add(new CreatureSkill(
					creature: this,
					skill: skill.Skill,
					skillValue: skill.SkillValue));
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
				throw new LogicBaseException($"У существа с айди {Id} отсутствуют способности.");

			var sortedAbilities = from a in Abilities
								orderby a.Accuracy, a.AttackSpeed, a.AttackDiceQuantity, a.DamageModifier
								select a;
			//TODO проверить
			return sortedAbilities.Last();
		}

		/// <summary>
		/// Защитный навык по умолчанию
		/// </summary>
		/// <param name="ability">Способность</param>
		/// <returns>Защитный навык существа</returns>
		internal int DefaultDefenseBase(IAttackFormula attackFormula)
		{
			if (!attackFormula.DefensiveSkills.Any())
				throw new LogicBaseException($"От способности {attackFormula.Name} c айди {attackFormula.Id} нет защиты.");

			var defenseBase = attackFormula.DefensiveSkills.Select(ds => SkillBase(ds.Skill)).Max();

			return defenseBase;
		}

		/// <summary>
		/// Расчет базы навыка
		/// </summary>
		/// <param name="skillId">Айди навыка</param>
		/// <returns></returns>
		public int SkillBase(Skill skill)
		{
			var statName = CorrespondingStat(skill);

			var value = typeof(Creature).GetProperty(Enum.GetName(statName)).GetValue(this);

			var statBase = Enum.IsDefined(statName)
				? (int)value
				: 0;

			var creatureSkill = CreatureSkills.FirstOrDefault(x => x.Skill == skill);

			if (creatureSkill is not null)
				statBase += creatureSkill.SkillValue;

			return statBase;
		}

		/// <summary>
		/// Возврат максимума скилла
		/// </summary>
		/// <param name="skillId">Айди навыка</param>
		/// <returns></returns>
		public int GetSkillMax(Skill skill)
		{
			var creatureSkill = CreatureSkills.FirstOrDefault(x => x.Skill == skill);

			return creatureSkill is null ? 0 : creatureSkill.MaxValue;
		}

		private Guid DefineLeadingArm()
		{
			var arms = CreatureParts.Where(x => x.BodyPartType == BodyPartType.Arm).ToList();

			if (!arms.Any()) return Guid.Empty;

			Random random = new();

			int i = random.Next(0, arms.Count - 1);

			return arms[i].Id;
		}

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="battle">Бой</param>
		/// <param name="creatureTemlpate">Шаблон существа</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="creatureType">Тип существа</param>
		/// <param name="hp">Хиты</param>
		/// <param name="maxHP">Максимальные хиты</param>
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
		/// <param name="stun">Устойчивость</param>
		/// <param name="createdOn">Дата создания</param>
		/// <param name="modifiedOn">Дата изменения</param>
		/// <param name="createdByUserId">Создавший пользователь</param>
		/// <returns></returns>
		[Obsolete("Только для тестов")]
		public static Creature CreateForTest(
			Guid? id = default,
			Battle battle = default,
			CreatureTemplate creatureTemlpate = default,
			ImgFile imgFile = default,
			CreatureType creatureType = default,
			string name = default,
			string description = default,
			int hp = default,
			int maxHP = default,
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
			int maxSpeed = default, 
			int stun = default,
			DateTime createdOn = default,
			DateTime modifiedOn = default,
			Guid createdByUserId = default)
			=> new() 
			{
				Id = id ?? Guid.NewGuid(),
				Battle = battle,
				CreatureTemplate = creatureTemlpate,
				ImgFile = imgFile,
				CreatureType = creatureType,
				Name = name ?? Enum.GetName(creatureType),
				Description = description,
				HP = hp == default ? 50 : hp,
				MaxHP = maxHP == default ? 50 : maxHP,
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
				MaxSpeed = maxSpeed == default ? 1 : maxSpeed,
				Stun = stun == default ? 1 : stun,
				CreatedOn = createdOn,
				ModifiedOn = modifiedOn,
				CreatedByUserId = createdByUserId,
				Effects = new List<Effect>(),
				CreatureSkills = new List<CreatureSkill>(),
				Abilities = new List<Ability>(),
				CreatureParts = new List<CreaturePart>(),
				DamageTypeModifiers = new List<CreatureDamageTypeModifier>()
			};

		internal int GetInt() => _int;

		internal int GetRef() => _ref;

		internal int GetDex() => _dex;

		internal int GetBody() => _body;

		internal int GetEmp() => _emp;

		internal int GetCra() => _cra;

		internal int GetWill() => _will;

		internal int GetSpeed() => _speed;

		private int CheckWoundTreshold(int value)
		{
			if (HP <= MaxHP / 5)
				value /= 2;

			return value < 1 ? 1 : value;
		}

		private static int CalculateStun(CreatureTemplate creatureTemplate)
		{
			var result = (creatureTemplate.Will + creatureTemplate.Body) / 2;

			return result > 10 ? 10 : result;
		}
	}

	/// <summary>
	/// Ход существа
	/// </summary>
	public class Turn
	{
		/// <summary>
		/// Завершенность хода существа
		/// </summary>
		public TurnState TurnState { get; set; }

		/// <summary>
		/// В этом ходу выполнялась бесплатная защита
		/// </summary>
		public int IsDefenseInThisTurnPerformed { get; set; }

		/// <summary>
		/// Количество оставшихся в этом действии мультиатак
		/// </summary>
		public int MultiattackRemainsQuantity { get; set; }

		/// <summary>
		/// Айди формулы для мультиатаки
		/// </summary>
		public Guid? MuitiattackAttackFormulaId { get; set; }

		/// <summary>
		/// Количество потраченной за ход энергии
		/// </summary>
		public int EnergySpendInThisTurn { get; set; }
	}
}
