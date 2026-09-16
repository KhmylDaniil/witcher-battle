using Wastelands.Core.EfDataAccess.Contracts;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Contracts.Repositories
{
	public interface ICharacterRepository : IBaseRepository<Character>
	{
		/// <summary>
		/// Без скоупинга по владельцу (в отличие от остальных методов этого репозитория) — нужен
		/// мастеру игры, чтобы видеть персонажей всех игроков (например, при добавлении в бой).
		/// Авторизацию ("я мастер этой игры") делает вызывающий сервис.
		/// </summary>
		Task<List<Character>> GetCharactersByGameIdAsync(long gameId);

		/// <summary>Без скоупинга по владельцу — см. <see cref="GetCharactersByGameIdAsync"/>.</summary>
		Task<Character?> GetByIdUnscopedAsync(long id);
	}
}
