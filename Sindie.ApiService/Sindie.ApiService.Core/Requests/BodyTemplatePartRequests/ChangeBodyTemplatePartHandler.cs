using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BodyTemplatePartsRequests;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.ChangeBodyTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using Sindie.ApiService.Core.Requests.BodyTemplateRequests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BodyTemplatePartRequests
{
	public class ChangeBodyTemplatePartHandler : IRequestHandler<ChangeBodyTemplatePartCommand, Unit>
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Сервис авторизации
		/// </summary>
		private readonly IAuthorizationService _authorizationService;

		public ChangeBodyTemplatePartHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		public async Task<Unit> Handle(ChangeBodyTemplatePartCommand request, CancellationToken cancellationToken)
		{
			var bodyTemplate = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(g => g.BodyTemplates.Where(bt => bt.Id == request.BodyTemplateId))
					.ThenInclude(bt => bt.BodyTemplateParts)
				.SelectMany(g => g.BodyTemplates)
				.FirstOrDefaultAsync(cancellationToken)
						?? throw new ExceptionNoAccessToEntity<Game>();

			CheckRequest(request, bodyTemplate);

			var correspondingBodyPart = bodyTemplate.BodyTemplateParts.First(x => x.Id == request.Id)
				?? throw new  ExceptionEntityNotFound<BodyTemplatePart>(request.Id);

			if (request.MaxToHit == correspondingBodyPart.MaxToHit && request.MinToHit == correspondingBodyPart.MinToHit)
				correspondingBodyPart.ChangeBodyTemplatePart(request);
			else
				bodyTemplate.CreateBodyTemplateParts(BodyTemplatePartsData.CreateBodyTemplatePartsData(bodyTemplate.BodyTemplateParts, request));

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		/// <summary>
		/// Проверка запроса
		/// </summary>
		/// <param name="request">Запрос</param>
		private void CheckRequest(ChangeBodyTemplatePartCommand request, BodyTemplate bodyTemplate)
		{
			if (request == null) throw new ArgumentNullException(nameof(request));

			if (bodyTemplate.BodyTemplateParts?.FirstOrDefault(x => x.Id == request.Id) == null)
				throw new ExceptionEntityNotIncluded<BodyTemplatePart>(nameof(bodyTemplate));

			if (!Enum.IsDefined(request.BodyPartType))
				throw new ExceptionRequestFieldIncorrectData<ChangeBodyTemplatePartCommand>(nameof(request.BodyPartType));

			if (request.MinToHit < 1)
				throw new ExceptionRequestFieldIncorrectData<ChangeBodyTemplatePartCommand>(nameof(request.MinToHit));

			if (request.MaxToHit > 10 || request.MaxToHit < request.MinToHit)
				throw new ExceptionRequestFieldIncorrectData<ChangeBodyTemplatePartCommand>(nameof(request.MaxToHit));
		}
	}
}
