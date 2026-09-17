using Wastelands.Service.Domain.Enums;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Contracts
{
	public interface IBattleService
	{
		Task<BattleDto> GetBattleByIdAsync(long id);

		Task<List<BattleDto>> GetBattlesAsync(BattleFilter filter);

		Task<BattleDto> CreateBattleAsync(CreateBattleRequest request);

		Task DeleteBattleAsync(long id);

		Task<BattleDto> AddCreatureAsync(AddCreatureToBattleRequest request);

		Task<BattleDto> UpdateCreatureAsync(UpdateBattleCreatureRequest request);

		Task<BattleDto> RemoveCreatureAsync(long battleId, long creatureId);

		Task<BattleDto> AddCreatureConditionAsync(long battleId, long creatureId, Condition condition);

		Task<BattleDto> RemoveCreatureConditionAsync(long battleId, long creatureId, Condition condition);

		Task<BattleDto> AddCharacterAsync(AddCharacterToBattleRequest request);

		Task<BattleDto> RemoveCharacterAsync(long battleId, long characterId);

		Task<BattleDto> AddCharacterConditionAsync(long battleId, long characterId, Condition condition);

		Task<BattleDto> RemoveCharacterConditionAsync(long battleId, long characterId, Condition condition);

		Task<BattleDto> StartBattleAsync(long battleId);

		Task<BattleDto> StartAttackAsync(StartAttackRequest request);

		Task<BattleDto> SetAttackerChoicesAsync(SetAttackerChoicesRequest request);

		Task<BattleDto> ConfirmAttackerAsync(long battleId);

		Task<BattleDto> SetDefenderChoiceAsync(SetDefenderChoiceRequest request);

		Task<BattleDto> ConfirmDefenderAsync(long battleId);

		Task<BattleDto> SetDamageRollAsync(SetDamageRollRequest request);

		Task<BattleDto> ContinueDamageAsync(long battleId);

		Task<BattleDto> NextSwingAsync(NextSwingRequest request);

		Task<BattleDto> EndActivationAsync(long battleId);

		Task<BattleDto> SkipTurnAsync(long battleId);
	}
}
