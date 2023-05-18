using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.ItemRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.ItemRequests
{
	public class EquipWeaponHandler : BaseHandler<EquipWeaponCommand, Unit>
	{
		public EquipWeaponHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public async override Task<Unit> Handle(EquipWeaponCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.CharacterOwnerFilter(_appDbContext.Games, request.CharacterId)
				.Include(g => g.Characters.Where(c => c.Id == request.CharacterId))
					.ThenInclude(c => c.Items.Where(i => i.Id == request.WeaponId))
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			var character = game.Characters.FirstOrDefault(c => c.Id == request.CharacterId)
				?? throw new EntityNotFoundException<Character>(request.CharacterId);

			var weapon = character.Items.FirstOrDefault(x => x.Id == request.WeaponId) as Weapon
				?? throw new EntityNotFoundException<ItemTemplate>(request.WeaponId);

			character.EquipWeapon(weapon);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
