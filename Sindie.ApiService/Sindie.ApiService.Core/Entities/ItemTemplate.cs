using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System;
using System.Collections.Generic;

namespace Sindie.ApiService.Core.Entities
{
	/// <summary>
	/// Шаблон предмета
	/// </summary>
	public class ItemTemplate : EntityBase
	{
		/// <summary>
		/// Поле для <see cref="_game"/>
		/// </summary>
		public const string GameField = nameof(_game);

		/// <summary>
		/// Поле для <see cref="_imgFile"/>
		/// </summary>
		public const string ImgFileField = nameof(_imgFile);

		private Game _game;
		private ImgFile _imgFile;
		private double _weight;
		private int _maxQuantity;

		/// <summary>
		/// Конструктор для EF
		/// </summary>
		protected ItemTemplate()
		{
		}

		/// <summary>
		/// Конструктор класса Шаблон предмета
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="name">Название шаблона предмета</param>
		/// <param name="description">Описание шаблона предмета</param>
		/// <param name="weight">Вес</param>
		/// <param name="maxQuntity">Максимальное количество в стаке</param>
		public ItemTemplate(
			Game game,
			ImgFile imgFile,
			string name,
			string description,
			double weight,
			int maxQuntity)
		{
			Game = game;
			ImgFile = imgFile;
			Name = name;
			Description = description;
			Weight = weight;
			MaxQuantity = maxQuntity;
			Items = new List<Item>();
			ItemTemplateModifiers = new List<ItemTemplateModifier>();
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
		/// Название шаблона предмета
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание шаблона предмета
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Вес
		/// </summary>
		public double Weight
        {
			get => _weight;
			set
            {
				if (value < 0)
					throw new ExceptionFieldOutOfRange<ItemTemplate>(nameof(Weight));
				_weight = value;
            }
        }

		/// <summary>
		/// Максимальное количество в стаке
		/// </summary>
		public int MaxQuantity
		{
			get => _maxQuantity;
			set
			{
				if (value < 1)
					throw new ExceptionFieldOutOfRange<ItemTemplate>(nameof(MaxQuantity));
				_maxQuantity = value;
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
		/// Предметы
		/// </summary>
		public List<Item> Items { get; set; }

		/// <summary>
		/// Модфикаторы шаблонов предметов
		/// </summary>
		public List<ItemTemplateModifier> ItemTemplateModifiers { get; set; }

		#endregion navigation properties

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="name">Название</param>
		/// <param name="description">Описание</param>
		/// <param name="imgFile">Графический файл</param>
		/// <param name="game">Игра</param>
		/// <returns>Шаблон предмета</returns>
		[Obsolete("Только для тестов")]
		public static ItemTemplate CreateForTest(
			Guid? id = default,
			string name = default,
			string description = default,
			ImgFile imgFile = default,
			Game game = default)
		=> new ItemTemplate()
		{
			Id = id ?? Guid.NewGuid(),
			Name = name ?? "ItemTemplate",
			Description = description,
			ImgFile = imgFile,
			Game = game,
			Items = new List<Item>(),
			ItemTemplateModifiers = new List<ItemTemplateModifier>()
		};
	}
}
