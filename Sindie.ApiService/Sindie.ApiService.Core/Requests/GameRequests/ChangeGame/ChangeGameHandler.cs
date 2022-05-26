using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.GameRequests.ChangeGame;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.GameRequests.CreateGame
{
	/// <summary>
	/// Обработчик команды изменения игры
	/// </summary>
	public class ChangeGameHandler : IRequestHandler<ChangeGameCommand, Unit>
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Сервис авторизации
		/// </summary>
		private readonly IAuthorizationService _authorizationService;

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
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
			_changeListService = changeListService;
		}

		/// <summary>
		/// Обработка команды создания игры
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns></returns>
		public async Task<Unit> Handle(ChangeGameCommand request, CancellationToken cancellationToken)
		{
			///проверка запроса на нулл
			if (request == null)
				throw new ExceptionRequestNull<ChangeGameCommand>();

			//проверка имени на нулл или пустую строку
			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ExceptionRequestFieldNull<ChangeGameCommand>($"{nameof(request.Name)}");

			var game = await _authorizationService
				.RoleGameFilter(_appDbContext.Games, request.Id, GameRoles.MainMasterRoleId)
				.Include(x => x.Avatar)
				.Include(x => x.ImgFiles)
				.Include(x => x.TextFiles)
				.Include(x => x.UserGames)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionEntityNotFound<Game>(request.Id);

			//проверка на уникальность имени игры
			if (request.Name != game.Name && await _appDbContext.Games
				.AnyAsync(x => x.Name == request.Name, cancellationToken))
				throw new ExceptionRequestNameNotUniq<ChangeGameCommand>(nameof(request.Name));

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
