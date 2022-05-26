using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.CharacterTemplateRequests.GetCharacterTemplate;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using Sindie.ApiService.Core.ExtensionMethods;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.CharacterTemplateRequests.GetCharacterTemplate
{
	/// <summary>
	/// Обработчик получения списка шаблонов персонажа
	/// </summary>
	public class GetCharacterTemplateHandler : IRequestHandler<GetCharacterTemplateQuery, GetCharacterTemplateQueryResponse>
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Сервис авторизации
		/// </summary>
		private readonly IAuthorizationService _authorizationService;

		/// <summary>
		/// Провайдер времени
		/// </summary>
		private readonly IDateTimeProvider _dateTimeProvider;

		/// <summary>
		/// Конструктор обработчика запроса получения списка шаблонов персонажа
		/// </summary>
		/// <param name="appDbContext"></param>
		/// <param name="authorizationService"></param>
		/// <param name="dateTimeProvider">Провайдер базы данных</param>
		public GetCharacterTemplateHandler(
			IAppDbContext appDbContext,
			IAuthorizationService authorizationService,
			IDateTimeProvider dateTimeProvider)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
			_dateTimeProvider = dateTimeProvider;
		}

		/// <summary>
		/// Обработчик запроса получения списка шаблонов персонажа
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Список шаблонов персонажа</returns>

		public async Task<GetCharacterTemplateQueryResponse> Handle(GetCharacterTemplateQuery request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ExceptionRequestNull<GetCharacterTemplateQuery>();
			if (request.PageSize < 1)
				throw new ArgumentOutOfRangeException(nameof(GetCharacterTemplateQuery.PageSize));
			if (request.PageNumber < 1)
				throw new ArgumentOutOfRangeException(nameof(GetCharacterTemplateQuery.PageNumber));

			if (request.CreationMinTime > _dateTimeProvider.TimeProvider)
				throw new ArgumentOutOfRangeException(nameof(GetCharacterTemplateQuery.CreationMinTime));
			if (request.ModificationMinTime > _dateTimeProvider.TimeProvider)
				throw new ArgumentOutOfRangeException(nameof(GetCharacterTemplateQuery.ModificationMinTime));

			if (request.CreationMaxTime != default && request.CreationMinTime >= request.CreationMaxTime)
				throw new ArgumentOutOfRangeException(nameof(GetCharacterTemplateQuery.CreationMaxTime));
			if (request.ModificationMaxTime != default && request.ModificationMinTime >= request.ModificationMaxTime)
				throw new ArgumentOutOfRangeException(nameof(GetCharacterTemplateQuery.ModificationMaxTime));

			var filter = _authorizationService
					.RoleGameFilter(_appDbContext.Games, request.GameId, GameRoles.MasterRoleId)
					.Include(x => x.CharacterTemplates)
						.ThenInclude(y => y.Characters)
					.SelectMany(x => x.CharacterTemplates)
						.Where(x => request.Name == null || x.Name.Contains(request.Name))
						.Where(x => x.CreatedOn >= request.CreationMinTime)
						.Where(x => (request.CreationMaxTime == default && x.CreatedOn <= _dateTimeProvider.TimeProvider)
						|| x.CreatedOn <= request.CreationMaxTime)
						.Where(x => x.ModifiedOn >= request.ModificationMinTime)
						.Where(x => (request.ModificationMaxTime == default && x.ModifiedOn <= _dateTimeProvider.TimeProvider)
						|| x.ModifiedOn <= request.ModificationMaxTime)
						.Where(x => request.AuthorName == null || x.Game.UserGames
							.Any(u => u.User.Name.Contains(request.AuthorName) && u.UserId == x.CreatedByUserId))
						.Where(x => request.CharacterName == null || x.Characters.Any(
							c => c.Name.Contains(request.CharacterName)));

			var list = await filter
					.OrderBy(request.OrderBy, request.IsAscending)
					.Skip(request.PageSize * (request.PageNumber - 1))
					.Take(request.PageSize)
					.Select(x => new GetCharacterTemplateQueryResponseItem()
					{
						Name = x.Name,
						Description = x.Description,
						Id = x.Id,
						GameId = x.GameId,
						ImgFileId = x.ImgFileId ?? default,
						InterfaceId = x.InterfaceId ?? default,
						CreatedByUserId = x.CreatedByUserId,
						ModifiedByUserId = x.ModifiedByUserId,
						CreatedOn = x.CreatedOn,
						ModifiedOn = x.ModifiedOn
					})
					.ToListAsync(cancellationToken);

			return new GetCharacterTemplateQueryResponse
			{ CharacterTemplatesList = list, TotalCount = list.Count };
		}
	}
}
