using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.SkillRequests.CreateSkill
{
	/// <summary>
	/// Обработчик команды создания навыка
	/// </summary>
	public class CreateSkillHandler : IRequestHandler<CreateSkillCommand>
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
		/// Конструктор обработчика создания навыка
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		public CreateSkillHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Создание навыка
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены запроса</param>
		/// <returns>Ответ</returns>
		public async Task<Unit> Handle
			(CreateSkillCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(x => x.Skills)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			if (game.Skills.Any(x => x.Name == request.Name))
				throw new ExceptionRequestNameNotUniq<CreateSkillCommand>(nameof(request.Name));

			var parameter = new Skill(
				game: game,
				name: request.Name,
				description: request.Description,
				statName: request.StatName,
				minValueSkill: request.MinValueSkill,
				maxValueSkill: request.MaxValueSkill);

			_appDbContext.Skills.Add(parameter);
			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}