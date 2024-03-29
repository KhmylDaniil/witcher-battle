﻿using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.CreatureTemplateRequests
{
	/// <summary>
	/// Обработчик запроса предоставления шаблона существа по айди
	/// </summary>
	public class GetCreatureTemplateByIdHandler : BaseHandler<GetCreatureTemplateByIdQuery, GetCreatureTemplateByIdResponse>
	{
		public GetCreatureTemplateByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService)
		{
		}

		/// <summary>
		/// Обработчик запроса получения шаблона существа по айди
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Шаблон существа</returns>
		public override async Task<GetCreatureTemplateByIdResponse> Handle(
			GetCreatureTemplateByIdQuery request, CancellationToken cancellationToken)
		{
			var filter = _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(x => x.CreatureTemplates.Where(x => x.Id == request.Id))
					.ThenInclude(x => x.CreatureTemplateSkills)
				.Include(x => x.CreatureTemplates.Where(x => x.Id == request.Id))
					.ThenInclude(x => x.Abilities)
					.ThenInclude(x => x.AppliedConditions)
				.Include(x => x.CreatureTemplates.Where(x => x.Id == request.Id))
					.ThenInclude(x => x.CreatureTemplateParts)
				.SelectMany(x => x.CreatureTemplates);

			var creatureTemplate = await filter.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
				?? throw new ExceptionEntityNotFound<CreatureTemplate>(request.Id);

			return new GetCreatureTemplateByIdResponse()
			{
				Id = creatureTemplate.Id,
				ImgFileId = creatureTemplate.ImgFileId,
				BodyTemplateId = creatureTemplate.BodyTemplateId,
				Name = creatureTemplate.Name,
				Description = creatureTemplate.Description,
				CreatureType = creatureTemplate.CreatureType,
				HP = creatureTemplate.HP,
				Sta = creatureTemplate.Sta,
				Int = creatureTemplate.Int,
				Ref = creatureTemplate.Ref,
				Dex = creatureTemplate.Dex,
				Body = creatureTemplate.Body,
				Emp = creatureTemplate.Emp,
				Cra = creatureTemplate.Cra,
				Will = creatureTemplate.Will,
				Speed = creatureTemplate.Speed,
				Luck = creatureTemplate.Luck,
				CreatedByUserId = creatureTemplate.CreatedByUserId,
				ModifiedByUserId = creatureTemplate.ModifiedByUserId,
				CreatedOn = creatureTemplate.CreatedOn,
				ModifiedOn = creatureTemplate.ModifiedOn,
				CreatureTemplateParts = creatureTemplate.CreatureTemplateParts
					.Select(x => new GetCreatureTemplateByIdResponseBodyPart()
					{
						Id = x.Id,
						Name = x.Name,
						BodyPartType = x.BodyPartType,
						HitPenalty = x.HitPenalty,
						DamageModifier = x.DamageModifier,
						MinToHit = x.MinToHit,
						MaxToHit = x.MaxToHit,
						Armor = x.Armor
					}).ToList(),
				CreatureTemplateSkills = creatureTemplate.CreatureTemplateSkills
					.Select(x => new GetCreatureTemplateByIdResponseSkill()
					{
						Id = x.Id,
						Skill = x.Skill,
						SkillValue = x.SkillValue
					}).ToList(),
				Abilities = creatureTemplate.Abilities
				.Select(x => new GetCreatureTemplateByIdResponseAbility()
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description,
					AttackSkill = x.AttackSkill,
					AttackDiceQuantity = x.AttackDiceQuantity,
					DamageModifier = x.DamageModifier,
					Accuracy = x.Accuracy,
					AttackSpeed = x.AttackSpeed,
					AppliedConditions = x.AppliedConditions
					.Select(x => new GetCreatureTemplateByIdResponseAppliedCondition()
					{
						Id = x.Id,
						Condition = x.Condition,
						ApplyChance = x.ApplyChance
					}).ToList()
				}).ToList(),
				DamageTypeModifiers = creatureTemplate.DamageTypeModifiers
				.Select(x => new GetCreatureTemplateByIdResponseDamageTypeModifier()
				{
					Id = x.Id,
					DamageType = x.DamageType,
					DamageTypeModifier = x.DamageTypeModifier
				}).ToList(),
			};
		}
	}
}
