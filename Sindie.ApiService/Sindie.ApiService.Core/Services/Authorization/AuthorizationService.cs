using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Entities;
using System;
using System.Linq;

namespace Sindie.ApiService.Core.Services.Authorization
{
	/// <inheritdoc/>
	public class AuthorizationService : IAuthorizationService
	{
		private readonly IUserContext _userContext;
		private readonly IGameIdService _gameIdService;


		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="userContext">Контекст пользователя</param>
		public AuthorizationService(IUserContext userContext, IGameIdService gameIdService)
		{
			_userContext = userContext;
			_gameIdService = gameIdService;
		}

		/// <inheritdoc/>
		public IQueryable<Game> AuthorizedGameFilter(
			IQueryable<Game> query, Guid gameRoleId = default)
		{
			if (query is null)
				throw new ArgumentNullException(nameof(query));

			gameRoleId = gameRoleId == Guid.Empty ? GameRoles.MasterRoleId : gameRoleId;

			if (string.Equals(_userContext.Role, SystemRoles.AdminRoleName, StringComparison.OrdinalIgnoreCase))
				return query;

			return query.Where(x => x.Id == _gameIdService.GameId
				&& x.UserGames.Any(y => y.UserId == _userContext.CurrentUserId
					&& y.GameRoleId == gameRoleId));
		}

		/// <inheritdoc/>
		public IQueryable<Game> UserGameFilter(
			IQueryable<Game> query, Guid gameId)
		{
			if (query is null)
				throw new ArgumentNullException(nameof(query));

			if (string.Equals(_userContext.Role, SystemRoles.AdminRoleName, StringComparison.OrdinalIgnoreCase))
				return query;

			return query.Where(x => x.Id == gameId
				&& x.UserGames.Any(y => y.UserId == _userContext.CurrentUserId));
		}

		/// <inheritdoc/>
		public IQueryable<Battle> BattleMasterFilter(
			IQueryable<Battle> query, Guid battleId)
		{
			if (query is null)
				throw new ArgumentNullException(nameof(query));

			if (string.Equals(_userContext.Role, SystemRoles.AdminRoleName, StringComparison.OrdinalIgnoreCase))
				return query;

			return query.Where(i => i.Id == battleId
			&& i.Game.UserGames.Any(u => u.UserId == _userContext.CurrentUserId && u.GameRoleId == GameRoles.MasterRoleId));
		}
	}
}
