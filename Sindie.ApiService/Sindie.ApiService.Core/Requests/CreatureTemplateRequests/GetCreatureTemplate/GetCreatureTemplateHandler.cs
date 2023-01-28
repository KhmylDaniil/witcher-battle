using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.GetCreatureTemplate;
using Sindie.ApiService.Core.ExtensionMethods;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.CreatureTemplateRequests.GetCreatureTemplate
{
	/// <summary>
	/// Обработчик команды получения списка шаблонов существа
	/// </summary>
	public class GetCreatureTemplateHandler : IRequestHandler<GetCreatureTemplateCommand, GetCreatureTemplateResponse>
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
		/// Конструктор обработчика команды получения списка шаблонов существа
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		public GetCreatureTemplateHandler(
			IAppDbContext appDbContext,
			IAuthorizationService authorizationService,
			IDateTimeProvider dateTimeProvider)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
			_dateTimeProvider = dateTimeProvider;
		}

		/// <summary>
		/// Обработчик команды запроса списка шаблонов существа
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Ответ на запрос списка шаблонов существа</returns>
		public async Task<GetCreatureTemplateResponse> Handle(GetCreatureTemplateCommand request, CancellationToken cancellationToken)
		{
			CheckRequest(request);

			var filter = _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(g => g.CreatureTemplates)
					.ThenInclude(ct => ct.BodyTemplate)
				.Include(g => g.CreatureTemplates)
					.ThenInclude(ct => ct.CreatureTemplateParts)
				.Include(g => g.CreatureTemplates)
					.ThenInclude(ct => ct.Abilities)
						.ThenInclude(a => a.AppliedConditions)
							.ThenInclude(ap => ap.Condition)
				.SelectMany(x => x.CreatureTemplates
					.Where(x => request.UserName == null || x.Game.UserGames
						.Any(u => u.User.Name.Contains(request.UserName) && u.UserId == x.CreatedByUserId))
					.Where(x => request.Name == null || x.Name.Contains(request.Name))
					.Where(x => request.CreatureType == null || Enum.GetName(x.CreatureType).Contains(request.CreatureType))
					.Where(x => request.BodyTemplateName == null || x.BodyTemplate.Name.Contains(request.BodyTemplateName))
					.Where(x => request.BodyPartType == null || x.CreatureTemplateParts.Any(x => Enum.GetName(x.BodyPartType).Contains(request.BodyPartType)))
					.Where(x => x.CreatedOn >= request.CreationMinTime)
					.Where(x => (request.CreationMaxTime == default && x.CreatedOn <= _dateTimeProvider.TimeProvider)
					|| x.CreatedOn <= request.CreationMaxTime)
					.Where(x => x.ModifiedOn >= request.ModificationMinTime)
					.Where(x => (request.ModificationMaxTime == default && x.ModifiedOn <= _dateTimeProvider.TimeProvider)
					|| x.ModifiedOn <= request.ModificationMaxTime)
					.Where(ct => request.ConditionName == null || ct.Abilities
						.Any(a => a.AppliedConditions.Any(ac => ac.Condition.Name.Contains(request.ConditionName)))));

			var list = await filter
				.OrderBy(request.OrderBy, request.IsAscending)
				.Skip(request.PageSize * (request.PageNumber - 1))
				.Take(request.PageSize)
				.Select(x => new GetCreatureTemplateResponseItem()
				{
					Name = x.Name,
					Description = x.Description,
					Id = x.Id,
					CreatureType = x.CreatureType,
					BodyTemplateName = x.BodyTemplate.Name,
					CreatedByUserId = x.CreatedByUserId,
					ModifiedByUserId = x.ModifiedByUserId,
					CreatedOn = x.CreatedOn,
					ModifiedOn = x.ModifiedOn
				}).ToListAsync(cancellationToken);

			return new GetCreatureTemplateResponse { CreatureTemplatesList = list, TotalCount = list.Count };
		}

		private void CheckRequest(GetCreatureTemplateCommand request)
		{
			if (request.PageSize < 1)
				throw new ArgumentOutOfRangeException(nameof(GetCreatureTemplateCommand.PageSize));
			if (request.PageNumber < 1)
				throw new ArgumentOutOfRangeException(nameof(GetCreatureTemplateCommand.PageNumber));

			if (request.CreationMinTime > _dateTimeProvider.TimeProvider)
				throw new ArgumentOutOfRangeException(nameof(GetCreatureTemplateCommand.CreationMinTime));
			if (request.ModificationMinTime > _dateTimeProvider.TimeProvider)
				throw new ArgumentOutOfRangeException(nameof(GetCreatureTemplateCommand.ModificationMinTime));

			if (request.CreationMaxTime != default && request.CreationMinTime >= request.CreationMaxTime)
				throw new ArgumentOutOfRangeException(nameof(GetCreatureTemplateCommand.CreationMaxTime));
			if (request.ModificationMaxTime != default && request.ModificationMinTime >= request.ModificationMaxTime)
				throw new ArgumentOutOfRangeException(nameof(GetCreatureTemplateCommand.ModificationMaxTime));
		}
	}
}
