using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests.CreateBattle;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.BattleRequests.CreateBattle
{
	/// <summary>
	/// Обработчик создания боя
	/// </summary>
	public class CreateBattleHandler : IRequestHandler<CreateBattleCommand>
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
		/// Конструктор обработчика создания боя
		/// </summary>
		/// <param name="appDbContext"></param>
		/// <param name="authorizationService"></param>
		public CreateBattleHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Обработчик создания боя
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Бой</returns>
		public async Task<Unit> Handle(CreateBattleCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(x => x.CreatureTemplates)
					.ThenInclude(x => x.CreatureTemplateParts)
				.Include(x => x.CreatureTemplates)
					.ThenInclude(x => x.BodyTemplate)
				.Include(x => x.CreatureTemplates)
					.ThenInclude(x => x.Abilities)
					.ThenInclude(x => x.AppliedConditions)
					.ThenInclude(x => x.Condition)
				.Include(x => x.CreatureTemplates)
					.ThenInclude(x => x.CreatureTemplateSkills)
				.Include(x => x.Battles)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			var imgFile = request.ImgFileId == null
				? null
				: await _appDbContext.ImgFiles.FirstOrDefaultAsync(x => x.Id == request.ImgFileId, cancellationToken)
				?? throw new ExceptionEntityNotFound<ImgFile>(request.ImgFileId.Value);

			CheckRequest(request, game);

			var newBattle = new Battle(
				game: game,
				imgFile: imgFile,
				name: request.Name,
				description: request.Description);

			newBattle.Creatures = CreateCreatures(request.Creatures, game, newBattle);

			game.Battles.Add(newBattle);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		/// <summary>
		/// Проверка запроса
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		private void CheckRequest(CreateBattleCommand request, Game game)
		{
			if (game.Battles.Any(x => x.Name == request.Name))
				throw new ExceptionRequestNameNotUniq<CreateBattleCommand>(nameof(request.Name));

			foreach (var item in request.Creatures)
			{
				_ = game.CreatureTemplates.FirstOrDefault(x => x.Id == item.CreatureTemplateId)
					?? throw new ExceptionEntityNotFound<CreatureTemplate>(item.CreatureTemplateId);

				if (string.IsNullOrEmpty(item.Name))
					throw new ExceptionRequestFieldNull<CreateBattleRequestItem>(nameof(item.Name));

				if (request.Creatures.Where(x => x.Name == item.Name).Count() != 1)
					throw new ApplicationException($"Значения в поле {nameof(item.Name)} повторяются");
			}
		}

		/// <summary>
		/// Создание списка существ
		/// </summary>
		/// <param name="data">Данные</param>
		/// <param name="game">Игра</param>
		/// <param name="battle">Бой</param>
		/// <returns>Список существ</returns>
		private List<Creature> CreateCreatures(
			List<CreateBattleRequestItem> data, Game game, Battle battle)
		{
			var result = new List<Creature>();
			foreach (var item in data)
				result.Add(new Creature(
					creatureTemplate: game.CreatureTemplates.FirstOrDefault(x => x.Id == item.CreatureTemplateId),
					battle: battle,
					name: item.Name,
					description: item.Description));
			return result;
		}
	}
}
