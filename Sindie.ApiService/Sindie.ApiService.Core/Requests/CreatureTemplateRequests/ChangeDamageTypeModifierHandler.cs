using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.CreatureTemplateRequests
{
	public class ChangeDamageTypeModifierHandler : BaseHandler<ChangeDamageTypeModifierCommand, Unit>
	{
		public ChangeDamageTypeModifierHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<Unit> Handle(ChangeDamageTypeModifierCommand request, CancellationToken cancellationToken)
		{
			var creatureTemplate = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.SelectMany(g => g.CreatureTemplates.Where(g => g.Id == request.CreatureTemplateId))
					.Include(ct => ct.DamageTypeModifiers)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			var creatureTemplateDamageTypeModifier = creatureTemplate.DamageTypeModifiers
				.FirstOrDefault(x => x.DamageType == request.DamageType);

			if (creatureTemplateDamageTypeModifier == null && request.DamageTypeModifier != BaseData.Enums.DamageTypeModifier.Normal)
				creatureTemplate.DamageTypeModifiers.Add(new CreatureTemplateDamageTypeModifier(
					request.CreatureTemplateId, request.DamageType, request.DamageTypeModifier));
			else if (creatureTemplateDamageTypeModifier != null && request.DamageTypeModifier != BaseData.Enums.DamageTypeModifier.Normal)
				creatureTemplateDamageTypeModifier.DamageTypeModifier = request.DamageTypeModifier;
			else if (creatureTemplateDamageTypeModifier != null && request.DamageTypeModifier == BaseData.Enums.DamageTypeModifier.Normal)
				creatureTemplate.DamageTypeModifiers.Remove(creatureTemplateDamageTypeModifier);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
