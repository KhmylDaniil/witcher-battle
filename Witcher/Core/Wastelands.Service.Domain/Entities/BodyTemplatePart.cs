using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Drafts;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	public class BodyTemplatePart : Entity
	{
		public const int MinHitRange = 1;
		public const int MaxHitRange = 10;

		public long BodyTemplateId { get; private set; }

		public string Name { get; private set; }

		public BodyPartType BodyPartType { get; private set; }

		public double DamageModifier { get; private set; }

		public int HitPenalty { get; private set; }

		public int MinToHit { get; private set; }

		public int MaxToHit { get; private set; }

		private BodyTemplatePart()
		{
		}

		// Без параметра bodyTemplateId: часть создаётся как элемент BodyTemplate.Parts ещё до того, как у
		// самого BodyTemplate появится реальный Id (см. BodyTemplate(...)) — BodyTemplateId проставляет EF
		// Core при SaveChanges по связи навигации (тот же приём, что и Game.Characters/Character.GameId).
		internal BodyTemplatePart(BodyTemplatePartDraft draft)
		{
			Validate(draft.Name, draft.DamageModifier, draft.HitPenalty, draft.MinToHit, draft.MaxToHit);

			Name = draft.Name;
			BodyPartType = draft.BodyPartType;
			DamageModifier = draft.DamageModifier;
			HitPenalty = draft.HitPenalty;
			MinToHit = draft.MinToHit;
			MaxToHit = draft.MaxToHit;
		}

		// Часть, добавляемая мастером игры вручную (в отличие от драфта — с произвольным именем/типом/
		// диапазоном кубика д10). Пересечение диапазона с уже существующими частями проверяет
		// BodyTemplate.AddPart, у самой части нет доступа к соседям.
		internal BodyTemplatePart(string name, BodyPartType bodyPartType, double damageModifier, int hitPenalty, int minToHit, int maxToHit)
		{
			Validate(name, damageModifier, hitPenalty, minToHit, maxToHit);

			Name = name;
			BodyPartType = bodyPartType;
			DamageModifier = damageModifier;
			HitPenalty = hitPenalty;
			MinToHit = minToHit;
			MaxToHit = maxToHit;
		}

		internal void UpdatePart(string name, BodyPartType bodyPartType, double damageModifier, int hitPenalty, int minToHit, int maxToHit)
		{
			Validate(name, damageModifier, hitPenalty, minToHit, maxToHit);

			Name = name;
			BodyPartType = bodyPartType;
			DamageModifier = damageModifier;
			HitPenalty = hitPenalty;
			MinToHit = minToHit;
			MaxToHit = maxToHit;
		}

		private static void Validate(string name, double damageModifier, int hitPenalty, int minToHit, int maxToHit)
		{
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(damageModifier, nameof(damageModifier));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(hitPenalty, nameof(hitPenalty));
			InvalidArgumentException.ThrowIfNotInRange(minToHit, MinHitRange, MaxHitRange, nameof(minToHit));
			InvalidArgumentException.ThrowIfNotInRange(maxToHit, minToHit, MaxHitRange, nameof(maxToHit));
		}
	}
}
