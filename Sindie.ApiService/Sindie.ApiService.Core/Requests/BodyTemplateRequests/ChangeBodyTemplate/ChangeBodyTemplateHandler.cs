using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.ChangeBodyTemplate;
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

namespace Sindie.ApiService.Core.Requests.BodyTemplateRequests.ChangeBodyTemplate
{
	public class ChangeBodyTemplateHandler: IRequestHandler <ChangeBodyTemplateCommand, Unit>
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
		public async Task<Unit> Handle(ChangeBodyTemplateCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(x => x.BodyTemplates)
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
		private void CheckRequest(ChangeBodyTemplateCommand request, Game game)
		{
			if (game.BodyTemplates.Any(x => x.Name == request.Name && x.Id != request.Id))
				throw new ExceptionRequestNameNotUniq<ChangeBodyTemplateCommand>(nameof(request.Name));

			var bodyTemplate = game.BodyTemplates.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<BodyTemplate>(request.Id);

			var sortedList = request.BodyTemplateParts.OrderBy(x => x.MinToHit).ToList();

			foreach (var part in sortedList)
			{
				if (string.IsNullOrEmpty(part.Name))
					throw new ExceptionRequestFieldNull<ChangeBodyTemplateCommand>(nameof(ChangeBodyTemplateRequestItem.Name));
				if (request.BodyTemplateParts.Where(x => x.Name == part.Name).Count() != 1)
					throw new ApplicationException($"Значения в поле {nameof(part.Name)} повторяются");

				if (part.DamageModifier <= 0)
					throw new ExceptionRequestFieldIncorrectData<ChangeBodyTemplateCommand>(nameof(ChangeBodyTemplateRequestItem.DamageModifier));

				if (part.HitPenalty < 1)
					throw new ExceptionRequestFieldIncorrectData<ChangeBodyTemplateCommand>(nameof(ChangeBodyTemplateRequestItem.HitPenalty));

				if (part.MinToHit < 1)
					throw new ExceptionRequestFieldIncorrectData<ChangeBodyTemplateCommand>(nameof(ChangeBodyTemplateRequestItem.MinToHit));

				if (part.MaxToHit > 10 || part.MaxToHit < part.MinToHit)
					throw new ExceptionRequestFieldIncorrectData<ChangeBodyTemplateCommand>(nameof(ChangeBodyTemplateRequestItem.MaxToHit));
			}

			if (sortedList.First().MinToHit != 1 || sortedList.Last().MaxToHit != 10)
				throw new ApplicationException($"Значения таблицы попаданий не охватывают необходимый диапазон");

			for (int i = 1; i < sortedList.Count; i++)
				if (sortedList[i].MinToHit != sortedList[i - 1].MaxToHit + 1)
					throw new ApplicationException($"Значения таблицы попаданий пересекаются");
		}
	}
}
