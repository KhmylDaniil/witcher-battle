using Wastelands.Service.Domain.Entities;

namespace Wastelands.Service.Application.Contracts
{
	/// <summary>
	/// Общая проверка "текущий пользователь — мастер (создатель) игры", нужная всем сервисам,
	/// которые создают/меняют принадлежащие игре объекты (шаблоны, бои, заявки на вступление).
	/// </summary>
	public interface IGameAccessGuard
	{
		/// <summary>Загружает игру по id и проверяет, что её создал текущий пользователь.</summary>
		Task<Game> GetGameOwnedByCurrentUserAsync(long gameId);

		/// <summary>Проверяет уже загруженную игру.</summary>
		void EnsureOwner(Game game);

		/// <summary>Является ли текущий пользователь мастером (создателем) игры — без исключения, для ветвления прав.</summary>
		Task<bool> IsOwnerAsync(long gameId);
	}
}
