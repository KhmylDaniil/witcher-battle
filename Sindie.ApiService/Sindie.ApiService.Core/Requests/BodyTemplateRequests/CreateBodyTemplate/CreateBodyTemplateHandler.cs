using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.CreateBodyTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BodyTemplateRequests.CreateBodyTemplate
{
	/// <summary>
	/// Обработчик создания шаблона тела
	/// </summary>
	public class CreateBodyTemplateHandler : BaseHandler<CreateBodyTemplateCommand, BodyTemplate>
	{
		public CreateBodyTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		/// <summary>
		/// Создание шаблона тела
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public override async Task<BodyTemplate> Handle(CreateBodyTemplateCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(x => x.BodyTemplates)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			if (game.BodyTemplates.Any(x => x.Name == request.Name))
				throw new RequestNameNotUniqException<CreateBodyTemplateCommand>(nameof(request.Name));

			var bodyTemplateParts = request.BodyTemplateParts == null
				? Drafts.BodyTemplateDrafts.CreateBodyTemplatePartsDraft.CreateBodyPartsDraft()
				: request.BodyTemplateParts.Select(x => (IBodyTemplatePartData)x);

			var newBodyTemplate = new BodyTemplate(
				game: game,
				name: request.Name,
				description: request.Description,
				bodyTemplateParts: bodyTemplateParts);

			_appDbContext.BodyTemplates.Add(newBodyTemplate);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return newBodyTemplate;
		}
	}
}
