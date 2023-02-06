using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.ChangeBodyTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BodyTemplateRequests.ChangeBodyTemplate
{
	public class ChangeBodyTemplateHandler: IRequestHandler <ChangeBodyTemplateRequest, Unit>
	{
		/// <summary>
		/// Контекст базы данных
		/// </summary>
		private readonly IAppDbContext _appDbContext;

		/// <summary>
		/// Сервис авторизации
		/// </summary>
		private readonly IAuthorizationService _authorizationService;

		public ChangeBodyTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Обработка изменения шаблона тела
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public async Task<Unit> Handle(ChangeBodyTemplateRequest request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(x => x.BodyTemplates)
					.ThenInclude(x => x.BodyTemplateParts)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			CheckRequest(request, game);

			game.BodyTemplates.FirstOrDefault(x => x.Id == request.Id)
				.ChangeBodyTemplate(
				game: game,
				name: request.Name,
				description: request.Description,
				bodyTemplateParts: BodyTemplatePartsData.CreateBodyTemplatePartsData(request));

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		/// <summary>
		/// Проверка запроса
		/// </summary>
		/// <param name="request">Запрос</param>
		private void CheckRequest(ChangeBodyTemplateRequest request, Game game)
		{
			if (game.BodyTemplates.Any(x => x.Name == request.Name && x.Id != request.Id))
				throw new ExceptionRequestNameNotUniq<ChangeBodyTemplateRequest>(nameof(request.Name));

			_ = game.BodyTemplates.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<BodyTemplate>(request.Id);

			var sortedList = request.BodyTemplateParts.OrderBy(x => x.MinToHit).ToList();

			foreach (var part in sortedList)
			{
				if (string.IsNullOrEmpty(part.Name))
					throw new ExceptionRequestFieldNull<ChangeBodyTemplateRequest>(nameof(ChangeBodyTemplateRequestItem.Name));
				if (request.BodyTemplateParts.Count(x => x.Name == part.Name) != 1)
					throw new ArgumentException($"Значения в поле {nameof(part.Name)} повторяются");

				if (!Enum.IsDefined(part.BodyPartType))
					throw new ExceptionRequestFieldIncorrectData<ChangeBodyTemplateRequest>(nameof(part.BodyPartType));

				if (part.DamageModifier <= 0)
					throw new ExceptionRequestFieldIncorrectData<ChangeBodyTemplateRequest>(nameof(ChangeBodyTemplateRequestItem.DamageModifier));

				if (part.HitPenalty < 1)
					throw new ExceptionRequestFieldIncorrectData<ChangeBodyTemplateRequest>(nameof(ChangeBodyTemplateRequestItem.HitPenalty));

				if (part.MinToHit < 1)
					throw new ExceptionRequestFieldIncorrectData<ChangeBodyTemplateRequest>(nameof(ChangeBodyTemplateRequestItem.MinToHit));

				if (part.MaxToHit > 10 || part.MaxToHit < part.MinToHit)
					throw new ExceptionRequestFieldIncorrectData<ChangeBodyTemplateRequest>(nameof(ChangeBodyTemplateRequestItem.MaxToHit));
			}

			if (sortedList.First().MinToHit != 1 || sortedList.Last().MaxToHit != 10)
				throw new ArgumentException($"Значения таблицы попаданий не охватывают необходимый диапазон");

			for (int i = 1; i < sortedList.Count; i++)
				if (sortedList[i].MinToHit != sortedList[i - 1].MaxToHit + 1)
					throw new ArgumentException($"Значения таблицы попаданий пересекаются");
		}
	}
}
