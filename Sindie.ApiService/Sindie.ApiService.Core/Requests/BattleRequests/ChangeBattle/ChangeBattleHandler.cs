using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.BattleRequests.ChangeBattle;
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

namespace Sindie.ApiService.Core.Requests.BattleRequests.ChangeBattle
{
	/*
	
	/// <summary>
	/// Обработчик создания инстанса
	/// </summary>
	public class ChangeInstanceHandler : IRequestHandler<ChangeInstanceRequest>
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
		/// Конструктор обработчика создания инстанса
		/// </summary>
		/// <param name="appDbContext"></param>
		/// <param name="authorizationService"></param>
		public ChangeInstanceHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}


		public async Task<Unit> Handle(ChangeInstanceCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(x => x.CreatureTemplates)
				.Include(x => x.Instances)
				.ThenInclude(x => x.Creatures)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			var imgFile = request.ImgFileId == null
				? null
				: await _appDbContext.ImgFiles.FirstOrDefaultAsync(x => x.Id == request.ImgFileId, cancellationToken)
				?? throw new ExceptionEntityNotFound<ImgFile>(request.ImgFileId.Value);

			CheckRequest(request, game);

			var instance = game.Instances.FirstOrDefault(x => x.Id == request.Id);

				new Instance(
				game: game,
				imgFile: imgFile,
				name: request.Name,
				description: request.Description);

			throw new NotImplementedException();
			

		}

		/// <summary>
		/// Проверка запроса
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		private void CheckRequest(ChangeInstanceCommand request, Game game)
		{
			var instance = game.Instances.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<Instance>(request.Id);

			if (game.Instances.Any(x => x.Name == request.Name && x.Id != request.Id))
				throw new ExceptionRequestNameNotUniq<ChangeInstanceCommand>(nameof(request.Name));

			foreach (var item in request.Creatures)
			{
				_ = game.CreatureTemplates.FirstOrDefault(x => x.Id == item.CreatureTemplateId)
					?? throw new ExceptionEntityNotFound<CreatureTemplate>(item.CreatureTemplateId);

				if (item.CreatureId != null)
					_ = instance.Creatures.FirstOrDefault(x => x.Id == item.CreatureId)
						?? throw new ExceptionEntityNotFound<Creature>(item.CreatureId.Value);

				if (string.IsNullOrEmpty(item.Name))
					throw new ExceptionRequestFieldNull<ChangeInstanceCommand>(nameof(item.Name));

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
			List<ChangeInstanceRequestItem> data, Game game, Instance instance)
		{
			var result = new List<Creature>();
			foreach (var item in data)
			{
				if (item.CreatureId != null)
					result.Add(instance.Creatures.FirstOrDefault(x => x.Id == item.CreatureTemplateId));
				else
					result.Add(new Creature(
					creatureTemplate: game.CreatureTemplates.FirstOrDefault(x => x.Id == item.CreatureTemplateId),
					instance: instance,
					name: item.Name,
					description: item.Description));
			}
			return result;
		}
	}

	*/
}
