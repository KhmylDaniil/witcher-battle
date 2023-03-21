using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BodyTemplateRequests;
using Witcher.Core.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.BodyTemplateRequests
{
	public class ChangeBodyTemplatePartHandler : BaseHandler<ChangeBodyTemplatePartCommand, Unit>
	{
		public ChangeBodyTemplatePartHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<Unit> Handle(ChangeBodyTemplatePartCommand request, CancellationToken cancellationToken)
		{
			var bodyTemplate = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.BodyTemplates.Where(bt => bt.Id == request.Id))
					.ThenInclude(bt => bt.BodyTemplateParts)
				.SelectMany(g => g.BodyTemplates)
				.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
						?? throw new NoAccessToEntityException<Game>();

			bodyTemplate.CreateBodyTemplateParts(BodyTemplatePartsData.CreateBodyTemplatePartsData(bodyTemplate.BodyTemplateParts, request));

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
