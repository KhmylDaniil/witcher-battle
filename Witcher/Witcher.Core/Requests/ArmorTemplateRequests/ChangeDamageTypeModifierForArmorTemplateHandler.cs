using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.ArmorTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.ArmorTemplateRequests
{
	public class ChangeDamageTypeModifierForArmorTemplateHandler : BaseHandler<ChangeDamageTypeModifierForArmorTemplateCommand, Unit>
	{
		public ChangeDamageTypeModifierForArmorTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService)
		{
		}

		public async override Task<Unit> Handle(ChangeDamageTypeModifierForArmorTemplateCommand request, CancellationToken cancellationToken)
		{
			var armorTemplate = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.SelectMany(g => g.ItemTemplates.Where(g => g.Id == request.ArmorTemplateId))
				.FirstOrDefaultAsync(at => at.Id == request.ArmorTemplateId, cancellationToken) as ArmorTemplate
					?? throw new NoAccessToEntityException<Game>();

			var damageTypeModifier = armorTemplate.DamageTypeModifiers
				.FirstOrDefault(x => x.DamageType == request.DamageType);

			EntityDamageTypeModifier.ChangeDamageTypeModifer(request.DamageTypeModifier, request.DamageType, armorTemplate, damageTypeModifier);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
