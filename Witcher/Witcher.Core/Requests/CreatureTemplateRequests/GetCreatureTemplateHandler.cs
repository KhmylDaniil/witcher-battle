using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Exceptions.RequestExceptions;
using Witcher.Core.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.CreatureTemplateRequests
{
	/// <summary>
	/// Обработчик команды получения списка шаблонов существа
	/// </summary>
	public class GetCreatureTemplateHandler : BaseHandler<GetCreatureTemplateQuery, IEnumerable<GetCreatureTemplateResponseItem>>
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
		public override async Task<IEnumerable<GetCreatureTemplateResponseItem>> Handle(
			GetCreatureTemplateQuery request, CancellationToken cancellationToken)
		{
			var conditionMatches = request.ConditionName.GetPossibleEnumNumbersFromSearchString<Condition>();
			var creatureTypeMatches = request.CreatureType.GetPossibleEnumNumbersFromSearchString<Enums.CreatureType>();
			var bodyPartTypeMatches = request.BodyPartType.GetPossibleEnumNumbersFromSearchString<Enums.BodyPartType>();

			var filter = _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.CreatureTemplates)
					.ThenInclude(ct => ct.BodyTemplate)
				.Include(g => g.CreatureTemplates)
					.ThenInclude(ct => ct.CreatureTemplateParts)
				.Include(g => g.CreatureTemplates)
					.ThenInclude(ct => ct.Abilities)
				.SelectMany(x => x.CreatureTemplates
					.Where(x => request.UserName == null || x.Game.UserGames
						.Any(u => u.User.Name.Contains(request.UserName) && u.UserId == x.CreatedByUserId))
					.Where(x => request.Name == null || x.Name.Contains(request.Name))
					.Where(x => request.CreatureType == null || creatureTypeMatches.Any(ctm => ctm == (int)x.CreatureType))
					.Where(x => request.BodyTemplateName == null || x.BodyTemplate.Name.Contains(request.BodyTemplateName))
					.Where(x => request.BodyPartType == null || x.CreatureTemplateParts.Any(ctp => bodyPartTypeMatches.Any(bptm => bptm == (int)ctp.BodyPartType)))
					.Where(x => x.CreatedOn >= request.CreationMinTime)
					.Where(x => request.CreationMaxTime == default && x.CreatedOn <= _dateTimeProvider.TimeProvider
					|| x.CreatedOn <= request.CreationMaxTime)
					.Where(x => x.ModifiedOn >= request.ModificationMinTime)
					.Where(x => request.ModificationMaxTime == default && x.ModifiedOn <= _dateTimeProvider.TimeProvider
					|| x.ModifiedOn <= request.ModificationMaxTime)
					.Where(ct => request.ConditionName == null || ct.Abilities
						.Any(a => a.AppliedConditions.Any(ac => conditionMatches.Any(ctm => ctm == (int)ac.Condition)))));

			var list = await filter
				.OrderBy(request.OrderBy, request.IsAscending)
				.Skip(request.PageSize * (request.PageNumber - 1))
				.Take(request.PageSize)
				.Select(x => new GetCreatureTemplateResponseItem()
				{
					Name = x.Name,
					Description = x.Description,
					Id = x.Id,
					CreatureType = x.CreatureType,
					BodyTemplateName = x.BodyTemplate.Name,
					CreatedByUserId = x.CreatedByUserId,
					ModifiedByUserId = x.ModifiedByUserId,
					CreatedOn = x.CreatedOn,
					ModifiedOn = x.ModifiedOn
				}).ToListAsync(cancellationToken);

			return list;
		}
	}
}
