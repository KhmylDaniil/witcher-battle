using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BattleRequests
{
	/// <summary>
	/// Обработчик создания инстанса
	/// </summary>
	public class ChangeBattleHandler : BaseHandler<ChangeBattleCommand, Unit>
	{
		public ChangeBattleHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		public override async Task<Unit> Handle(ChangeBattleCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(x => x.Battles)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			if (game.Battles.Any(x => x.Name == request.Name && x.Id != request.Id))
				throw new RequestNameNotUniqException<ChangeBattleCommand>(nameof(request.Name));

			var imgFile = request.ImgFileId == null
				? null
				: await _appDbContext.ImgFiles.FirstOrDefaultAsync(x => x.Id == request.ImgFileId, cancellationToken)
				?? throw new ExceptionEntityNotFound<ImgFile>(request.ImgFileId.Value);

			var battle = game.Battles.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<Battle>(request.Id);

			battle.ChangeBattle(request.Name, request.Description, imgFile);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}
	}
}
