using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.DeleteBodyTemplateById;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BodyTemplateRequests.DeleteBodyTemplateById
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
			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(x => x.BodyTemplates.Where(x => x.Id == request.Id))
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			var bodyTemplate = game.BodyTemplates.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<BodyTemplate>(request.Id);

			game.BodyTemplates.Remove(bodyTemplate);
			await _appDbContext.SaveChangesAsync();
			return Unit.Value;
		}
	}
}
