using Wastelands.Core.Contracts.Models;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;

namespace Wastelands.Service.Application.Contracts
{
	public interface IBattleMapService
	{
		Task<BattleMapDto> GetBattleMapByIdAsync(long id);

		Task<PagedResultDto<BattleMapSummaryDto>> GetBattleMapsAsync(BattleMapFilter filter, PagedRequest paging);

		Task<BattleMapDto> CreateBattleMapAsync(CreateBattleMapRequest request);

		Task<BattleMapDto> UpdateBattleMapAsync(UpdateBattleMapRequest request);

		Task<BattleMapDto> PaintHexesAsync(PaintBattleMapHexesRequest request);

		Task DeleteBattleMapAsync(long id);
	}
}
