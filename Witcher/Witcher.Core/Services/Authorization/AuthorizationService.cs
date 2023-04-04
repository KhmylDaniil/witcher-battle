using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Entities;
using System;
using System.Linq;
using Witcher.Core.Exceptions.SystemExceptions;
using Witcher.Core.Services.Hasher;

namespace Witcher.Core.Services.Authorization
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
				throw new ApplicationSystemNullException<AuthorizationService>(nameof(query));

			gameRoleId = gameRoleId == Guid.Empty ? GameRoles.MasterRoleId : gameRoleId;

			if (string.Equals(_userContext.Role, SystemRoles.AdminRoleName, StringComparison.OrdinalIgnoreCase))
				return query;

			return query.Where(x => x.Id == _gameIdService.GameId
				&& x.UserGames.Any(y => y.UserId == _userContext.CurrentUserId
					&& (y.GameRoleId == gameRoleId || y.GameRoleId == GameRoles.MainMasterRoleId)));
		}

		/// <inheritdoc/>
		public IQueryable<Game> UserGameFilter(
			IQueryable<Game> query)
		{
			if (query is null)
				throw new ApplicationSystemNullException<AuthorizationService>(nameof(query));

			if (string.Equals(_userContext.Role, SystemRoles.AdminRoleName, StringComparison.OrdinalIgnoreCase))
				return query;

			return query.Where(x => x.Id == _gameIdService.GameId
				&& x.UserGames.Any(y => y.UserId == _userContext.CurrentUserId));
		}

		/// <inheritdoc/>
		public IQueryable<Character> CharacterOwnerFilter(
			IQueryable<Game> query)
		{
			if (query is null)
				throw new ApplicationSystemNullException<AuthorizationService>(nameof(query));

			if (string.Equals(_userContext.Role, SystemRoles.AdminRoleName, StringComparison.OrdinalIgnoreCase))
				return query.Where(x => x.Id == _gameIdService.GameId).SelectMany(g => g.Characters);

			return query.Where(x => x.Id == _gameIdService.GameId)
				.SelectMany(g => g.Characters.Where(c => c.UserGameCharacters
				.Any(y => y.UserGame.UserId == _userContext.CurrentUserId)));
		}

		/// <inheritdoc/>
		public IQueryable<Battle> BattleMasterFilter(
			IQueryable<Battle> query, Guid battleId)
		{
			if (query is null)
				throw new ApplicationSystemNullException<AuthorizationService>(nameof(query));

			if (string.Equals(_userContext.Role, SystemRoles.AdminRoleName, StringComparison.OrdinalIgnoreCase))
				return query;

			return query.Where(i => i.Id == battleId
			&& i.Game.UserGames.Any(u => u.UserId == _userContext.CurrentUserId
			&& (u.GameRoleId == GameRoles.MasterRoleId || u.GameRoleId == GameRoles.MainMasterRoleId)));
		}
	}
}
