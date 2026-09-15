using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.EfDataAccess.Entities;
using Wastelands.Service.Domain.Drafts;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Entities
{
	public class BodyTemplate : Entity
	{
		public long GameId { get; private set; }

		public string Name { get; private set; }

		public string? Description { get; private set; }

		public List<BodyTemplatePart> Parts { get; private set; } = [];

		private BodyTemplate()
		{
		}

		public BodyTemplate(long gameId, string name, string? description)
		{
			InvalidArgumentException.ThrowIfLessOrEqualToZero(gameId, nameof(gameId));
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

			GameId = gameId;
			Name = name;
			Description = description;

			Parts = DefaultHumanBodyTemplatePartsDraft.Create()
				.Select(draft => new BodyTemplatePart(draft))
				.ToList();
		}

		public void UpdateBodyTemplate(string name, string? description)
		{
			InvalidArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

			Name = name;
			Description = description;
		}

		public BodyTemplatePart AddPart(string name, BodyPartType bodyPartType, double damageModifier, int hitPenalty, int minToHit, int maxToHit)
		{
			ThrowIfRangeOverlaps(minToHit, maxToHit, excludePartId: null);

			var part = new BodyTemplatePart(name, bodyPartType, damageModifier, hitPenalty, minToHit, maxToHit);
			Parts.Add(part);

			return part;
		}

		public void UpdatePart(long partId, string name, BodyPartType bodyPartType, double damageModifier, int hitPenalty, int minToHit, int maxToHit)
		{
			var part = GetPart(partId);
			ThrowIfRangeOverlaps(minToHit, maxToHit, excludePartId: partId);

			part.UpdatePart(name, bodyPartType, damageModifier, hitPenalty, minToHit, maxToHit);
		}

		public void RemovePart(long partId)
		{
			var part = GetPart(partId);
			Parts.Remove(part);
		}

		private BodyTemplatePart GetPart(long partId)
		{
			var part = Parts.FirstOrDefault(x => x.Id == partId);

			NotFoundException.ThrowIfNull(
				part,
				ErrorCode.BodyTemplatePartNotFound,
				nameof(BodyTemplatePart),
				nameof(BodyTemplatePart.Id),
				partId.ToString());

			return part;
		}

		// Каждое значение кубика д10 (1..10) должно однозначно указывать на одну часть тела — если
		// диапазоны пересекаются, бросок за случайную часть тела становится неоднозначным.
		private void ThrowIfRangeOverlaps(int minToHit, int maxToHit, long? excludePartId)
		{
			var overlaps = Parts.Any(p => p.Id != excludePartId && p.MinToHit <= maxToHit && minToHit <= p.MaxToHit);

			if (overlaps)
			{
				throw new InvalidArgumentException(
					ErrorCode.BodyTemplatePartRangeOverlap,
					"Диапазон значений кубика д10 пересекается с уже существующей частью тела.");
			}
		}
	}
}
