using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BodyTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions;
using Witcher.Core.Exceptions.RequestExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.BodyTemplateRequests
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
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
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
