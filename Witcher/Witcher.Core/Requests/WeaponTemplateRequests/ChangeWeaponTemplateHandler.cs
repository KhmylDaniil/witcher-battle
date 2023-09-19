using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.WeaponTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.WeaponTemplateRequests
{
	public sealed class ChangeWeaponTemplateHandler : BaseHandler<ChangeWeaponTemplateCommand, Unit>
	{
		public ChangeWeaponTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<Unit> Handle(ChangeWeaponTemplateCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.ItemTemplates)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			if (game.ItemTemplates.Exists(x => x.Name == request.Name && x.Id != request.Id))
				throw new RequestNameNotUniqException<ItemTemplate>(request.Name);

			var weaponTemplate = game.ItemTemplates.Find(x => x.Id == request.Id) as WeaponTemplate
				?? throw new EntityNotFoundException<WeaponTemplate>(request.Id);

			weaponTemplate.ChangeWeaponTemplate(
				name: request.Name,
				description: request.Description,
				weight: request.Weight,
				price: request.Price,
				attackSkill: request.AttackSkill,
				damageType: request.DamageType,
				attackDiceQuantity: request.AttackDiceQuantity,
				damageModifier: request.DamageModifier,
				accuracy: request.Accuracy,
				durability: request.Durability,
				range: request.Range);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
