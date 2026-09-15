using Wastelands.Service.Domain.Models.Dto;

namespace Wastelands.Service.Domain.Contracts
{
	public interface IGameJoinRequestService
	{
		Task<GameJoinRequestDto> CreateJoinRequestAsync(long gameId);

		Task<List<GameJoinRequestDto>> GetMyJoinRequestsAsync();

		Task<List<GameJoinRequestDto>> GetIncomingJoinRequestsAsync(long gameId);

		Task AcceptJoinRequestAsync(long requestId);

		Task DeclineJoinRequestAsync(long requestId);
	}
}
