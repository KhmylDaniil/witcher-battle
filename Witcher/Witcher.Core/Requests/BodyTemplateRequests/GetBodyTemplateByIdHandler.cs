using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BodyTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.BodyTemplateRequests
{
	/// <summary>
	/// Запрос шаблона тела по айди
	/// </summary>
	public class GetBodyTemplateByIdHandler : BaseHandler<GetBodyTemplateByIdQuery, GetBodyTemplateByIdResponse>
	{
		public GetBodyTemplateByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		/// <summary>
		/// Запрос шаблона тела по айди
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public override async Task<GetBodyTemplateByIdResponse> Handle(GetBodyTemplateByIdQuery request, CancellationToken cancellationToken)
		{
			var filter = _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(x => x.BodyTemplates.Where(x => x.Id == request.Id))
					.ThenInclude(x => x.BodyTemplateParts)
				.SelectMany(x => x.BodyTemplates);

			var bodyTemplate = await filter.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
				?? throw new EntityNotFoundException<BodyTemplate>(request.Id);

			var bodyTemplateParts = bodyTemplate.BodyTemplateParts.Select(x => new GetBodyTemplateByIdResponseItem()
			{
				Id = x.Id,
				Name = x.Name,
				BodyPartType = x.BodyPartType,
				HitPenalty = x.HitPenalty,
				DamageModifier = x.DamageModifier,
				MinToHit = x.MinToHit,
				MaxToHit = x.MaxToHit
			}).OrderBy(x => x.MinToHit).ToList();

			return new GetBodyTemplateByIdResponse()
			{
				Name = bodyTemplate.Name,
				Description = bodyTemplate.Description,
				Id = bodyTemplate.Id,
				CreatedByUserId = bodyTemplate.CreatedByUserId,
				ModifiedByUserId = bodyTemplate.ModifiedByUserId,
				CreatedOn = bodyTemplate.CreatedOn,
				ModifiedOn = bodyTemplate.ModifiedOn,
				GetBodyTemplateByIdResponseItems = bodyTemplateParts
			};
		}
	}
}
