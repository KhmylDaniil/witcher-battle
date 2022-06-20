using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.InstanceRequests.CreateInstance;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.InstanceRequests.CreateInstance
{
	/// <summary>
	/// Обработчик создания инстанса
	/// </summary>
	public class CreateInstanceHandler : IRequestHandler<CreateInstanceCommand>
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
		/// Конструктор обработчика создания шаблона существа
		/// </summary>
		/// <param name="appDbContext"></param>
		/// <param name="authorizationService"></param>
		public CreateInstanceHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Обработчик создания инстанса
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Инстанс</returns>
		public async Task<Unit> Handle(CreateInstanceCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(x => x.CreatureTemplates)
					.ThenInclude(x => x.CreatureTemplateParts)
					.ThenInclude(x => x.BodyPartType)
				.Include(x => x.CreatureTemplates)
					.ThenInclude(x => x.BodyTemplate)
				.Include(x => x.CreatureTemplates)
					.ThenInclude(x => x.Abilities)
					.ThenInclude(x => x.AppliedConditions)
					.ThenInclude(x => x.Condition)
				.Include(x => x.CreatureTemplates)
					.ThenInclude(x => x.CreatureTemplateParameters)
					.ThenInclude(x => x.Parameter)
				.Include(x => x.Instances)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			var imgFile = request.ImgFileId == null
				? null
				: await _appDbContext.ImgFiles.FirstOrDefaultAsync(x => x.Id == request.ImgFileId, cancellationToken)
				?? throw new ExceptionEntityNotFound<ImgFile>(request.ImgFileId.Value);

			CheckRequest(request, game);

			var newInstance = new Instance(
				game: game,
				imgFile: imgFile,
				name: request.Name,
				description: request.Description);

			newInstance.Creatures = CreateCreatures(request.Creatures, game, newInstance);

			game.Instances.Add(newInstance);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		/// <summary>
		/// Проверка запроса
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		private void CheckRequest(CreateInstanceCommand request, Game game)
		{
			if (game.Instances.Any(x => x.Name == request.Name))
				throw new ExceptionRequestNameNotUniq<CreateInstanceCommand>(nameof(request.Name));

			foreach (var item in request.Creatures)
			{
				_ = game.CreatureTemplates.FirstOrDefault(x => x.Id == item.CreatureTemplateId)
					?? throw new ExceptionEntityNotFound<CreatureTemplate>(item.CreatureTemplateId);

				if (string.IsNullOrEmpty(item.Name))
					throw new ExceptionRequestFieldNull<CreateInstanceRequestItem>(nameof(item.Name));

				if (request.Creatures.Where(x => x.Name == item.Name).Count() != 1)
					throw new ApplicationException($"Значения в поле {nameof(item.Name)} повторяются");
			}
		}

		/// <summary>
		/// Создание списка существ
		/// </summary>
		/// <param name="data">Данные</param>
		/// <param name="game">Игра</param>
		/// <param name="instance">Инстанс</param>
		/// <returns>Список существ</returns>
		private List<Creature> CreateCreatures(
			List<CreateInstanceRequestItem> data, Game game, Instance instance)
		{
			var result = new List<Creature>();
			foreach (var item in data)
				result.Add(new Creature(
					creatureTemplate: game.CreatureTemplates.FirstOrDefault(x => x.Id == item.CreatureTemplateId),
					instance: instance,
					name: item.Name,
					description: item.Description));
			return result;
		}
	}
}
