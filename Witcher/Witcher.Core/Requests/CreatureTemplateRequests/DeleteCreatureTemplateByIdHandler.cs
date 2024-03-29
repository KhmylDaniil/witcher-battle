﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions;
using Witcher.Core.Exceptions.EntityExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.CreatureTemplateRequests
{
	/// <summary>
	/// Обработчик удаления шаблона существа по айди
	/// </summary>
	public class DeleteCreatureTemplateByIdHandler : BaseHandler<DeleteCreatureTemplateByIdCommand, Unit>
	{
		public DeleteCreatureTemplateByIdHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService)
		{
		}

		/// <summary>
		/// Удаление шаблона существа по айди
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public override async Task<Unit> Handle(DeleteCreatureTemplateByIdCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(x => x.CreatureTemplates.Where(x => x.Id == request.Id))
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			var creatureTemplate = game.CreatureTemplates.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<CreatureTemplate>(request.Id);

			game.CreatureTemplates.Remove(creatureTemplate);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
