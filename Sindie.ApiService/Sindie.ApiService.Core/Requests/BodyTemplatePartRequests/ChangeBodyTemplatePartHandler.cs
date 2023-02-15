using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BodyTemplatePartsRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Requests.BodyTemplateRequests;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BodyTemplatePartRequests
{
	public class ChangeBodyTemplatePartHandler : BaseHandler<ChangeBodyTemplatePartCommand, Unit>
	{
		public ChangeBodyTemplatePartHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<Unit> Handle(ChangeBodyTemplatePartCommand request, CancellationToken cancellationToken)
		{
			var bodyTemplate = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(g => g.BodyTemplates.Where(bt => bt.Id == request.Id))
					.ThenInclude(bt => bt.BodyTemplateParts)
				.SelectMany(g => g.BodyTemplates)
				.FirstOrDefaultAsync(cancellationToken)
						?? throw new ExceptionNoAccessToEntity<Game>();

			bodyTemplate.CreateBodyTemplateParts(BodyTemplatePartsData.CreateBodyTemplatePartsData(bodyTemplate.BodyTemplateParts, request));

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
