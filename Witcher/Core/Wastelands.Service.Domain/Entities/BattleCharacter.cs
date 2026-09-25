using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Drafts;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	/// <summary>
	/// Персонаж игрока, вошедший в конкретный бой — HP/Sta скопированы из <see cref="Character"/>
	/// на момент входа в бой (CurrentHP — из накопленного вне боя Character.CurrentHP, а не всегда с
	/// полного HP), дальнейшее изменение листа персонажа задним числом на бой не влияет. При удалении
	/// боя CurrentHP синхронизируется обратно на персонажа — см. BattleService.DeleteBattleAsync.
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

		/// <summary>Критические ранения этого персонажа — по слоту (часть тела + тип урона), см. Creature.CriticalWounds.</summary>
		public Dictionary<string, Condition> CriticalWounds { get; private set; } = [];

		/// <summary>
		/// true — этот персонаж уже потратил в текущий свой ход основное (бесплатное) действие и может
		/// либо взять дополнительное за BattleAttack.BonusActionStaminaCost выносливости, либо закончить
		/// ход (см. BattleCombatService.EndActivationAsync/StartAttackAsync). Сбрасывается на false в
		/// начале каждого следующего хода — см. Battle.AdvanceTurn.
		/// </summary>
		public bool HasActedThisTurn { get; private set; }

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
			CurrentHP = character.CurrentHP;
			MaxSta = character.Sta;
			CurrentSta = character.Sta;
		}

		/// <summary>Позиция на карте, подключённой к бою (см. Battle.PlaceParticipantOnMap). Null — участник на карту не выставлен.</summary>
		public int? MapColumn { get; private set; }

		public int? MapRow { get; private set; }

		internal void PlaceOnMap(int column, int row)
		{
			MapColumn = column;
			MapRow = row;
		}

		internal void RemoveFromMap()
		{
			MapColumn = null;
			MapRow = null;
		}

		public void SetInitiative(int value)
		{
			if (Initiative.HasValue)
			{
				throw new InvalidArgumentException(ErrorCode.InvalidArgument, "Инициатива уже выставлена и не может быть изменена.");
			}

			Initiative = value;
		}

		/// <summary>Переиндексация после Battle.RemoveCreature — в отличие от SetInitiative, перезаписывает уже выставленное значение.</summary>
		internal void ReassignInitiative(int value)
		{
			Initiative = value;
		}

		/// <summary>
		/// Применяет урон, полученный в результате атаки — в отличие от Creature (где 0 сразу означает
		/// смерть и удаление из боя), у персонажа HP может уходить в минус: это и есть счётчик
		/// "отрицательных хитов" для состояния Dying (см. BattleParticipants.CheckCharacterDying,
		/// BattleCombatService.StabilizeAsync — сложность стабилизации равна |CurrentHP|).
		/// </summary>
		public void ApplyDamage(int damage)
		{
			InvalidArgumentException.ThrowIfLessThanZero(damage, nameof(damage));
			CurrentHP -= damage;
		}

		/// <summary>Успешная стабилизация умирающего персонажа (см. BattleCombatService.StabilizeAsync) — HP становится 1, Dying снимается.</summary>
		public void Stabilize()
		{
			CurrentHP = 1;
			RemoveCondition(Condition.Dying);
		}

		/// <summary>Списывает выносливость на дополнительное действие — CurrentSta не опускается ниже нуля.</summary>
		public void SpendStamina(int amount)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(amount, nameof(amount));
			CurrentSta = Math.Max(0, CurrentSta - amount);
		}

		/// <summary>Основное действие хода потрачено — открывает окно дополнительного действия (см. HasActedThisTurn).</summary>
		public void MarkActedThisTurn()
		{
			HasActedThisTurn = true;
		}

		/// <summary>Новый ход — сбрасывает окно дополнительного действия. Вызывается из Battle.AdvanceTurn.</summary>
		internal void ResetTurnState()
		{
			HasActedThisTurn = false;
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

		/// <summary>См. Creature.ApplyCriticalWound — то же поведение, для персонажа.</summary>
		public void ApplyCriticalWound(string slotKey, Condition wound)
		{
			var hadPrevious = CriticalWounds.TryGetValue(slotKey, out var previous);
			if (hadPrevious && CriticalWoundCatalog.IsAtLeastAsSevere(previous, wound))
			{
				return;
			}

			CriticalWounds[slotKey] = wound;

			if (hadPrevious && previous != wound && !CriticalWounds.Values.Contains(previous))
			{
				AppliedConditions.Remove(previous);
			}

			AddCondition(wound);
		}
	}
}
