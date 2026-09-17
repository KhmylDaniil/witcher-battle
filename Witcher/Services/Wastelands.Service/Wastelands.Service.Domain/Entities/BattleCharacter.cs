using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	/// <summary>
	/// Персонаж игрока, вошедший в конкретный бой — HP/Sta скопированы из <see cref="Character"/>
	/// на момент входа в бой, дальнейшее изменение листа персонажа задним числом на бой не влияет.
	/// </summary>
	public class BattleCharacter : Entity
	{
		public long BattleId { get; private set; }

		public long CharacterId { get; private set; }

		public int MaxHP { get; private set; }

		public int CurrentHP { get; private set; }

		public int MaxSta { get; private set; }

		public int CurrentSta { get; private set; }

		public int? Initiative { get; private set; }

		public List<Condition> AppliedConditions { get; private set; } = [];

		/// <summary>EF-навигация — нужна для отображения имени персонажа в списке боя.</summary>
		public Character Character { get; set; }

		private BattleCharacter()
		{
		}

		public BattleCharacter(long battleId, Character character)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(battleId, nameof(battleId));
			InvalidArgumentException.ThrowIfNull(character, nameof(character));

			BattleId = battleId;
			CharacterId = character.Id;
			MaxHP = character.HP;
			CurrentHP = character.HP;
			MaxSta = character.Sta;
			CurrentSta = character.Sta;
		}

		public void SetInitiative(int value)
		{
			if (Initiative.HasValue)
			{
				throw new InvalidArgumentException(ErrorCode.InvalidArgument, "Инициатива уже выставлена и не может быть изменена.");
			}

			Initiative = value;
		}

		/// <summary>Применяет урон, полученный в результате атаки — CurrentHP не опускается ниже нуля.</summary>
		public void ApplyDamage(int damage)
		{
			InvalidArgumentException.ThrowIfLessThanZero(damage, nameof(damage));
			CurrentHP = Math.Max(0, CurrentHP - damage);
		}

		public void AddCondition(Condition condition)
		{
			if (!AppliedConditions.Contains(condition))
			{
				AppliedConditions.Add(condition);
			}
		}

		public void RemoveCondition(Condition condition)
		{
			AppliedConditions.Remove(condition);
		}
	}
}
