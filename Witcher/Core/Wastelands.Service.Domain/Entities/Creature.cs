using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Drafts;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	/// <summary>
	/// Участник боя, скопированный из <see cref="CreatureTemplate"/> в момент добавления в конкретный
	/// бой — дальнейшее редактирование шаблона на уже созданных существ не влияет. Части тела/способности/
	/// модификаторы урона не копируются — они понадобятся только на шаге "расчёт действий".
	/// </summary>
	public class Creature : Entity
	{
		public long BattleId { get; private set; }

		public long CreatureTemplateId { get; private set; }

		public string Name { get; private set; }

		public CreatureType CreatureType { get; private set; }

		public int MaxHP { get; private set; }

		public int CurrentHP { get; private set; }

		public int MaxSta { get; private set; }

		public int CurrentSta { get; private set; }

		public int Ref { get; private set; }

		/// <summary>(Body+Will шаблона)/2 с округлением вниз — не вводится с фронтенда, см. Character.Recovery.</summary>
		public int Recovery { get; private set; }

		/// <summary>(Body+Will шаблона)/2 с округлением вниз — см. Recovery.</summary>
		public int Stun { get; private set; }

		public int? Initiative { get; private set; }

		public List<Condition> AppliedConditions { get; private set; } = [];

		/// <summary>
		/// Накопленный износ брони по частям тела (partId → сколько очков брони потеряно) — только
		/// для этого конкретного существа в этом бою, шаблон не меняется. Эффективная броня части
		/// считается как (шаблонная броня − износ), с полом в 0; см. BattleCombatCalculator.
		/// </summary>
		public Dictionary<long, int> ArmorReductionByPartId { get; private set; } = [];

		/// <summary>
		/// Критические ранения этого существа — по слоту (часть тела + тип урона, см.
		/// CriticalWoundCatalog.SlotKey), а не по типу части тела: у существа с несколькими частями
		/// одного BodyPartType (например, несколько Leg) каждая часть — свой независимый слот.
		/// </summary>
		public Dictionary<string, Condition> CriticalWounds { get; private set; } = [];

		private Creature()
		{
		}

		public Creature(long battleId, CreatureTemplate template, string? nameOverride)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(battleId, nameof(battleId));
			InvalidArgumentException.ThrowIfNull(template, nameof(template));

			BattleId = battleId;
			CreatureTemplateId = template.Id;
			Name = string.IsNullOrWhiteSpace(nameOverride) ? template.Name : nameOverride;
			CreatureType = template.CreatureType;
			MaxHP = template.HP;
			CurrentHP = template.HP;
			MaxSta = template.Sta;
			CurrentSta = template.Sta;
			Ref = template.Ref;
			Recovery = (template.Body + template.Will) / 2;
			Stun = (template.Body + template.Will) / 2;
		}

		public void UpdateState(string name, int currentHp, int currentSta)
		{
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
			InvalidArgumentException.ThrowIfNotInRange(currentHp, 0, MaxHP, nameof(currentHp));
			InvalidArgumentException.ThrowIfNotInRange(currentSta, 0, MaxSta, nameof(currentSta));

			Name = name;
			CurrentHP = currentHp;
			CurrentSta = currentSta;
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

		public int GetArmorReduction(long partId) => ArmorReductionByPartId.TryGetValue(partId, out var reduction) ? reduction : 0;

		/// <summary>Износ от одного попадания в часть тела — на 1 очко брони, независимо от того, поглотила ли броня урон.</summary>
		public void WearArmor(long partId)
		{
			ArmorReductionByPartId[partId] = GetArmorReduction(partId) + 1;
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

		/// <summary>
		/// Накладывает критическое ранение в конкретный слот (часть тела + тип урона). Если в этом
		/// слоте уже есть ранение не легче нового — новое не применяется (повторное ранение того же
		/// слота только усугубляет, не облегчает). Итоговое ранение слота также появляется бейджем в
		/// AppliedConditions — прежняя метка убирается оттуда, только если её не удерживает другой слот.
		/// </summary>
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
