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
			InvalidArgumentException.ThrowIfNullOrEmpty(draft.Name, nameof(draft.Name));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(draft.DamageModifier, nameof(draft.DamageModifier));
			InvalidArgumentException.ThrowIfLessOrEqualToZero(draft.HitPenalty, nameof(draft.HitPenalty));
			InvalidArgumentException.ThrowIfNotInRange(draft.MinToHit, MinHitRange, MaxHitRange, nameof(draft.MinToHit));
			InvalidArgumentException.ThrowIfNotInRange(draft.MaxToHit, draft.MinToHit, MaxHitRange, nameof(draft.MaxToHit));

			Name = draft.Name;
			BodyPartType = draft.BodyPartType;
			DamageModifier = draft.DamageModifier;
			HitPenalty = draft.HitPenalty;
			MinToHit = draft.MinToHit;
			MaxToHit = draft.MaxToHit;
		}
	}
}
