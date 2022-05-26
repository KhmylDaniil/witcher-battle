using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.CharacterTemplateRequests.DeleteCharacterTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.CharacterTemplateRequests.DeleteCharacterTemplate
{
	/// <summary>
	/// Обработчик удаления шаблона персонажа
	/// </summary>
	public class DeleteCharacterTemplateHandler : IRequestHandler<DeleteCharacterTemplateCommand, Unit>
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Сервис автоизации
		/// </summary>
		private readonly IAuthorizationService _authorizationService;

		/// <summary>
		/// Конструктор обработчика удаления шаблона персонажа
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		public DeleteCharacterTemplateHandler(
			IAppDbContext appDbContext,
			IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Обработчик удаления шаблона персонажа
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Юнит</returns>
		public async Task<Unit> Handle(DeleteCharacterTemplateCommand request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ExceptionRequestNull<DeleteCharacterTemplateCommand>();

			var characterTemplate = await _authorizationService.RoleGameFilter
				(_appDbContext.Games, request.GameId, GameRoles.MasterRoleId)
				.SelectMany(x => x.CharacterTemplates)
				.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
				?? throw new ExceptionEntityNotFound<CharacterTemplate>(request.Id);

			_appDbContext.CharacterTemplates.Remove(characterTemplate);
			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
