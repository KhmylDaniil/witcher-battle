using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BodyTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.BodyTemplateRequests
{
	/// <summary>
	/// Обработчик удаления шаблона тела по айди
	/// </summary>
	public class DeleteBodyTemplateByIdHandler : BaseHandler<DeleteBodyTemplateByIdCommand, Unit>
	{
		public DeleteBodyTemplateByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		/// <summary>
		/// Удаление шаблона тела по айди
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public override async Task<Unit> Handle(DeleteBodyTemplateByIdCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(x => x.BodyTemplates.Where(x => x.Id == request.Id))
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			var bodyTemplate = game.BodyTemplates.FirstOrDefault(x => x.Id == request.Id)
				?? throw new EntityNotFoundException<BodyTemplate>(request.Id);

			game.BodyTemplates.Remove(bodyTemplate);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
