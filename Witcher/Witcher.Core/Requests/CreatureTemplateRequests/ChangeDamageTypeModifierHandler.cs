using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Exceptions.RequestExceptions;
using static Witcher.Core.BaseData.Enums;

namespace Witcher.Core.Requests.CreatureTemplateRequests
{
	public class ChangeDamageTypeModifierHandler : BaseHandler<ChangeDamageTypeModifierForCreatureTemplateCommand, Unit>
	{
		public ChangeDamageTypeModifierHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService)
		{
		}

		public async override Task<Unit> Handle(ChangeDamageTypeModifierForCreatureTemplateCommand request, CancellationToken cancellationToken)
		{
			var creatureTemplate = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.SelectMany(g => g.CreatureTemplates.Where(g => g.Id == request.CreatureTemplateId))
					.Include(ct => ct.DamageTypeModifiers)
				.FirstOrDefaultAsync(ct => ct.Id == request.CreatureTemplateId, cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			var creatureTemplateDamageTypeModifier = creatureTemplate.DamageTypeModifiers
				.FirstOrDefault(x => x.DamageType == request.DamageType);

			EntityDamageTypeModifier.ChangeDamageTypeModifer(request.DamageTypeModifier, request.DamageType, creatureTemplate, creatureTemplateDamageTypeModifier);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
