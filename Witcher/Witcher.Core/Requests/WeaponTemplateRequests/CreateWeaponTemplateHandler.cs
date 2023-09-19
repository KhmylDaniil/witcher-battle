using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.WeaponTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.WeaponTemplateRequests
{
	public sealed class CreateWeaponTemplateHandler : BaseHandler<CreateWeaponTemplateCommand, Guid>
	{
		public CreateWeaponTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<Guid> Handle(CreateWeaponTemplateCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.ItemTemplates)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			if (game.ItemTemplates.Exists(x => x.Name == request.Name))
				throw new RequestNameNotUniqException<ItemTemplate>(request.Name);

			var newWeaponTemplate = WeaponTemplate.CreateWeaponTemplate(
				game: game,
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

			_appDbContext.ItemTemplates.Add(newWeaponTemplate);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return newWeaponTemplate.Id;
		}
	}
}
