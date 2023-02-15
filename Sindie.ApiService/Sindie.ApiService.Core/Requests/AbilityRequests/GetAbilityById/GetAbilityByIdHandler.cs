﻿using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.AbilityRequests.GetAbilityById;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplateById;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.AbilityRequests.GetAbilityById
{
	public class GetAbilityByIdHandler : BaseHandler<GetAbilityByIdQuery, Ability>
	{
		public GetAbilityByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService) { }

		public override async Task<Ability> Handle(GetAbilityByIdQuery request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new RequestNullException<GetBodyTemplateByIdQuery>();

			var filter = _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, GameRoles.MasterRoleId)
				.Include(x => x.Abilities.Where(x => x.Id == request.Id))
					.ThenInclude(x => x.AppliedConditions)
				.SelectMany(x => x.Abilities);

			return await filter.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken) ?? throw new ExceptionEntityNotFound<Ability>(request.Id);
		}
	}
}
