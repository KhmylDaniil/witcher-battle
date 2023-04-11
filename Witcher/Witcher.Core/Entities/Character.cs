using System;
using System.Collections.Generic;
using System.Linq;
using Witcher.Core.Exceptions;
using Witcher.Core.Exceptions.EntityExceptions;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Entities
{
	/// <summary>
	/// Персонаж
	/// </summary>
	public class Character : Creature
	{
		public const string UserGameActivatedField = nameof(_userGameActivated);
		public const string GameField = nameof(_game);
		public const string BagField = nameof(_bag);

		private Game _game;
		private UserGameCharacter _userGameActivated;
		private Bag _bag;

		protected Character() { }

		public Character(Game game, CreatureTemplate creatureTemplate, Battle battle, string name, string Description)
			: base(creatureTemplate, battle, name, Description)
		{
			Game = game;
			UserGameCharacters = new List<UserGameCharacter>();
			Bag = new Bag(this);
		}

		public Guid GameId { get; protected set; }

		public Guid BagId { get; protected set; }

		/// <summary>
		/// Айди активировавшего пользователя
		/// </summary>
		public Guid? UserGameActivatedId { get; protected set; }

		/// <summary>
		/// Время активации персонажа
		/// </summary>
		public DateTime? ActivationTime { get; set; }

		#region navigation properties

		/// <summary>
		/// Активировавший персонажа пользователь игры
		/// </summary>
		public UserGameCharacter UserGameActivated
		{
			get => _userGameActivated;
			set
			{
				_userGameActivated = value;
				UserGameActivatedId = value?.Id;
			}
		}

		/// <summary>
		/// Персонажи пользователя игры
		/// </summary>
		public List<UserGameCharacter> UserGameCharacters { get; set; }

		/// <summary>
		/// Игра
		/// </summary>
		public Game Game
		{
			get => _game;
			protected set
			{
				_game = value ?? throw new EntityNotIncludedException<Character>(nameof(Game));
				GameId = value.Id;
			}
		}

		/// <summary>
		/// Сумка
		/// </summary>
		public Bag Bag
		{
			get => _bag;
			protected set
			{
				_bag = value ?? throw new EntityNotIncludedException<Character>(nameof(Bag));
				BagId = value.Id;
			}
		}

		/// <summary>
		/// Список экипированного оружия
		/// </summary>
		public List<Weapon> EquippedWeapons { get; set; }

		#endregion navigation properties

		internal void AddUserGameCharacters(Game game, Guid currentUserId)
		{
			var mainMasterUserGame = game.UserGames.FirstOrDefault(x => x.GameRoleId == BaseData.GameRoles.MainMasterRoleId)
				?? throw new LogicBaseException("В игре нет главмастера.");

			var characterCreatorUserGame = game.UserGames.FirstOrDefault(x => x.UserId == currentUserId)
				?? throw new EntityNotFoundException<UserGame>(currentUserId);

			UserGameCharacters.Add(new UserGameCharacter(this, mainMasterUserGame));

			if (mainMasterUserGame.Id !=characterCreatorUserGame.Id)
				UserGameCharacters.Add(new UserGameCharacter(this, characterCreatorUserGame));
		}

		/// <summary>
		/// Создать тестовую сущность с заполненными полями
		/// </summary>
		/// <param name="id">Айди</param>
		/// <param name="game">Игра</param>
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
		public static Character CreateForTest(
			Guid? id = default,
			Game game = default,
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
		{
			Character character = new()
			{
				Id = id ?? Guid.NewGuid(),
				Game = game,
				Battle = battle,
				CreatureTemplate = creatureTemlpate ?? CreatureTemplate.CreateForTest(game: game),
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
				DamageTypeModifiers = new List<CreatureDamageTypeModifier>(),
				UserGameCharacters = new List<UserGameCharacter>(),
			};

			character.Bag = new(character);
			return character;
		}	
	}
}
