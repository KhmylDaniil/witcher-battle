using Wastelands.Core.Contracts.Contracts;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Domain.Entities;
using Wastelands.Service.Domain.Enums;

namespace Wastelands.Service.Application.Services
{
	public class BattleParticipantAuthorizer : IBattleParticipantAuthorizer
	{
		private readonly IGameAccessGuard _gameAccessGuard;
		private readonly IUserContext _userContext;

		public BattleParticipantAuthorizer(IGameAccessGuard gameAccessGuard, IUserContext userContext)
		{
			_gameAccessGuard = gameAccessGuard;
			_userContext = userContext;
		}

		public async Task EnsureControllerAsync(Battle battle, ParticipantKind kind, long participantId, ErrorCode errorCode)
		{
			if (kind == ParticipantKind.Creature)
			{
				try
				{
					await _gameAccessGuard.GetGameOwnedByCurrentUserAsync(battle.GameId);
				}
				catch (InvalidArgumentException)
				{
					throw new InvalidArgumentException(errorCode, "Существом в бою распоряжается только мастер игры.");
				}

				return;
			}

			var battleCharacter = BattleParticipants.GetBattleCharacter(battle, participantId);
			if (battleCharacter.Character.UserId != _userContext.CurrentUserId)
			{
				throw new InvalidArgumentException(errorCode, "Этим персонажем в бою распоряжается только контролирующий его игрок.");
			}
		}
	}
}
