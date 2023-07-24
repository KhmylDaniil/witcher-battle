using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.WeaponTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.WeaponTemplateRequests
{
	public sealed class DeleteAppliedConditionForWeaponTemplateHandler : BaseHandler<DeleteAppliedConditionForWeaponTemplateCommand, Unit>
	{
		public DeleteAppliedConditionForWeaponTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService)
		{
		}

		public async override Task<Unit> Handle(DeleteAppliedConditionForWeaponTemplateCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.ItemTemplates.Where(a => a.Id == request.WeaponTemplateId))
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new NoAccessToEntityException<Game>();

			var weaponTemplate = game.ItemTemplates.Find(a => a.Id == request.WeaponTemplateId) as WeaponTemplate
				?? throw new EntityNotFoundException<WeaponTemplate>(request.WeaponTemplateId);

			var appliedCondition = weaponTemplate.AppliedConditions.Find(x => x.Id == request.Id)
				?? throw new EntityNotFoundException<AppliedCondition>(request.Id);

			weaponTemplate.AppliedConditions.Remove(appliedCondition);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
