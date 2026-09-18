using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Domain.Drafts
{
	/// <summary>
	/// Дефолтный набор частей тела человека, которым заполняется каждый новый <see cref="Entities.BodyTemplate"/>.
	/// Портировано из Witcher.Core.Drafts.BodyTemplateDrafts.CreateBodyTemplatePartsDraft (значения полей идентичны).
	/// </summary>
	public static class DefaultHumanBodyTemplatePartsDraft
	{
		public static List<BodyTemplatePartDraft> Create()
			=> new()
			{
				new BodyTemplatePartDraft
				{
					Name = "Голова",
					BodyPartType = BodyPartType.Head,
					HitPenalty = 4,
					DamageModifier = 3,
					MinToHit = 1,
					MaxToHit = 1,
				},
				new BodyTemplatePartDraft
				{
					Name = "Торс",
					BodyPartType = BodyPartType.Torso,
					HitPenalty = 1,
					DamageModifier = 1,
					MinToHit = 2,
					MaxToHit = 4,
				},
				new BodyTemplatePartDraft
				{
					Name = "Правая рука",
					BodyPartType = BodyPartType.Arm,
					HitPenalty = 3,
					DamageModifier = 0.5,
					MinToHit = 5,
					MaxToHit = 5,
				},
				new BodyTemplatePartDraft
				{
					Name = "Левая рука",
					BodyPartType = BodyPartType.Arm,
					HitPenalty = 3,
					DamageModifier = 0.5,
					MinToHit = 6,
					MaxToHit = 6,
				},
				new BodyTemplatePartDraft
				{
					Name = "Правая нога",
					BodyPartType = BodyPartType.Leg,
					HitPenalty = 2,
					DamageModifier = 0.5,
					MinToHit = 7,
					MaxToHit = 8,
				},
				new BodyTemplatePartDraft
				{
					Name = "Левая нога",
					BodyPartType = BodyPartType.Leg,
					HitPenalty = 2,
					DamageModifier = 0.5,
					MinToHit = 9,
					MaxToHit = 10,
				},
			};
	}
}
