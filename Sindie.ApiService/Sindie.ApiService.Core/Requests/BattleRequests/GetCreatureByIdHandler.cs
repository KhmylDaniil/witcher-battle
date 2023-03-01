using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BattleRequests
{
	public class GetCreatureByIdHandler : BaseHandler<GetCreatureByIdQuery, GetCreatureByIdResponse>
	{
		public GetCreatureByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<GetCreatureByIdResponse> Handle(GetCreatureByIdQuery request, CancellationToken cancellationToken)
		{
			var creature = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Battles.Where(b => b.Id == request.BattleId))
					.ThenInclude(b => b.Creatures.Where(c => c.Id == request.Id))
						.ThenInclude(c => c.CreatureTemplate)
							.ThenInclude(ct => ct.BodyTemplate)
				.Include(g => g.Battles.Where(b => b.Id == request.BattleId))
					.ThenInclude(b => b.Creatures.Where(c => c.Id == request.Id))
						.ThenInclude(c => c.CreatureParts)
				.Include(g => g.Battles.Where(b => b.Id == request.BattleId))
					.ThenInclude(b => b.Creatures.Where(c => c.Id == request.Id))
						.ThenInclude(c => c.CreatureSkills)
				.Include(g => g.Battles.Where(b => b.Id == request.BattleId))
					.ThenInclude(b => b.Creatures.Where(c => c.Id == request.Id))
						.ThenInclude(c => c.Abilities)
							.ThenInclude(a => a.AppliedConditions)
				.Include(g => g.Battles.Where(b => b.Id == request.BattleId))
					.ThenInclude(b => b.Creatures.Where(c => c.Id == request.Id))
						.ThenInclude(c => c.DamageTypeModifiers)
				.SelectMany(x => x.Battles).SelectMany(b => b.Creatures).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
				?? throw new ExceptionEntityNotFound<Creature>(request.Id);

			return new GetCreatureByIdResponse()
			{
				Id = creature.Id,
				BattleId = creature.BattleId,
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
