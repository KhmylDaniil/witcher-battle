using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.GetBodyTemplateById;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BodyTemplateRequests.GetBodyTemplateById
{
	/// <summary>
	/// Запрос шаблона тела по айди
	/// </summary>
	public class GetBodyTemplateByIdHandler : IRequestHandler<GetBodyTemplateByIdQuery, GetBodyTemplateByIdResponse>
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
		/// Конструктор обработчика запроса шаблона тела по айди
		/// </summary>
		/// <param name="appDbContext"></param>
		/// <param name="authorizationService"></param>
		public GetBodyTemplateByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Запрос шаблона тела по айди
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public async Task<GetBodyTemplateByIdResponse> Handle(GetBodyTemplateByIdQuery request, CancellationToken cancellationToken)
		{
			var filter = _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, GameRoles.MasterRoleId)
				.Include(x => x.BodyTemplates.Where(x => x.Id == request.Id))
				.ThenInclude(x => x.BodyTemplateParts)
				.SelectMany(x => x.BodyTemplates);

			var bodyTemplate = filter.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<BodyTemplate>(request.Id);

			var bodyTemplateParts = bodyTemplate.BodyTemplateParts.Select(x => new GetBodyTemplateByIdResponseItem()
			{
				Name = x.Name,
				HitPenalty = x.HitPenalty,
				DamageModifier = x.DamageModifier,
				MinToHit = x.MinToHit,
				MaxToHit = x.MaxToHit
			}).ToList();

			return new GetBodyTemplateByIdResponse()
			{
				Name = bodyTemplate.Name,
				Description = bodyTemplate.Description,
				Id = bodyTemplate.Id,
				GameId = bodyTemplate.GameId,
				CreatedByUserId = bodyTemplate.CreatedByUserId,
				ModifiedByUserId = bodyTemplate.ModifiedByUserId,
				CreatedOn = bodyTemplate.CreatedOn,
				ModifiedOn = bodyTemplate.ModifiedOn,
				GetBodyTemplateByIdResponseItems = bodyTemplateParts
			};
		}
	}
}
