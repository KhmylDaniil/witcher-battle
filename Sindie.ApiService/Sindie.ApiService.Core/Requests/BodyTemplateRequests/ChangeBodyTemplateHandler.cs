using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BodyTemplateRequests
{
	public class ChangeBodyTemplateHandler : BaseHandler<ChangeBodyTemplateCommand, Unit>
	{
		public ChangeBodyTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		/// <summary>
		/// Обработка изменения шаблона тела
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public override async Task<Unit> Handle(ChangeBodyTemplateCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(x => x.BodyTemplates)
					.ThenInclude(x => x.BodyTemplateParts)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			if (game.BodyTemplates.Any(x => x.Name == request.Name && x.Id != request.Id))
				throw new RequestNameNotUniqException<ChangeBodyTemplateCommand>(nameof(request.Name));

			var bodyTemplate = game.BodyTemplates.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<BodyTemplate>(request.Id);

			bodyTemplate.ChangeBodyTemplate(
				game: game,
				name: request.Name,
				description: request.Description,
				bodyTemplateParts: request.BodyTemplateParts);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
