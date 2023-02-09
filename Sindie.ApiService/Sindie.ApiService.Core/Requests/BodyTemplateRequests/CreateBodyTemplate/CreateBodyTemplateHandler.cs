using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests;
using Sindie.ApiService.Core.Contracts.BodyTemplateRequests.CreateBodyTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BodyTemplateRequests.CreateBodyTemplate
{
	/// <summary>
	/// Обработчик создания шаблона тела
	/// </summary>
	public class CreateBodyTemplateHandler : IRequestHandler<CreateBodyTemplateRequest, BodyTemplate>
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
		/// Конструктор обработчика создания шаблона тела
		/// </summary>
		/// <param name="appDbContext"></param>
		/// <param name="authorizationService"></param>
		public CreateBodyTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Создание шаблона тела
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns></returns>
		public async Task<BodyTemplate> Handle(CreateBodyTemplateRequest request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(x => x.BodyTemplates)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			CheckRequest(request, game);

			var newBodyTemplate = new BodyTemplate(
				game: game,
				name: request.Name,
				description: request.Description,
				bodyTemplateParts: BodyTemplatePartsData.CreateBodyTemplatePartsData(request));

			_appDbContext.BodyTemplates.Add(newBodyTemplate);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return newBodyTemplate;
		}

		/// <summary>
		/// Проверка запроса
		/// </summary>
		/// <param name="request">Запрос</param>
		private void CheckRequest(CreateBodyTemplateRequest request, Game game)
		{
			if (game.BodyTemplates.Any(x => x.Name == request.Name))
				throw new ExceptionRequestNameNotUniq<CreateBodyTemplateRequest>(nameof(request.Name));

			if (request.BodyTemplateParts == null)
				return;

			var sortedList = request.BodyTemplateParts.OrderBy(x => x.MinToHit).ToList();

			foreach (var part in sortedList)
			{
				if (request.BodyTemplateParts.Count(x => x.Name == part.Name) != 1)
					throw new ArgumentException($"Значения в поле {nameof(part.Name)} повторяются");

				if (!Enum.IsDefined(part.BodyPartType))
					throw new ExceptionRequestFieldIncorrectData<CreateBodyTemplateRequest>(nameof(part.BodyPartType));

				if (part.DamageModifier <= 0)
					throw new ExceptionRequestFieldIncorrectData<CreateBodyTemplateRequest>(nameof(UpdateBodyTemplateRequestItem.DamageModifier));

				if (part.MaxToHit > 10 || part.MaxToHit < part.MinToHit)
					throw new ExceptionRequestFieldIncorrectData<CreateBodyTemplateRequest>(nameof(UpdateBodyTemplateRequestItem.MaxToHit));
			}

			if (sortedList.First().MinToHit != 1 || sortedList.Last().MaxToHit != 10)
				throw new ArgumentException($"Значения таблицы попаданий не охватывают необходимый диапазон");

			for (int i = 1; i < sortedList.Count; i++)
				if (sortedList[i].MinToHit != sortedList[i - 1].MaxToHit + 1)
					throw new ArgumentException($"Значения таблицы попаданий пересекаются");
		}
	}
}
