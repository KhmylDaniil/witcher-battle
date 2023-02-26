using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.GameRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.GameRequests
{
	/// <summary>
	/// Обработчик команды изменения игры
	/// </summary>
	public class ChangeGameHandler : BaseHandler<ChangeGameCommand, Unit>
	{
		/// <summary>
		/// Сервис изменения списков графических и текстовых файлов
		/// </summary>
		private readonly IChangeListService _changeListService;

		/// <summary>
		/// Конструктор обработчика команды изменения игры
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		/// <param name="changeListService">Сервис изменения списков графических и текстовых файлов</param>
		public ChangeGameHandler(
			IAppDbContext appDbContext,
			IAuthorizationService authorizationService,
			IChangeListService changeListService)
			: base(appDbContext, authorizationService)
		{
			_changeListService = changeListService;
		}

		/// <summary>
		/// Обработка команды создания игры
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns></returns>
		public override async Task<Unit> Handle(ChangeGameCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService
				.AuthorizedGameFilter(_appDbContext.Games)
				.Include(x => x.Avatar)
				.Include(x => x.ImgFiles)
				.Include(x => x.TextFiles)
				.Include(x => x.UserGames)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionNoAccessToEntity<Game>();

			if (!string.Equals(request.Name, game.Name, System.StringComparison.Ordinal) && await _appDbContext.Games
				.AnyAsync(x => x.Name == request.Name, cancellationToken))
				throw new RequestNameNotUniqException<ChangeGameCommand>(nameof(request.Name));

			var avatar = request.AvatarId == null
				? null
				: await _appDbContext.ImgFiles
				.FirstOrDefaultAsync(x => x.Id == request.AvatarId, cancellationToken)
				?? throw new ExceptionEntityNotFound<ImgFile>(nameof(request.AvatarId));

			var textFiles = new List<TextFile>();

			if (request.TextFiles != null)
			{
				foreach (var x in request.TextFiles)
				{
					var textFile = await _appDbContext.TextFiles
					.FirstOrDefaultAsync(y => y.Id == x, cancellationToken)
					?? throw new ExceptionEntityNotFound<TextFile>(nameof(x));
					textFiles.Add(textFile);
				}
			}

			var imgFiles = new List<ImgFile>();

			if (request.ImgFiles != null)
			{
				foreach (var x in request.ImgFiles)
				{
					var imgFile = await _appDbContext.ImgFiles
					.FirstOrDefaultAsync(y => y.Id == x, cancellationToken)
					?? throw new ExceptionEntityNotFound<ImgFile>(nameof(x));
					imgFiles.Add(imgFile);
				}
			}

			game.ChangeGame(
				name: request.Name,
				avatar: avatar,
				description: request.Description);

			_changeListService.ChangeTextFilesList(game, textFiles);
			_changeListService.ChangeImgFilesList(game, imgFiles);
			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
