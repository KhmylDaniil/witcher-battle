using Wastelands.Core.EfDataAccess.Contracts;
using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Contracts.Repositories
{
	public interface ICreatureTemplateRepository : IBaseRepository<CreatureTemplate>
	{
		/// <summary>
		/// Без скоупинга по владельцу — нужен для боевых расчётов (часть тела/броня/статы/навыки
		/// защищающегося существа), которые может инициировать не только мастер игры, но и
		/// защищающийся игрок. Авторизацию делает вызывающий сервис.
		/// </summary>
		Task<CreatureTemplate?> GetByIdUnscopedAsync(long id);
	}
}
