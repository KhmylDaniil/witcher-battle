using Wastelands.Core.Contracts.Models;
using Wastelands.Core.EfDataAccess.Contracts;
using Wastelands.Core.EfDataAccess.Models;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Contracts.Repositories
{
	public interface IBattleMapRepository : IBaseRepository<BattleMap>
	{
		/// <summary>
		/// Как <see cref="IBaseReadRepository{TEntity}.GetPagedAsync"/>, но без подгрузки гексов — у каждой
		/// карты их до MaxDimension² штук, а списку карт они не нужны.
		/// </summary>
		Task<PagedList<BattleMap>> GetPagedWithoutHexesAsync(PagedRequest request, IFilter<BattleMap>? filter);

		/// <summary>
		/// Сохраняет изменения карты, загруженной через GetByIdAsync, по change tracking'у. В отличие от
		/// UpdateAsync (помечает Modified весь граф) — UPDATE уходит только для реально перекрашенных гексов,
		/// а не для всех до MaxDimension² штук.
		/// </summary>
		Task SaveTrackedChangesAsync();

		/// <summary>
		/// Без скоупинга по владельцу игры — карта идущего боя видна и игрокам, чьи персонажи в нём
		/// участвуют. Доступ к самому бою проверяет вызывающий сервис (через скоуп BattleRepository).
		/// </summary>
		Task<BattleMap?> GetByIdUnscopedAsync(long id);
	}
}
