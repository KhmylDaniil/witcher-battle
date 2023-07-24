using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Exceptions.RequestExceptions;
using System.Collections.Generic;
using System;
using Witcher.Core.ExtensionMethods;

namespace Witcher.Core.Requests.RunBattleRequests
{
	public sealed class MakeTurnHandler : BaseHandler<MakeTurnCommand, MakeTurnResponse>
	{
		public MakeTurnHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<MakeTurnResponse> Handle(MakeTurnCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.CharacterOwnerFilter(_appDbContext.Games, request.CreatureId)
				.GetCreaturesAndCharactersFormBattle(request.BattleId)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new NoAccessToEntityException<Game>();

			var battle = game.Battles.FirstOrDefault()
				?? throw new EntityNotFoundException<Battle>(request.BattleId);

			var currentCreature = battle.Creatures.Find(x => x.Id == request.CreatureId)
				?? throw new EntityNotFoundException<Creature>(request.CreatureId);

			var equippedWeapons = new Dictionary<Guid, string>();
			if (currentCreature is Character currentCharacter)
				equippedWeapons = currentCharacter.Items.Where(i => i.ItemType == BaseData.Enums.ItemType.Weapon && i.IsEquipped.Value)
					.Select(x => x.ItemTemplate).ToDictionary(x => x.Id, x => x.Name);

			return new MakeTurnResponse
			{
				BattleId = battle.Id,
				CreatureId = currentCreature.Id,
				CurrentCreatureName = currentCreature.Name,
				MultiAttackAbilityId = currentCreature.Turn.MuitiattackAttackFormulaId,
				PossibleTargets = battle.Creatures.ToDictionary(x => x.Id, x => x.Name),
				MyAbilities = currentCreature.Abilities.ToDictionary(x => x.Id, x => x.Name),
				EquippedWeapons = equippedWeapons,
				TurnState = currentCreature.Turn.TurnState
			};
		}
	}
}
