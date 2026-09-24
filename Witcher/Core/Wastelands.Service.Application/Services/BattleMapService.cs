using AutoMapper;
using Wastelands.Core.Contracts.Enums;
using Wastelands.Core.Contracts.Exceptions.BusinessLogicExceptions;
using Wastelands.Core.Contracts.Models;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Contracts.Repositories;
using Wastelands.Service.Application.Models.Dto;
using Wastelands.Service.Application.Models.Filters;
using Wastelands.Service.Application.Models.Requests;
using Wastelands.Service.Domain.Drafts;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Services
{
	public class BattleMapService : IBattleMapService
	{
		private readonly IBattleMapRepository _battleMapRepository;
		private readonly IGameAccessGuard _gameAccessGuard;
		private readonly IMapper _mapper;

		public BattleMapService(
			IBattleMapRepository battleMapRepository,
			IGameAccessGuard gameAccessGuard,
			IMapper mapper)
		{
			_battleMapRepository = battleMapRepository;
			_gameAccessGuard = gameAccessGuard;
			_mapper = mapper;
		}

		public async Task<BattleMapDto> GetBattleMapByIdAsync(long id)
		{
			var battleMap = await GetByIdAsync(id);
			return ToDto(battleMap);
		}

		public async Task<PagedResultDto<BattleMapSummaryDto>> GetBattleMapsAsync(BattleMapFilter filter, PagedRequest paging)
		{
			var paged = await _battleMapRepository.GetPagedWithoutHexesAsync(paging, filter);
			var dtos = _mapper.Map<List<BattleMapSummaryDto>>(paged.Entities);

			return new PagedResultDto<BattleMapSummaryDto> { Items = dtos, TotalCount = paged.TotalCount, PageNumber = paging.PageNumber, PageSize = paging.PageSize };
		}

		public async Task<BattleMapDto> CreateBattleMapAsync(CreateBattleMapRequest request)
		{
			var game = await _gameAccessGuard.GetGameOwnedByCurrentUserAsync(request.GameId);

			var entity = new BattleMap(game.Id, request.Name, request.Description, request.Columns, request.Rows, request.TerrainStyle);
			await _battleMapRepository.CreateAsync(entity);

			return ToDto(entity);
		}

		public async Task<BattleMapDto> UpdateBattleMapAsync(UpdateBattleMapRequest request)
		{
			var battleMap = await GetByIdAsync(request.Id);
			battleMap.UpdateBattleMap(request.Name, request.Description);
			battleMap.Resize(request.Columns, request.Rows, request.FillTerrainStyle);

			await _battleMapRepository.SaveTrackedChangesAsync();

			return ToDto(battleMap);
		}

		public async Task<BattleMapDto> PaintHexesAsync(PaintBattleMapHexesRequest request)
		{
			var battleMap = await GetByIdAsync(request.BattleMapId);
			battleMap.PaintHexes(request.Hexes
				.Select(h => new BattleMapHexPaintDraft(h.Column, h.Row, h.TerrainType, h.TerrainStyle))
				.ToList());

			await _battleMapRepository.SaveTrackedChangesAsync();

			return ToDto(battleMap);
		}

		public async Task DeleteBattleMapAsync(long id)
		{
			var battleMap = await GetByIdAsync(id);
			await _battleMapRepository.DeleteAsync(battleMap);
		}

		// Порядок гексов в БД не гарантирован (а после Resize новые гексы ещё и дописаны в конец списка) —
		// отдаём их построчно, чтобы клиенту не приходилось сортировать.
		private BattleMapDto ToDto(BattleMap battleMap)
		{
			var dto = _mapper.Map<BattleMapDto>(battleMap);
			dto.Hexes = dto.Hexes.OrderBy(h => h.Row).ThenBy(h => h.Column).ToList();

			return dto;
		}

		private async Task<BattleMap> GetByIdAsync(long id)
		{
			var battleMap = await _battleMapRepository.GetByIdAsync(id);

			NotFoundException.ThrowIfNull(
				battleMap,
				ErrorCode.BattleMapNotFound,
				nameof(BattleMap),
				nameof(BattleMap.Id),
				id.ToString());

			return battleMap;
		}
	}
}
