using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System;
using System.Collections.Generic;

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
