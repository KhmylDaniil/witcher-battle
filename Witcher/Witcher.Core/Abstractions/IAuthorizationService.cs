using Witcher.Core.Entities;
using System;
using System.Linq;
using Witcher.Core.BaseData;
using Witcher.Core.Exceptions.SystemExceptions;
using Witcher.Core.Services.Authorization;

namespace Witcher.Core.Abstractions
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
		/// <param name="battleId">Айди боя</param>
		/// <returns>Отфильтрованный запрос</returns>
		public IQueryable<Battle> BattleMasterFilter(
			IQueryable<Battle> query, Guid battleId);

		/// <summary>
		/// Проверить права создателя персонажа/главмастера
		/// </summary>
		/// <param name="query">Запрос</param>
		/// <returns></returns>
		public IQueryable<Character> CharacterOwnerFilter(
			IQueryable<Game> query);
	}
}
