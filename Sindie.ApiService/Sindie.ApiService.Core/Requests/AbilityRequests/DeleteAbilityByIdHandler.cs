using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.AbilityRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.AbilityRequests
{
	/// <summary>
	/// Обработчик удаления способности
	/// </summary>
	public class DeleteAbilityByIdHandler : BaseHandler<DeleteAbilityByIdCommand, Unit>
	{
		public DeleteAbilityByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService) { }

		/// <summary>
		/// Удаление способности
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public override async Task<Unit> Handle(DeleteAbilityByIdCommand request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new RequestNullException<DeleteAbilityByIdCommand>();

			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(x => x.Abilities.Where(x => x.Id == request.Id))
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			var ability = game.Abilities.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<Ability>(request.Id);

			game.Abilities.Remove(ability);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
