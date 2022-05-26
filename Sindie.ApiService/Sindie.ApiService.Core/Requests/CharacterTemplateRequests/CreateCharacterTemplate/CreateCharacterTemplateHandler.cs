using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.CharacterTemplateRequests.CreateCharacterTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.CharacterTemplateRequests.CreateCharacterTemplate
{
	/// <summary>
	/// Обработчик команды создания шаблона персонажа
	/// </summary>
	public class CreateCharacterTemplateHandler : IRequestHandler<CreateCharacterTemplateCommand, Unit>
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
		/// Конструктор для <see cref="CreateCharacterTemplateHandler"/>
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		public CreateCharacterTemplateHandler(
			IAppDbContext appDbContext,
			IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Создание шаблона персонажа
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Юнит</returns>
		public async Task<Unit> Handle(CreateCharacterTemplateCommand request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ExceptionRequestNull<CreateCharacterTemplateCommand>();

			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ExceptionRequestFieldNull<CreateCharacterTemplateCommand>(nameof(request.Name));

			var game = await _authorizationService
				.RoleGameFilter(_appDbContext.Games, request.GameId, GameRoles.MasterRoleId)
				.Include(x => x.CharacterTemplates)
				.Include(x => x.Modifiers)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionEntityNotFound<Game>(request.GameId);

			if (game.CharacterTemplates.Any(x => x.Name == request.Name))
				throw new ExceptionRequestNameNotUniq<CreateCharacterTemplateCommand>(nameof(request.Name));

			var imgFile = request.ImgFileId == null
				? null
				: await _appDbContext.ImgFiles
				.FirstOrDefaultAsync(x => x.Id == request.ImgFileId, cancellationToken)
				?? throw new ExceptionEntityNotFound<ImgFile>((Guid)request.ImgFileId);

			var @interface = request.InterfaceId == null
				? null
				: await _appDbContext.Interfaces
				.FirstOrDefaultAsync(x => x.Id == request.InterfaceId, cancellationToken)
				?? throw new ExceptionEntityNotFound<Interface>((Guid)request.InterfaceId);

			TryRelatedEntities(game, request);

			var newCharacterTemplate = CharacterTemplate.CreateCharacterTemplate(
				game, imgFile, @interface, request.Name, request.Description);

			newCharacterTemplate.ChangeCharacterTemplateModifiersList(
				CreateModifiersList(game.Modifiers, request.Modifiers));

			_appDbContext.CharacterTemplates.Add(newCharacterTemplate);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		private List<Modifier> CreateModifiersList(List<Modifier> modifiers, List<Guid> guids)
		{
			var result = new List<Modifier>();
			foreach (var guid in guids)
				result.Add(modifiers.FirstOrDefault(y => y.Id == guid));
			return result;
		}

		private void TryRelatedEntities(Game game, CreateCharacterTemplateCommand request)
		{
			if (request.Modifiers == null)
				throw new ExceptionRequestFieldNull<CreateCharacterTemplateCommand>(nameof(request.Modifiers));
			if (request.Modifiers.Any())
				foreach (var x in request.Modifiers)
					_ = game.Modifiers.FirstOrDefault(y => y.Id == x)
						?? throw new ExceptionEntityNotFound<Modifier>(x);
		}
	}
}
