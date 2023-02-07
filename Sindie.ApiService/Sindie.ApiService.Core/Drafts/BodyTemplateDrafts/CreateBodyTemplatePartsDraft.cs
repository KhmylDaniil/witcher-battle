using Sindie.ApiService.Core.Requests.BodyTemplateRequests;
using System.Collections.Generic;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Sindie.ApiService.Core.Drafts.BodyTemplateDrafts
{
	public static class CreateBodyTemplatePartsDraft
	{
		public static List<BodyTemplatePartsData> CreateBodyPartsDraft()
			=> new()
			{
				new BodyTemplatePartsData()
				{
					Name = "Head",
					BodyPartType = BodyPartType.Head,
					HitPenalty = 4,
					DamageModifier = 3,
					MinToHit = 1,
					MaxToHit = 1
				},
				new BodyTemplatePartsData()
				{
					Name = "Torso",
					BodyPartType = BodyPartType.Torso,
					HitPenalty = 1,
					DamageModifier = 1,
					MinToHit = 2,
					MaxToHit = 4
				},
				new BodyTemplatePartsData()
				{
					Name = "Right arm",
					BodyPartType = BodyPartType.Arm,
					HitPenalty = 3,
					DamageModifier = 0.5,
					MinToHit = 5,
					MaxToHit = 5
				},
				new BodyTemplatePartsData()
				{
					Name = "Left arm",
					BodyPartType = BodyPartType.Arm,
					HitPenalty = 3,
					DamageModifier = 0.5,
					MinToHit = 6,
					MaxToHit = 6
				},
				new BodyTemplatePartsData()
				{
					Name = "Right leg",
					BodyPartType = BodyPartType.Leg,
					HitPenalty = 2,
					DamageModifier = 0.5,
					MinToHit = 7,
					MaxToHit = 8
				},
				new BodyTemplatePartsData()
				{
					Name = "Left leg",
					BodyPartType = BodyPartType.Leg,
					HitPenalty = 2,
					DamageModifier = 0.5,
					MinToHit = 9,
					MaxToHit = 10
				}
			};
	}
}
