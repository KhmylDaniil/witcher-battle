using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.ModifierRequests.CreateModifier;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.ModifierRequests.CreateModifier
{
	/// <summary>
	/// Обработчик команды создания модификатора
	/// </summary>
	public class CreateModifierHandler: IRequestHandler<CreateModifierCommand, Unit>
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
		/// Конструктор для <see cref="CreateModifierHandler"/>
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		public CreateModifierHandler(
			IAppDbContext appDbContext,
			IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Обработка команды создания модификатора
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Юнит</returns>
		public async Task<Unit> Handle(CreateModifierCommand request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ExceptionRequestNull<CreateModifierCommand>();

			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ExceptionRequestFieldNull<CreateModifierCommand>($"{nameof(request.Name)}");

			var game = await _authorizationService
				.RoleGameFilter(_appDbContext.Games, request.GameId, GameRoles.MasterRoleId)
				.Include(x => x.ItemTemplates)
				.Include(x => x.CharacterTemplates)
				.Include(x => x.Events)
				.Include(x => x.Scripts)
				.Include(x => x.Modifiers)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionEntityNotFound<Game>(request.GameId);

			if (game.Modifiers.Any(x => x.Name == request.Name))
				throw new ExceptionRequestNameNotUniq<CreateModifierCommand>(nameof(request.Name));

			var imgFile = request.ImgFileId == null
				? null
				: await _appDbContext.ImgFiles
				.FirstOrDefaultAsync(x => x.Id == request.ImgFileId, cancellationToken)
				?? throw new ExceptionEntityNotFound<ImgFile>((Guid)request.ImgFileId);

			TryRelatedEntities(game, request);

			var newModifier = Modifier.CreateModifier(
				game, imgFile, request.Name, request.Description);

			newModifier.ChangeItemTemplateModifiersList(
				CreateItemTemplatesList(game.ItemTemplates, request.ItemTemplates));
			newModifier.ChangeCharacterTemplateModifiersList(
				CreateCharacterTemplatesList(game.CharacterTemplates, request.CharacterTemplates));
			newModifier.CreateModifierScriptsList(
				CreateModifierScriptsData(request, game));
			
			_appDbContext.Modifiers.Add(newModifier);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		/// <summary>
		/// Получение данных для создания списка скриптов модификатора
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		/// <returns>Список данных</returns>
		private IEnumerable<(Script Script,
				Event Event,
				DateTime ActivationTime,
				int PeriodOfActivity,
				int PeriodOfInactivity,
				int NumberOfRepetitions)> 
			CreateModifierScriptsData(CreateModifierCommand request, Game game)
		{
			var result = new List<(Script script,
				Event @event,
				DateTime activationTime,
				int periodOfActivity,
				int periodOfInactivity,
				int numberOfRepetitions)>();

			if (request.ModifierScripts != null)
				foreach (var requestItem in request.ModifierScripts)
					result.Add((
						script: game.Scripts.FirstOrDefault(x => x.Id == requestItem.ScriptId),
						@event: requestItem.EventId == null
								? null
								: game.Events.FirstOrDefault(x => x.Id == requestItem.EventId),
						activationTime: requestItem.ActivationTime,
						periodOfActivity: requestItem.PeriodOfActivity,
						periodOfInactivity: requestItem.PeriodOfInactivity,
						numberOfRepetitions: requestItem.NumberOfRepetitions));

			return result;
		}

		/// <summary>
		/// Проверка списков зависимых сущностей
		/// </summary>
		/// <param name="game">Игра</param>
		/// <param name="request">Запрос</param>
		private static void TryRelatedEntities(Game game, CreateModifierCommand request)
		{
			if (request.CharacterTemplates == null)
				throw new ExceptionRequestFieldNull<CreateModifierCommand>(nameof(request.CharacterTemplates));
			if (request.ItemTemplates == null)
				throw new ExceptionRequestFieldNull<CreateModifierCommand>(nameof(request.ItemTemplates));
			if (request.ModifierScripts == null)
				throw new ExceptionRequestFieldNull<CreateModifierCommand>(nameof(request.ModifierScripts));

			if (request.CharacterTemplates.Any())
				foreach (var x in request.CharacterTemplates)
					_ = game.CharacterTemplates.FirstOrDefault(y => y.Id == x)
						?? throw new ExceptionEntityNotFound<CharacterTemplate>(x);

			if (request.ItemTemplates.Any())
				foreach (var x in request.ItemTemplates)
					_= game.ItemTemplates.FirstOrDefault(y => y.Id == x) 
						?? throw new ExceptionEntityNotFound<ItemTemplate>(x);

			if (request.ModifierScripts.Any())
				foreach (var requestItem in request.ModifierScripts)
				{
					_ = game.Scripts.FirstOrDefault(y => y.Id == requestItem.ScriptId)
							?? throw new ExceptionEntityNotFound<Script>(requestItem.ScriptId);
					if (requestItem.EventId != null)
						_ = game.Events.FirstOrDefault(y => y.Id == requestItem.EventId)
							?? throw new ExceptionEntityNotFound<Event>((Guid)requestItem.EventId);
					if (requestItem.PeriodOfActivity < 0)
						throw new ExceptionRequestFieldIncorrectData<CreateModifierCommandItem>(nameof(requestItem.PeriodOfActivity));
					if (requestItem.PeriodOfInactivity < 0)
						throw new ExceptionRequestFieldIncorrectData<CreateModifierCommandItem>(nameof(requestItem.PeriodOfInactivity));
					if (requestItem.NumberOfRepetitions < 0)
						throw new ExceptionRequestFieldIncorrectData<CreateModifierCommandItem>(nameof(requestItem.NumberOfRepetitions));
				}
		}

		/// <summary>
		/// Создание списка шаблонов персонажа
		/// </summary>
		/// <param name="entities">Данные из БД</param>
		/// <param name="guids">Список айди</param>
		/// <returns>Список шаблонов персонажа</returns>
		private List<CharacterTemplate> CreateCharacterTemplatesList(List<CharacterTemplate> entities, List<Guid> guids)
		{
			var result = new List<CharacterTemplate>();
			if (guids != null)
				foreach (var x in guids)
					result.Add(entities.FirstOrDefault(y => y.Id == x));
			return result;
		}

		/// <summary>
		/// Создание списка шаблонов предмета
		/// </summary>
		/// <param name="entities">Данные из БД</param>
		/// <param name="guids">Список айди</param>
		/// <returns>Список шаблонов предмета</returns>
		private List<ItemTemplate> CreateItemTemplatesList(List<ItemTemplate> entities, List<Guid> guids)
		{
			var result = new List<ItemTemplate>();
			if (guids != null)
				foreach (var x in guids)
					result.Add(entities.FirstOrDefault(y => y.Id == x));
			return result;
		}
	}
}
