using Microsoft.EntityFrameworkCore;
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

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="userContext">Контекст пользователя</param>
		public AuthorizationService(IUserContext userContext)
		{
			_userContext = userContext;
		}

		/// <inheritdoc/>
		public IQueryable<Game> RoleGameFilter(
			IQueryable<Game> query, Guid gameId, Guid gameRoleId)
		{
			if (query is null)
				throw new ArgumentNullException(nameof(query));

			if (_userContext.Role == SystemRoles.AndminRoleName)
				return query;

			return query.Where(x => x.Id == gameId
				&& x.UserGames.Any(y => y.UserId == _userContext.CurrentUserId
					&& y.GameRoleId == gameRoleId));
		}

		/// <inheritdoc/>
		public IQueryable<Game> UserGameFilter(
			IQueryable<Game> query, Guid gameId)
		{
			if (query is null)
				throw new ArgumentNullException(nameof(query));

			if (_userContext.Role == SystemRoles.AndminRoleName)
				return query;

			return query.Where(x => x.Id == gameId
				&& x.UserGames.Any(y => y.UserId == _userContext.CurrentUserId));
		}

		/// <inheritdoc/>
		public IQueryable<Game> BagOwnerOrMasterFilter(
			IQueryable<Game> query, Guid gameId, Guid bagId)
		{
			if (query is null)
				throw new ArgumentNullException(nameof(query));

			if (_userContext.Role == SystemRoles.AndminRoleName)
				return query;

			return query.Where(x => x.Id == gameId
			&& (x.UserGames.Any(u => u.UserId == _userContext.CurrentUserId && u.GameRoleId == GameRoles.MasterRoleId)
				|| x.Instances.Any(i =>
					!i.Characters.Any(c => c.BagId == bagId)
					||
					i.Characters.Any(c => c.BagId == bagId && x.UserGames
						.Any(ug => ug.UserId == _userContext.CurrentUserId && ug.UserGameCharacters
							.Any(ugc => ugc.Id == c.UserGameActivatedId))))));
		}
	}
}
