using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.GameRequests.CreateGame;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.GameRequests.CreateGame
{
	/// <summary>
	/// Обработчик команды создания игры
	/// </summary>
	public class CreateGameHandler : IRequestHandler<CreateGameCommand, Unit>
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Интерфейс получения данных пользователя из веба
		/// </summary>
		private readonly IUserContext _userContext;

		/// <summary>
		/// Сервис изменения списков графических и текстовых файлов
		/// </summary>
		private readonly IChangeListService _changeListService;

		/// <summary>
		/// Конструктор обработчика команды регистрации пользователя
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="userContext">Получение контекста пользователя из веба</param>
		/// <param name="changeListService">Сервис изменения списков графических и текстовых файлов</param>
		public CreateGameHandler(
			IAppDbContext appDbContext,
			IUserContext userContext,
			IChangeListService changeListService)
		{
			_appDbContext = appDbContext;
			_userContext = userContext;
			_changeListService = changeListService;
		}

		/// <summary>
		/// Обработка команды создания игры
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns></returns>
		public async Task<Unit> Handle(CreateGameCommand request, CancellationToken cancellationToken)
		{
			///проверка запроса на нулл
			if (request is null)
				throw new ExceptionRequestNull<CreateGameCommand>();

			//проверка имени на нулл или пустую строку
			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ExceptionRequestFieldNull<CreateGameCommand>($"{nameof(request.Name)}");

			//проверка на уникальность имени игры
			if (await _appDbContext.Games
				.AnyAsync(x => x.Name == request.Name, cancellationToken))
				throw new ExceptionRequestNameNotUniq<CreateGameCommand>(nameof(request.Name));

			//проверка пользователя, все равно нам нужен currentUser
			var currentUser = await _appDbContext.Users
				.FirstOrDefaultAsync(u => u.Id == _userContext.CurrentUserId, cancellationToken)
				?? throw new ExceptionRequestNull<User>("Текущий пользователь не найден");

			//находим роль мастера
			var currentMasterRole = await _appDbContext.GameRoles
				.FirstOrDefaultAsync(u => u.Name == GameRoles.MasterRoleName, cancellationToken)
				?? throw new ExceptionRequestNull<GameRole>("Роль мастера не найдена");

			//находим роль главмастера
			var currentMainMasterRole = await _appDbContext.GameRoles
				.FirstOrDefaultAsync(u => u.Name == GameRoles.MainMasterRoleName, cancellationToken)
				?? throw new ExceptionRequestNull<GameRole>("Роль главного мастера не найдена");

			//находим интерфейс
			var currentGameInterface = await _appDbContext.Interfaces
				.FirstOrDefaultAsync(u => u.Name == SystemInterfaces.GameDarkName, cancellationToken)
				?? throw new ExceptionRequestNull<Interface>("Интерфейс не найден");

			//находим аватар, проверяем на наличие в БД 
			var avatar = request.AvatarId == null
				? null
				: await _appDbContext.ImgFiles
				.FirstOrDefaultAsync(x => x.Id == request.AvatarId, cancellationToken)
				?? throw new ExceptionEntityNotFound<ImgFile>(nameof(request.AvatarId));

			//находим списки и проверяем на наличие в БД
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

			var newGame = Game.CreateGame(
				name: request.Name,
				avatar: avatar,
				description: request.Description,
				user: currentUser,
				@interface: currentGameInterface,
				masterRole: currentMasterRole,
				mainMasterRole: currentMainMasterRole);

			_changeListService.ChangeTextFilesList(newGame, textFiles);
			_changeListService.ChangeImgFilesList(newGame, imgFiles);

			_appDbContext.Games.Add(newGame);
			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
