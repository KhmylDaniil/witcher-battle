﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.AbilityRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;

namespace Sindie.ApiService.Core.Requests.AbilityRequests
{
	public class CreateDefensiveSkillHandler : BaseHandler<CreateDefensiveSkillCommand, Unit>
	{
		public CreateDefensiveSkillHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService) { }

		public override async Task<Unit> Handle(CreateDefensiveSkillCommand request, CancellationToken cancellationToken)
		{
			if (!Enum.IsDefined(request.Skill))
				throw new ExceptionFieldOutOfRange<CreateDefensiveSkillCommand>(nameof(request.Skill));

			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(g => g.Abilities.Where(a => a.Id == request.AbilityId))
					.ThenInclude(a => a.DefensiveSkills)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionNoAccessToEntity<Game>();

			var ability = game.Abilities.FirstOrDefault(a => a.Id == request.AbilityId)
				?? throw new ExceptionEntityNotFound<Ability>(request.AbilityId);

			if (ability.DefensiveSkills.Any(x => x.Skill == request.Skill))
				throw new RequestFieldIncorrectDataException<CreateDefensiveSkillCommand>(nameof(request.Skill), "Указанный навык уже включен в список");

			ability.DefensiveSkills.Add(new DefensiveSkill(ability.Id, request.Skill));

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}