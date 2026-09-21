using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	public class Character : Entity
	{
		public const int MinStat = 1;
		public const int MaxStat = 15;
		public const int MinSkillValue = 1;
		public const int MaxSkillValue = 10;

		public long UserId { get; private set; }

		/// <summary>
		/// Игра, в которой создан персонаж. Может стать null — если игру снесли, персонаж не удаляется
		/// вместе с ней (в отличие от остального содержимого игры), а остаётся у игрока в архивном виде.
		/// </summary>
		public long? GameId { get; private set; }

		public string Name { get; private set; }

		/// <summary>Ключ объекта в MinIO с изображением персонажа — показывается в листе персонажа/существа во время боя.</summary>
		public string? ImageKey { get; private set; }

		public int HP { get; private set; }

		public int CurrentHP { get; private set; }

		public int Sta { get; private set; }

		public int Int {  get; private set; }

		public int Str { get; private set; }

		public int Rea { get; private set; }

		public int Dex { get; private set; }

		public int Cra { get; private set; }

		public int Emp { get; private set; }

		public int Wil { get; private set; }

		/// <summary>
		/// (Str+Wil)/2 с округлением вниз — не вводится с фронтенда, а пересчитывается при каждом
		/// создании/изменении персонажа. Отдельное поле от Stun, хотя формула сейчас совпадает — они
		/// могут разойтись в будущем.
		/// </summary>
		public int Recovery { get; private set; }

		/// <summary>(Str+Wil)/2 с округлением вниз — см. Recovery.</summary>
		public int Stun { get; private set; }

		public Dictionary<Skill, int> Skills { get; private set; } = [];

		public List<Ability> Abilities { get; private set; } = [];

		public List<Item> Items { get; private set; } = [];

		private Character()
		{
		}

		public Character(long userId, long gameId, string name, int hp, int sta, int @int, int str, int rea, int dex, int cra, int emp, int wil)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(userId, nameof(userId));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(gameId, nameof(gameId));
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(hp, nameof(hp));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(sta, nameof(sta));
			InvalidArgumentException.ThrowIfNotInRange(@int, MinStat, MaxStat, nameof(@int));
			InvalidArgumentException.ThrowIfNotInRange(str, MinStat, MaxStat, nameof(str));
			InvalidArgumentException.ThrowIfNotInRange(rea, MinStat, MaxStat, nameof(rea));
			InvalidArgumentException.ThrowIfNotInRange(dex, MinStat, MaxStat, nameof(dex));
			InvalidArgumentException.ThrowIfNotInRange(cra, MinStat, MaxStat, nameof(cra));
			InvalidArgumentException.ThrowIfNotInRange(emp, MinStat, MaxStat, nameof(emp));
			InvalidArgumentException.ThrowIfNotInRange(wil, MinStat, MaxStat, nameof(wil));

			UserId = userId;
			GameId = gameId;
			Name = name;
			HP = hp;
			CurrentHP = hp;
			Sta = sta;
			Int = @int;
			Str = str;
			Rea = rea;
			Dex = dex;
			Cra = cra;
			Emp = emp;
			Wil = wil;
			Recovery = (str + wil) / 2;
			Stun = (str + wil) / 2;
		}

		public void UpdateCharacter(string name, int hp, int sta, int @int, int str, int rea, int dex, int cra, int emp, int wil)
		{
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(hp, nameof(hp));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(sta, nameof(sta));
			InvalidArgumentException.ThrowIfNotInRange(@int, MinStat, MaxStat, nameof(@int));
			InvalidArgumentException.ThrowIfNotInRange(str, MinStat, MaxStat, nameof(str));
			InvalidArgumentException.ThrowIfNotInRange(rea, MinStat, MaxStat, nameof(rea));
			InvalidArgumentException.ThrowIfNotInRange(dex, MinStat, MaxStat, nameof(dex));
			InvalidArgumentException.ThrowIfNotInRange(cra, MinStat, MaxStat, nameof(cra));
			InvalidArgumentException.ThrowIfNotInRange(emp, MinStat, MaxStat, nameof(emp));
			InvalidArgumentException.ThrowIfNotInRange(wil, MinStat, MaxStat, nameof(wil));

			Name = name;
			HP = hp;
			CurrentHP = Math.Min(CurrentHP, hp);
			Sta = sta;
			Int = @int;
			Str = str;
			Rea = rea;
			Dex = dex;
			Cra = cra;
			Emp = emp;
			Wil = wil;
			Recovery = (str + wil) / 2;
			Stun = (str + wil) / 2;
		}

		public void SetImage(string? imageKey)
		{
			ImageKey = imageKey;
		}

		/// <summary>Отдых вне боя — восстанавливает HP на величину Recovery, не выше максимума.</summary>
		public void Rest()
		{
			CurrentHP = Math.Min(HP, CurrentHP + Recovery);
		}

		/// <summary>
		/// Синхронизация текущего HP из боя обратно на персонажа — вызывается при удалении боя, в
		/// котором он участвовал (см. BattleService.DeleteBattleAsync), т.к. это единственный момент,
		/// когда участие персонажа в начавшемся бою завершается.
		/// </summary>
		public void SyncCurrentHpFromBattle(int currentHp)
		{
			CurrentHP = Math.Clamp(currentHp, 0, HP);
		}
	}
}
