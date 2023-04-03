using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Contracts.CharacterRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;

namespace Witcher.Core.Requests.CharacterRequests
{
	public class GetCharacterByIdHandler : BaseHandler<GetCharacterByIdCommand, GetCharacterByIdResponse>
	{
		public GetCharacterByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<GetCharacterByIdResponse> Handle(GetCharacterByIdCommand request, CancellationToken cancellationToken)
		{
			var creature = await _authorizationService.UserGameFilter(_appDbContext.Games)
				.Include(g => g.Characters.Where(c => c.Id == request.Id))
					.ThenInclude(c => c.CreatureTemplate)
							.ThenInclude(ct => ct.BodyTemplate)
				.Include(g => g.Characters.Where(c => c.Id == request.Id))
					.ThenInclude(c => c.CreatureParts)
				.Include(g => g.Characters.Where(c => c.Id == request.Id))
					.ThenInclude(c => c.CreatureSkills)
				.Include(g => g.Characters.Where(c => c.Id == request.Id))
					.ThenInclude(c => c.Abilities)
						.ThenInclude(a => a.AppliedConditions)
				.Include(g => g.Characters.Where(c => c.Id == request.Id))
					.ThenInclude(c => c.DamageTypeModifiers)
				.Include(g => g.Characters.Where(c => c.Id == request.Id))
					.ThenInclude(c => c.Effects)
				.SelectMany(g => g.Characters).FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
				?? throw new EntityNotFoundException<Character>(request.Id);

			return new GetCharacterByIdResponse()
			{
				Id = creature.Id,
				CreatureTemplateId = creature.CreatureTemplateId,
				CreatureTemplateName = creature.CreatureTemplate.Name,
				BodyTemplateId = creature.CreatureTemplate.BodyTemplateId,
				BodyTemplateName = creature.CreatureTemplate.BodyTemplate.Name,
				Name = creature.Name,
				Description = creature.Description,
				CreatureType = creature.CreatureType,
				HP = (creature.HP, creature.MaxHP),
				Sta = (creature.Sta, creature.MaxSta),
				Int = (creature.Int, creature.MaxInt),
				Ref = (creature.Ref, creature.MaxRef),
				Dex = (creature.Dex, creature.MaxDex),
				Body = (creature.Body, creature.MaxBody),
				Emp = (creature.Emp, creature.MaxEmp),
				Cra = (creature.Cra, creature.MaxCra),
				Will = (creature.Will, creature.MaxWill),
				Speed = (creature.Speed, creature.MaxSpeed),
				Luck = (creature.Luck, creature.MaxLuck),

				CreatureParts = creature.CreatureParts
					.Select(x => new GetCreatureByIdResponsePart()
					{
						Name = x.Name,
						BodyPartType = x.BodyPartType,
						HitPenalty = x.HitPenalty,
						DamageModifier = x.DamageModifier,
						MinToHit = x.MinToHit,
						MaxToHit = x.MaxToHit,
						Armor = (x.CurrentArmor, x.StartingArmor)
					}).ToList(),
				CreatureSkills = creature.CreatureSkills
					.Select(x => new GetCreatureByIdResponseSkill()
					{
						Skill = x.Skill,
						SkillValue = (x.SkillValue, x.MaxValue)
					}).ToList(),
				Abilities = creature.Abilities
				.Select(x => new GetCreatureByIdResponseAbility()
				{
					Id = x.Id,
					Name = x.Name,
					AttackSkill = x.AttackSkill,
					AttackDiceQuantity = x.AttackDiceQuantity,
					DamageModifier = x.DamageModifier,
					Accuracy = x.Accuracy,
					AttackSpeed = x.AttackSpeed,
					AppliedConditions = x.AppliedConditions
					.Select(x => (x.Condition, x.ApplyChance)).ToList()
				}).ToList(),
				DamageTypeModifiers = creature.DamageTypeModifiers
				.Select(x => (x.DamageType, x.DamageTypeModifier)).ToList(),
			};
		}
	}
}
