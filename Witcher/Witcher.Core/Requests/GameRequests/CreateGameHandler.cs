using MediatR;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.BaseData;
using Witcher.Core.Contracts.GameRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.GameRequests
{
	/// <summary>
	/// Обработчик команды создания игры
	/// </summary>
	public class CreateGameHandler : BaseHandler<CreateGameCommand, Unit>
	{
		/// <summary>
		/// Интерфейс получения данных пользователя из веба
		/// </summary>
		private readonly IUserContext _userContext;

		/// <summary>
		/// Сервис изменения списков графических и текстовых файлов
		/// </summary>
		private readonly IChangeListService _changeListService;

		/// <summary>
		/// Конструктор обработчика команды создания игры
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="userContext">Получение контекста пользователя из веба</param>
		/// <param name="changeListService">Сервис изменения списков графических и текстовых файлов</param>
		public CreateGameHandler(
			IAppDbContext appDbContext,
			IAuthorizationService authorizationService,
			IUserContext userContext,
			IChangeListService changeListService)
			: base(appDbContext, authorizationService)
		{
			_userContext = userContext;
			_changeListService = changeListService;
		}

		/// <summary>
		/// Обработка команды создания игры
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns></returns>
		public override async Task<Unit> Handle(CreateGameCommand request, CancellationToken cancellationToken)
		{

			if (await _appDbContext.Games.AnyAsync(x => x.Name == request.Name, cancellationToken))
				throw new RequestNameNotUniqException<CreateGameCommand>(nameof(request.Name));

			var currentUser = await _appDbContext.Users
				.FirstOrDefaultAsync(u => u.Id == _userContext.CurrentUserId, cancellationToken)
				?? throw new RequestNullException<User>("Текущий пользователь не найден");

			var currentMainMasterRole = await _appDbContext.GameRoles
				.FirstOrDefaultAsync(u => u.Name == GameRoles.MainMasterRoleName, cancellationToken)
				?? throw new RequestNullException<GameRole>("Роль главного мастера не найдена");

			var currentGameInterface = await _appDbContext.Interfaces
				.FirstOrDefaultAsync(u => u.Name == SystemInterfaces.GameDarkName, cancellationToken)
				?? throw new RequestNullException<Interface>("Интерфейс не найден");

			var avatar = request.AvatarId == null
				? null
				: await _appDbContext.ImgFiles
				.FirstOrDefaultAsync(x => x.Id == request.AvatarId, cancellationToken)
				?? throw new EntityNotFoundException<ImgFile>(request.AvatarId.Value);

			var textFiles = new List<TextFile>();

			if (request.TextFiles != null)
				foreach (var x in request.TextFiles)
				{
					var textFile = await _appDbContext.TextFiles
					.FirstOrDefaultAsync(y => y.Id == x, cancellationToken)
					?? throw new EntityNotFoundException<TextFile>(x);
					textFiles.Add(textFile);
				}

			var imgFiles = new List<ImgFile>();

			if (request.ImgFiles != null)
				foreach (var x in request.ImgFiles)
				{
					var imgFile = await _appDbContext.ImgFiles
					.FirstOrDefaultAsync(y => y.Id == x, cancellationToken)
					?? throw new EntityNotFoundException<ImgFile>(x);
					imgFiles.Add(imgFile);
				}

			var newGame = Game.CreateGame(
				name: request.Name,
				avatar: avatar,
				description: request.Description,
				user: currentUser,
				@interface: currentGameInterface,
				mainMasterRole: currentMainMasterRole);

			_changeListService.ChangeTextFilesList(newGame, textFiles);
			_changeListService.ChangeImgFilesList(newGame, imgFiles);

			_appDbContext.Games.Add(newGame);
			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
