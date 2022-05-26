using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.BaseData;
using Sindie.ApiService.Core.Contracts.ModifierRequests.ChangeModifier;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.ModifierRequests.ChangeModifier
{
	/// <summary>
	/// Обработчик изменения модификатора
	/// </summary>
	public class ChangeModifierHandler: IRequestHandler<ChangeModifierCommand, Unit>
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
		/// Конструктор обработчика изменения модификатора
		/// </summary>
		/// <param name="appDbContext">Контекст базы данных</param>
		/// <param name="authorizationService">Сервис авторизации</param>
		public ChangeModifierHandler(
			IAppDbContext appDbContext,
			IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Изменение модификатора
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Юнит</returns>
		public async Task<Unit> Handle(ChangeModifierCommand request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ExceptionRequestNull<ChangeModifierCommand>();

			var game = await _authorizationService.RoleGameFilter(
				_appDbContext.Games, request.GameId, GameRoles.MasterRoleId)
				.Include(x => x.Modifiers.Where(m => m.Id == request.ModifierId))
					.ThenInclude(y => y.CharacterTemplateModifiers)
				.Include(x => x.Modifiers.Where(m => m.Id == request.ModifierId))
					.ThenInclude(y => y.ItemTemplateModifiers)
				.Include(x => x.Modifiers.Where(m => m.Id == request.ModifierId))
					.ThenInclude(y => y.ModifierScripts)
				.Include(x => x.CharacterTemplates)
				.Include(x => x.ItemTemplates)
				.Include(x => x.Events)
				.Include(x => x.Scripts)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new ExceptionNoAccessToEntity<Game>();

			var modifier = game.Modifiers.FirstOrDefault(x => x.Id == request.ModifierId)
				?? throw new ExceptionEntityNotFound<Modifier>(request.ModifierId);

			if (string.IsNullOrWhiteSpace(request.Name))
				throw new ExceptionRequestFieldNull<ChangeModifierCommand>($"{nameof(request.Name)}");

			if (request.Name != modifier.Name && game.Modifiers
				.Any(x => x.Name == request.Name))
				throw new ExceptionRequestNameNotUniq<ChangeModifierCommand>(nameof(request.Name));

			var imgFile = request.ImgFileId == null
				? null
				: await _appDbContext.ImgFiles
				.FirstOrDefaultAsync(x => x.Id == request.ImgFileId, cancellationToken)
				?? throw new ExceptionEntityNotFound<ImgFile>((Guid)request.ImgFileId);

			TryRelatedEntities(game, request);

			modifier.ChangeModifier(
				name: request.Name,
				description: request.Description,
				imgFile: imgFile);

			modifier.ChangeCharacterTemplateModifiersList(
				CreateCharacterTemplatesList(game.CharacterTemplates, request.CharacterTemplates));
			modifier.ChangeItemTemplateModifiersList(
				CreateItemTemplatesList(game.ItemTemplates, request.ItemTemplates));
			modifier.ChangeModifierScriptsList(
				CreateModifierScriptsData(request, game));

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		/// <summary>
		/// Получение данных для создания списка скриптов модификатора
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		/// <returns>Список данных</returns>
		private List<(Script Script,
				Event Event,
				Guid Id,
				DateTime ActivationTime,
				int PeriodOfActivity,
				int PeriodOfInactivity,
				int NumberOfRepetitions)>
			CreateModifierScriptsData(ChangeModifierCommand request, Game game)
		{
			var result = new List<(Script script,
				Event @event,
				Guid id,
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
						id: requestItem.Id,
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
		private static void TryRelatedEntities(Game game, ChangeModifierCommand request)
		{
			if (request.CharacterTemplates == null)
				throw new ExceptionRequestFieldNull<ChangeModifierCommand>(nameof(request.CharacterTemplates));
			if (request.ItemTemplates == null)
				throw new ExceptionRequestFieldNull<ChangeModifierCommand>(nameof(request.ItemTemplates));
			if (request.ModifierScripts == null)
				throw new ExceptionRequestFieldNull<ChangeModifierCommand>(nameof(request.ModifierScripts));

			if (request.CharacterTemplates.Any())
				foreach (var x in request.CharacterTemplates)
					_ = game.CharacterTemplates.FirstOrDefault(y => y.Id == x)
						?? throw new ExceptionEntityNotFound<CharacterTemplate>(x);

			if (request.ItemTemplates.Any())
				foreach (var x in request.ItemTemplates)
					_ = game.ItemTemplates.FirstOrDefault(y => y.Id == x)
						?? throw new ExceptionEntityNotFound<ItemTemplate>(x);

			if (request.ModifierScripts.Any())
				foreach (var requestItem in request.ModifierScripts)
				{
					_ = game.Scripts.FirstOrDefault(y => y.Id == requestItem.ScriptId)
							?? throw new ExceptionEntityNotFound<Script>(requestItem.ScriptId);
					if (requestItem.EventId != default)
						_ = game.Events.FirstOrDefault(y => y.Id == requestItem.EventId)
							?? throw new ExceptionEntityNotFound<Event>((Guid)requestItem.EventId);
					if (requestItem.Id != default)
						_ = game.Modifiers
						.SelectMany(y => y.ModifierScripts)
						.FirstOrDefault(y => y.Id == requestItem.Id)
							?? throw new ExceptionEntityNotFound<ModifierScript>(requestItem.Id);
					if (requestItem.PeriodOfActivity < 0)
						throw new ExceptionRequestFieldIncorrectData<ChangeModifierCommandItem>(nameof(requestItem.PeriodOfActivity));
					if (requestItem.PeriodOfInactivity < 0)
						throw new ExceptionRequestFieldIncorrectData<ChangeModifierCommandItem>(nameof(requestItem.PeriodOfInactivity));
					if (requestItem.NumberOfRepetitions < 0)
						throw new ExceptionRequestFieldIncorrectData<ChangeModifierCommandItem>(nameof(requestItem.NumberOfRepetitions));
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
