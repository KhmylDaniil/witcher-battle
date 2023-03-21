using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BodyTemplateRequests;
using Witcher.Core.Exceptions.RequestExceptions;
using Witcher.Core.ExtensionMethods;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.BodyTemplateRequests
{
	/// <summary>
	/// Обработчик запроса списка шаблонов тела
	/// </summary>
	public class GetBodyTemplateHandler : BaseHandler<GetBodyTemplateQuery, IEnumerable<GetBodyTemplateResponseItem>>
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
		public override async Task<IEnumerable<GetBodyTemplateResponseItem>> Handle(GetBodyTemplateQuery request, CancellationToken cancellationToken)
		{
			if (request.CreationMinTime > _dateTimeProvider.TimeProvider)
				throw new RequestFieldIncorrectDataException<GetBodyTemplateQuery>(nameof(request.CreationMinTime));
			if (request.ModificationMinTime > _dateTimeProvider.TimeProvider)
				throw new RequestFieldIncorrectDataException<GetBodyTemplateQuery>(nameof(request.CreationMinTime));

			var filter = _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.BodyTemplates)
					.ThenInclude(bt => bt.BodyTemplateParts)
					.SelectMany(x => x.BodyTemplates
						.Where(x => request.UserName == null || x.Game.UserGames
							.Any(u => u.User.Name.Contains(request.UserName) && u.UserId == x.CreatedByUserId))
						.Where(x => request.Name == null || x.Name.Contains(request.Name))
						.Where(x => x.CreatedOn >= request.CreationMinTime)
						.Where(x => request.CreationMaxTime == default && x.CreatedOn <= _dateTimeProvider.TimeProvider
						|| x.CreatedOn <= request.CreationMaxTime)
						.Where(x => x.ModifiedOn >= request.ModificationMinTime)
						.Where(x => request.ModificationMaxTime == default && x.ModifiedOn <= _dateTimeProvider.TimeProvider
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
					CreatedByUserId = x.CreatedByUserId,
					ModifiedByUserId = x.ModifiedByUserId,
					CreatedOn = x.CreatedOn,
					ModifiedOn = x.ModifiedOn
				}).ToListAsync(cancellationToken);

			return list;
		}
	}
}
