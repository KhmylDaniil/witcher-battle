using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.CreateBodyTemplate;
using static Sindie.ApiService.Core.BaseData.Enums;

namespace Witcher.MVC.ViewModels.Drafts
{
	public class CreateBodyTemplateDraft
	{
		public static CreateBodyTemplateRequest CreateDraft(Guid gameId)
		{
			return new CreateBodyTemplateRequest()
			{
				GameId = gameId,
				Name = "draft",
				Description = "description",
				BodyTemplateParts = new List<CreateBodyTemplateRequestItem>
				{
					new CreateBodyTemplateRequestItem()
					{
						Name = "head",
						BodyPartType = BodyPartType.Head,
						HitPenalty = 4,
						DamageModifier = 3,
						MinToHit = 1,
						MaxToHit = 1
					},
					new CreateBodyTemplateRequestItem()
					{
						Name = "torso",
						BodyPartType = BodyPartType.Torso,
						HitPenalty = 1,
						DamageModifier = 1,
						MinToHit = 2,
						MaxToHit = 4
					},
					new CreateBodyTemplateRequestItem()
					{
						Name = "rarm",
						BodyPartType = BodyPartType.Arm,
						HitPenalty = 3,
						DamageModifier = 0.5,
						MinToHit = 5,
						MaxToHit = 5
					},
					new CreateBodyTemplateRequestItem()
					{
						Name = "larm",
						BodyPartType = BodyPartType.Arm,
						HitPenalty = 3,
						DamageModifier = 0.5,
						MinToHit = 6,
						MaxToHit = 6
					},
					new CreateBodyTemplateRequestItem()
					{
						Name = "rleg",
						BodyPartType = BodyPartType.Leg,
						HitPenalty = 2,
						DamageModifier = 0.5,
						MinToHit = 7,
						MaxToHit = 8
					},
					new CreateBodyTemplateRequestItem()
					{
						Name = "lleg",
						BodyPartType = BodyPartType.Leg,
						HitPenalty = 2,
						DamageModifier = 0.5,
						MinToHit = 9,
						MaxToHit = 10
					}
				}
			};
		}
	}
}
