using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.CreatureTemplateRequests
{
	public class ChangeDamageTypeModifierHandler : BaseHandler<ChangeDamageTypeModifierCommand, Unit>
	{
		public ChangeDamageTypeModifierHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService)
		{
		}

		public async override Task<Unit> Handle(ChangeDamageTypeModifierCommand request, CancellationToken cancellationToken)
		{
			var creatureTemplate = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.SelectMany(g => g.CreatureTemplates.Where(g => g.Id == request.CreatureTemplateId))
					.Include(ct => ct.DamageTypeModifiers)
				.FirstOrDefaultAsync(ct => ct.Id == request.CreatureTemplateId, cancellationToken)
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
