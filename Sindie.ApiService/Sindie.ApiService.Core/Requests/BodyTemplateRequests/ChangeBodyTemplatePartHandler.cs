using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BodyTemplateRequests
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
						?? throw new ExceptionNoAccessToEntity<Game>();

			bodyTemplate.CreateBodyTemplateParts(BodyTemplatePartsData.CreateBodyTemplatePartsData(bodyTemplate.BodyTemplateParts, request));

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
