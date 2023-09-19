using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.BattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.BattleRequests
{
	/// <summary>
	/// Обработчик создания боя
	/// </summary>
	public sealed class CreateBattleHandler : BaseHandler<CreateBattleCommand, Battle>
	{
		public CreateBattleHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		/// <summary>
		/// Обработчик создания боя
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Бой</returns>
		public override async Task<Battle> Handle(CreateBattleCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(x => x.Battles)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			var imgFile = request.ImgFileId == null
				? null
				: await _appDbContext.ImgFiles.FirstOrDefaultAsync(x => x.Id == request.ImgFileId, cancellationToken)
				?? throw new EntityNotFoundException<ImgFile>(request.ImgFileId.Value);

			if (game.Battles.Exists(x => x.Name == request.Name))
				throw new RequestNameNotUniqException<CreateBattleCommand>(nameof(request.Name));

			var newBattle = new Battle(
				game: game,
				imgFile: imgFile,
				name: request.Name,
				description: request.Description);

			game.Battles.Add(newBattle);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return newBattle;
		}
	}
}
