using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplate;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using Sindie.ApiService.Core.ExtensionMethods;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BodyTemplateRequests.GetBodyTemplate
{
	/// <summary>
	/// Обработчик запроса списка шаблонов тела
	/// </summary>
	public class GetBodyTemplateHandler : BaseHandler<GetBodyTemplateQuery, GetBodyTemplateResponse>
	{
		/// <summary>
		/// Провайдер времени
		/// </summary>
		private readonly IDateTimeProvider _dateTimeProvider;

		public GetBodyTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IDateTimeProvider dateTimeProvider)
			: base(appDbContext, authorizationService)
		{
			_dateTimeProvider = dateTimeProvider;
		}

		/// <summary>
		/// Запрос списка шаблонов тела
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Ответ на запрос списка шаблонов тела</returns>
		public override async Task<GetBodyTemplateResponse> Handle(GetBodyTemplateQuery request, CancellationToken cancellationToken)
		{
			if (request.CreationMinTime > _dateTimeProvider.TimeProvider)
				throw new RequestFieldIncorrectDataException<GetBodyTemplateQuery>(nameof(request.CreationMinTime));
			if (request.ModificationMinTime > _dateTimeProvider.TimeProvider)
				throw new RequestFieldIncorrectDataException<GetBodyTemplateQuery>(nameof(request.CreationMinTime));

			var filter = _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(g => g.BodyTemplates)
					.ThenInclude(bt => bt.BodyTemplateParts)
					.SelectMany(x => x.BodyTemplates
						.Where(x => request.UserName == null || x.Game.UserGames
							.Any(u => u.User.Name.Contains(request.UserName) && u.UserId == x.CreatedByUserId))
						.Where(x => request.Name == null || x.Name.Contains(request.Name))
						.Where(x => x.CreatedOn >= request.CreationMinTime)
						.Where(x => (request.CreationMaxTime == default && x.CreatedOn <= _dateTimeProvider.TimeProvider)
						|| x.CreatedOn <= request.CreationMaxTime)
						.Where(x => x.ModifiedOn >= request.ModificationMinTime)
						.Where(x => (request.ModificationMaxTime == default && x.ModifiedOn <= _dateTimeProvider.TimeProvider)
						|| x.ModifiedOn <= request.ModificationMaxTime))
						.Where(x => request.BodyPartName == null || x.BodyTemplateParts.Any(x => x.Name.Contains(request.BodyPartName)));

			var list = await filter
				.OrderBy(request.OrderBy, request.IsAscending)
				.Skip(request.PageSize * (request.PageNumber - 1))
				.Take(request.PageSize)
				.Select(x => new GetBodyTemplateResponseItem()
				{
					Name = x.Name,
					Description = x.Description,
					Id = x.Id,
					GameId = x.GameId,
					CreatedByUserId = x.CreatedByUserId,
					ModifiedByUserId = x.ModifiedByUserId,
					CreatedOn = x.CreatedOn,
					ModifiedOn = x.ModifiedOn
				}).ToListAsync(cancellationToken);

			return new GetBodyTemplateResponse { BodyTemplatesList = list, TotalCount = list.Count };
		}
	}
}
