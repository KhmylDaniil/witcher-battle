using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
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

		public int? Initiative { get; private set; }

		public List<Condition> AppliedConditions { get; private set; } = [];

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
