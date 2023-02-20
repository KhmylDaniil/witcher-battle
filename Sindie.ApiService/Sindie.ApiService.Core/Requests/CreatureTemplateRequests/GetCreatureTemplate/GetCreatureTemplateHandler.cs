using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplate;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using Sindie.ApiService.Core.ExtensionMethods;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.CreatureTemplateRequests.GetCreatureTemplate
{
	/// <summary>
	/// Обработчик команды получения списка шаблонов существа
	/// </summary>
	public class GetCreatureTemplateHandler : BaseHandler<GetCreatureTemplateQuery, GetCreatureTemplateResponse>
	{
		/// <summary>
		/// Провайдер времени
		/// </summary>
		private readonly IDateTimeProvider _dateTimeProvider;

		public GetCreatureTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IDateTimeProvider dateTimeProvider)
			: base(appDbContext, authorizationService)
		{
			_dateTimeProvider = dateTimeProvider;
		}

		/// <summary>
		/// Обработчик команды запроса списка шаблонов существа
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Ответ на запрос списка шаблонов существа</returns>
		public override async Task<GetCreatureTemplateResponse> Handle(GetCreatureTemplateQuery request, CancellationToken cancellationToken)
		{
			if (request.CreationMinTime > _dateTimeProvider.TimeProvider)
				throw new RequestFieldIncorrectDataException<GetCreatureTemplateQuery>(nameof(request.CreationMinTime));
			if (request.ModificationMinTime > _dateTimeProvider.TimeProvider)
				throw new RequestFieldIncorrectDataException<GetCreatureTemplateQuery>(nameof(request.CreationMinTime));

			var filter = _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(g => g.CreatureTemplates)
					.ThenInclude(ct => ct.BodyTemplate)
				.Include(g => g.CreatureTemplates)
					.ThenInclude(ct => ct.CreatureTemplateParts)
				.Include(g => g.CreatureTemplates)
					.ThenInclude(ct => ct.Abilities)
						.ThenInclude(a => a.AppliedConditions)
				.SelectMany(x => x.CreatureTemplates
					.Where(x => request.UserName == null || x.Game.UserGames
						.Any(u => u.User.Name.Contains(request.UserName) && u.UserId == x.CreatedByUserId))
					.Where(x => request.Name == null || x.Name.Contains(request.Name))
					.Where(x => request.CreatureType == null || Enum.GetName(x.CreatureType).Contains(request.CreatureType))
					.Where(x => request.BodyTemplateName == null || x.BodyTemplate.Name.Contains(request.BodyTemplateName))
					.Where(x => request.BodyPartType == null || x.CreatureTemplateParts.Any(x => Enum.GetName(x.BodyPartType).Contains(request.BodyPartType)))
					.Where(x => x.CreatedOn >= request.CreationMinTime)
					.Where(x => (request.CreationMaxTime == default && x.CreatedOn <= _dateTimeProvider.TimeProvider)
					|| x.CreatedOn <= request.CreationMaxTime)
					.Where(x => x.ModifiedOn >= request.ModificationMinTime)
					.Where(x => (request.ModificationMaxTime == default && x.ModifiedOn <= _dateTimeProvider.TimeProvider)
					|| x.ModifiedOn <= request.ModificationMaxTime)
					.Where(ct => request.ConditionName == null || ct.Abilities
						.Any(a => a.AppliedConditions.Any(ac => CritNames.GetConditionFullName(ac.Condition).Contains(request.ConditionName)))));

			var list = await filter
				.OrderBy(request.OrderBy, request.IsAscending)
				.Skip(request.PageSize * (request.PageNumber - 1))
				.Take(request.PageSize)
				.Select(x => new GetCreatureTemplateResponseItem()
				{
					Name = x.Name,
					Description = x.Description,
					GameId = x.GameId,
					Id = x.Id,
					CreatureType = x.CreatureType,
					BodyTemplateName = x.BodyTemplate.Name,
					CreatedByUserId = x.CreatedByUserId,
					ModifiedByUserId = x.ModifiedByUserId,
					CreatedOn = x.CreatedOn,
					ModifiedOn = x.ModifiedOn
				}).ToListAsync(cancellationToken);

			return new GetCreatureTemplateResponse { CreatureTemplatesList = list, TotalCount = list.Count };
		}
	}
}
