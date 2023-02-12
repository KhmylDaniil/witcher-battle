using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.AbilityRequests.DeleteAbilitybyId;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.AbilityRequests.DeleteAbilityById
{
	/// <summary>
	/// Обработчик удаления способности
	/// </summary>
	public class DeleteAbilityByIdHandler : BaseHandler, IRequestHandler<DeleteAbilityByIdCommand>
	{
		public DeleteAbilityByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService) { }

		/// <summary>
		/// Удаление способности
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public async Task<Unit> Handle(DeleteAbilityByIdCommand request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ExceptionRequestNull<DeleteAbilityByIdCommand>();

			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
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
