using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplateById;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.CreatureTemplateRequests.GetCreatureTemplateById
{
	/// <summary>
	/// Обработчик запроса предоставления шаблона существа по айди
	/// </summary>
	public class GetCreatureTemplateByIdHandler : IRequestHandler<GetCreatureTemplateByIdQuery, GetCreatureTemplateByIdResponse>
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Сервис авторизации
		/// </summary>
		private readonly IAuthorizationService _authorizationService;

		/// <summary>
		/// Конструктор обработчика запроса шаблона существа по айди
		/// </summary>
		/// <param name="appDbContext"></param>
		/// <param name="authorizationService"></param>
		public GetCreatureTemplateByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Обработчик запроса получения шаблона существа по айди
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Шаблон существа</returns>
		public async Task<GetCreatureTemplateByIdResponse> Handle(GetCreatureTemplateByIdQuery request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ExceptionRequestNull<GetCreatureTemplateByIdQuery>();

			var filter = _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, GameRoles.MasterRoleId)
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
				GameId = creatureTemplate.GameId,
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
			};
		}
	}
}
