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
		public IQueryable<Game> RoleGameFilter(
			IQueryable<Game> query, Guid gameId, Guid gameRoleId);

		/// <summary>
		/// Проверить права доступа для пользователя игры
		/// </summary>
		/// <param name="query">Запрос</param>
		/// <returns>Отфильтрованный запрос</returns>
		public IQueryable<Game> UserGameFilter(
			IQueryable<Game> query, Guid gameId);

		/// <summary>
		/// Проверить права на доступ к сумке (владелец или мастер)
		/// </summary>
		/// <param name="query">Запрос</param>
		/// <param name="gameId">Айди игры</param>
		/// <param name="bagId">Айди сумки</param>
		/// <returns>Отфильтрованный запрос</returns>
		public IQueryable<Game> BagOwnerOrMasterFilter(
				IQueryable<Game> query, Guid gameId, Guid bagId);

		/// <summary>
		/// Проверить права мастера на инстанс</summary>
		/// <param name="query">Запрос</param>
		/// <param name="instanceId">Айди инстанса</param>
		/// <returns>Отфильтрованный запрос</returns>
		public IQueryable<Instance> InstanceMasterFilter(
			IQueryable<Instance> query, Guid instanceId);
	}
}
