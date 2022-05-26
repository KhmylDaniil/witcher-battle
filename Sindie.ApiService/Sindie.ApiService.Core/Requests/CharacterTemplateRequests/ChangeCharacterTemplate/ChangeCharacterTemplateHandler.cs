using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.CharacterTemplateRequests.ChangeCharacterTemplate;
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

namespace Sindie.ApiService.Core.Requests.CharacterTemplateRequests.ChangeCharacterTemplate
{
	/// <summary>
	/// Обработчик команды изменения шаблона персонажа
	/// </summary>
	public class ChangeCharacterTemplateHandler : IRequestHandler<ChangeCharacterTemplateCommand, Unit>
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
		/// Конструктор для <see cref="ChangeCharacterTemplateHandler"/>
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		public ChangeCharacterTemplateHandler(
			IAppDbContext appDbContext,
			IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Изменение шаблона персонажа
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Юнит</returns>
		public async Task<Unit> Handle(ChangeCharacterTemplateCommand request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ExceptionRequestNull<ChangeCharacterTemplateCommand>();

			var game = await _authorizationService.RoleGameFilter(
				_appDbContext.Games, request.GameId, GameRoles.MasterRoleId)
				.Include(x => x.CharacterTemplates.Where(m => m.Id == request.Id))
					.ThenInclude(y => y.CharacterTemplateModifiers)
				.Include(x => x.Instances)
					.ThenInclude(i => i.Characters)
				.Include(x => x.Modifiers)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionNoAccessToEntity<Game>();

			var characterTemplate = game.CharacterTemplates.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<CharacterTemplate>(request.Id);

			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ExceptionRequestFieldNull<ChangeCharacterTemplateCommand>(nameof(request.Name));

			if (request.Name != characterTemplate.Name && game.CharacterTemplates
				.Any(x => x.Name == request.Name))
				throw new ExceptionRequestNameNotUniq<ChangeCharacterTemplateCommand>(nameof(request.Name));

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

			characterTemplate.ChangeCharacterTemplate(
				name: request.Name,
				description: request.Description,
				imgFile: imgFile,
				@interface: @interface);

			characterTemplate.ChangeCharacterTemplateModifiersList(
				CreateModifiersList(game.Modifiers, request.Modifiers));

			characterTemplate.Characters = CreateCharactersList(
				game.Instances.SelectMany(x => x.Characters),
				request.Characters);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		private static void TryRelatedEntities(Game game, ChangeCharacterTemplateCommand request)
		{
			if (request.Modifiers == null)
				throw new ExceptionRequestFieldNull<ChangeCharacterTemplateCommand>(nameof(request.Modifiers));
			if (request.Characters == null)
				throw new ExceptionRequestFieldNull<ChangeCharacterTemplateCommand>(nameof(request.Characters));

			if (request.Modifiers.Any())
				foreach (var x in request.Modifiers)
					_ = game.Modifiers.FirstOrDefault(y => y.Id == x)
						?? throw new ExceptionEntityNotFound<Modifier>(x);

			if (request.Characters.Any())
				foreach (var x in request.Characters)
					_ = game.Instances.SelectMany(x => x.Characters).FirstOrDefault(c => c.Id == x)
						?? throw new ExceptionEntityNotFound<Character>(x);
		}

		private List<Modifier> CreateModifiersList(List<Modifier> entities, List<Guid> guids)
		{
			var result = new List<Modifier>();
			foreach (var guid in guids)
				result.Add(entities.FirstOrDefault(y => y.Id == guid));
			return result;
		}

		private List<Character> CreateCharactersList(IEnumerable<Character> entities, List<Guid> guids)
		{
			var result = new List<Character>();
			foreach (var guid in guids)
				result.Add(entities.FirstOrDefault(y => y.Id == guid));
			return result;
		}
	}
}
