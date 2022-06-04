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
