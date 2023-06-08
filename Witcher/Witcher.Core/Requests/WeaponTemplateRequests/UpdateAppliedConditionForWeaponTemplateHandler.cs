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
	public class UpdateAppliedConditionForWeaponTemplateHandler : BaseHandler<UpdateAppliedConditionForWeaponTemplateCommand, Unit>
	{
		public UpdateAppliedConditionForWeaponTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService)
		{
		}

		public async override Task<Unit> Handle(UpdateAppliedConditionForWeaponTemplateCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.ItemTemplates.Where(a => a.Id == request.WeaponTemplateId))
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new NoAccessToEntityException<Game>();

			var weaponTemplate = game.ItemTemplates.FirstOrDefault(a => a.Id == request.WeaponTemplateId) as WeaponTemplate
				?? throw new EntityNotFoundException<ItemTemplate>(request.WeaponTemplateId);

			var currentAppliedCondition = weaponTemplate.AppliedConditions.FirstOrDefault(x => x.Id == request.Id);

			if (weaponTemplate.AppliedConditions.Any(x => x.Condition == request.Condition && x.Id != request.Id))
				throw new RequestNotUniqException<UpdateAppliedConditionForWeaponTemplateCommand>(nameof(request.Condition));

			if (currentAppliedCondition is null)
				weaponTemplate.AppliedConditions.Add(new AppliedCondition(request.Condition, request.ApplyChance));
			else
				currentAppliedCondition.ChangeAppliedCondition(request.Condition, request.ApplyChance);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
