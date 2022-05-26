using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.ModifierRequests.GetModifier;
using Sindie.ApiService.Core.ExtensionMethods;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sindie.ApiService.Core.Entities;

namespace Sindie.ApiService.Core.Requests.ModifierRequests.GetModifier
{
	/// <summary>
	/// Обработчик запроса получения модификатора
	/// </summary>
	public class GetModifierHandler : IRequestHandler<GetModifierQuery, GetModifierQueryResponse>
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Провайдер времени
		/// </summary>
		private readonly IDateTimeProvider _dateTimeProvider;

		/// <summary>
		/// Сервис авторизации
		/// </summary>
		private readonly IAuthorizationService _authorizationService;

		/// <summary>
		/// Конструктор обработчика получения модификатора
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="dateTimeProvider">Провайдер базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		public GetModifierHandler(
			IAppDbContext appDbContext,
			IDateTimeProvider dateTimeProvider,
			IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_dateTimeProvider = dateTimeProvider;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Обработка запроса получения модификатора
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Ответ на запрос получения списка модификаторов</returns>		
		public async Task<GetModifierQueryResponse> Handle(
			GetModifierQuery request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ExceptionRequestNull<GetModifierQuery>();
			if (request.PageSize < 1)
				throw new ArgumentOutOfRangeException(nameof(GetModifierQuery.PageSize));
			if (request.PageNumber < 1)
				throw new ArgumentOutOfRangeException(nameof(GetModifierQuery.PageNumber));

			if (request.CreationMinTime > _dateTimeProvider.TimeProvider)
				throw new ArgumentOutOfRangeException(nameof(GetModifierQuery.CreationMinTime));
			if (request.ModificationMinTime > _dateTimeProvider.TimeProvider)
				throw new ArgumentOutOfRangeException(nameof(GetModifierQuery.ModificationMinTime));

			if (request.CreationMaxTime != default && request.CreationMinTime >= request.CreationMaxTime)
				throw new ArgumentOutOfRangeException(nameof(GetModifierQuery.CreationMaxTime));
			if (request.ModificationMaxTime != default && request.ModificationMinTime >= request.ModificationMaxTime)
				throw new ArgumentOutOfRangeException(nameof(GetModifierQuery.ModificationMaxTime));

			var filteredList = _authorizationService
					.RoleGameFilter(_appDbContext.Games, request.GameId, GameRoles.MasterRoleId)
					.Include(x => x.Modifiers)
						.ThenInclude(y => y.ModifierScripts)
							.ThenInclude(y => y.Event)
					.SelectMany(x => x.Modifiers
						.Where(x => request.UserName == null || x.Game.UserGames
							.Any(u => u.User.Name.Contains(request.UserName) && u.UserId == x.CreatedByUserId))
						.Where(x => request.SearchText == null || x.Name.Contains(request.SearchText))
						.Where(x => x.CreatedOn >= request.CreationMinTime)
						.Where(x => (request.CreationMaxTime == default && x.CreatedOn <= _dateTimeProvider.TimeProvider)
						|| x.CreatedOn <= request.CreationMaxTime)
						.Where(x => x.ModifiedOn >= request.ModificationMinTime)
						.Where(x => (request.ModificationMaxTime == default && x.ModifiedOn <= _dateTimeProvider.TimeProvider)
						|| x.ModifiedOn <= request.ModificationMaxTime));
			
			var result = await ActiveFilter(filteredList, request.IsActive)
				.OrderBy(request.OrderBy, request.IsAscending)
				.Skip(request.PageSize * (request.PageNumber - 1))
				.Take(request.PageSize)
				.Select(x => new GetModifierQueryResponseItem()
				{
					Name = x.Name,
					Description = x.Description,
					Id = x.Id,
					GameId = x.GameId,
					ImgFileId = x.ImgFileId ?? default,
					CreatedByUserId = x.CreatedByUserId,
					ModifiedByUserId = x.ModifiedByUserId,
					CreatedOn = x.CreatedOn,
					ModifiedOn = x.ModifiedOn
				}).ToListAsync(cancellationToken);

			return new GetModifierQueryResponse
			{ ModifiersList = result, TotalCount = result.Count };
		}

		/// <summary>
		/// Фильтр активности скрипта модификатора
		/// </summary>
		/// <param name="query">Запрос</param>
		/// <param name="isActive">Активно?</param>
		/// <returns>Отфильтрованный запрос</returns>
		private IQueryable<Modifier> ActiveFilter(IQueryable<Modifier> query, bool isActive)
		{
			if (isActive)
				return query.Where(m => m.ModifierScripts.Any(
				x => (x.Event != null && x.Event.IsActive)
				|| x.ActiveCycles.Any(
				y => _dateTimeProvider.TimeProvider >= y.ActivationBeginning
				&& _dateTimeProvider.TimeProvider <= y.ActivationEnd)));
			else
				return query.Where(m =>
				m.ModifierScripts.Any(x => x.Event == null || 
				(x.Event.IsActive == false && !x.ActiveCycles.Any(
				y => _dateTimeProvider.TimeProvider >= y.ActivationBeginning
				&& _dateTimeProvider.TimeProvider <= y.ActivationEnd))));
		}
	}
}
