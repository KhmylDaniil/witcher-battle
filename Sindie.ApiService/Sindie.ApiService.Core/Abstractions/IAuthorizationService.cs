using Sindie.ApiService.Core.Entities;
using System;
using System.Linq;

namespace Sindie.ApiService.Core.Abstractions
{
	/// <summary>
	/// Сервис проверки прав доступа
	/// </summary>
	public interface IAuthorizationService
	{
		/// <summary>
		/// Проверить права доступа для роли в игре
		/// </summary>
		/// <param name="query">Запрос</param>
		/// <returns>Отфильтрованный запрос</returns>
		public IQueryable<Game> AuthorizedGameFilter(
			IQueryable<Game> query, Guid gameRoleId = default);

		/// <summary>
		/// Проверить права доступа для пользователя игры
		/// </summary>
		/// <param name="query">Запрос</param>
		/// <returns>Отфильтрованный запрос</returns>
		public IQueryable<Game> UserGameFilter(
			IQueryable<Game> query);

		/// <summary>
		/// Проверить права мастера боя</summary>
		/// <param name="query">Запрос</param>
		/// <param name="instanceId">Айди инстанса</param>
		/// <returns>Отфильтрованный запрос</returns>
		public IQueryable<Battle> BattleMasterFilter(
			IQueryable<Battle> query, Guid battleId);
	}
}
