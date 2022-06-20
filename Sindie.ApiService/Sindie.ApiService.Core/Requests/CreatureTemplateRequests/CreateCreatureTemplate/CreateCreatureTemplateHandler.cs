using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.CreateCreatureTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.CreatureTemplateRequests.CreateCreatureTemplate
{
	/// <summary>
	/// Обработчик создания шаблона существа
	/// </summary>
	public class CreateCreatureTemplateHandler : IRequestHandler<CreateCreatureTemplateCommand>
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
		public CreateCreatureTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Создание шаблона существа
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Шаблон существа</returns>
		public async Task<Unit> Handle(CreateCreatureTemplateCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(x => x.BodyTemplates.Where(bt => bt.Id == request.BodyTemplateId))
					.ThenInclude(x => x.BodyTemplateParts)
					.ThenInclude(x => x.BodyPartType)
				.Include(x => x.Conditions)
				.Include(x => x.Parameters)
				.Include(x => x.CreatureTemplates)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			var imgFile = request.ImgFileId == null
				? null
				: await _appDbContext.ImgFiles.FirstOrDefaultAsync(x => x.Id == request.ImgFileId, cancellationToken)
				?? throw new ExceptionEntityNotFound<ImgFile>(request.ImgFileId.Value);

			CheckRequest(request, game);

			var bodyTemplate = game.BodyTemplates.FirstOrDefault(x => x.Id == request.BodyTemplateId);


			var newCreatureTemplate = new CreatureTemplate(
				game: game,
				imgFile: imgFile,
				bodyTemplate: bodyTemplate,
				hp: request.HP,
				sta: request.Sta,
				@int: request.Int,
				@ref: request.Ref,
				dex: request.Dex,
				body: request.Body,
				emp: request.Emp,
				cra: request.Cra,
				will: request.Will,
				speed: request.Speed,
				luck: request.Luck,
				name: request.Name,
				description: request.Description,
				type: request.Type,
				armorList: CreateArmorList(bodyTemplate, request.ArmorList));

			newCreatureTemplate.UpdateAlibilities(AbilityData.CreateAbilityData(request, game));
			newCreatureTemplate.UpdateCreatureTemplateParameters(
				CreatureTemplateParameterData.CreateCreatureTemplateParameterData(request, game));

			_appDbContext.CreatureTemplates.Add(newCreatureTemplate);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		/// <summary>
		/// Проверка запроса
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		private void CheckRequest(CreateCreatureTemplateCommand request, Game game)
		{
			if (game.CreatureTemplates.Any(x => x.Name == request.Name))
				throw new ExceptionRequestNameNotUniq<CreateCreatureTemplateCommand>(nameof(request.Name));

			var bodyTemplate = game.BodyTemplates.FirstOrDefault(x => x.Id == request.BodyTemplateId)
				?? throw new ExceptionEntityNotFound<BodyTemplate>(request.BodyTemplateId);

			foreach (var item in request.ArmorList)
			{
				_ = bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Id == item.BodyTemplatePartId)
					?? throw new ExceptionEntityNotFound<BodyPartType>(item.BodyTemplatePartId);
				
				if (item.Armor < 0)
					throw new ExceptionRequestFieldIncorrectData<CreateCreatureTemplateCommand>(nameof(item.Armor));
			}

			foreach (var parameter in request.CreatureTemplateParameters)
			{
				_ = game.Parameters.FirstOrDefault(x => x.Id == parameter.ParameterId)
					?? throw new ExceptionEntityNotFound<Parameter>(parameter.ParameterId);

				if (parameter.Value < 1)
					throw new ExceptionRequestFieldIncorrectData<CreateCreatureTemplateCommand>(nameof(parameter.Value));
			}

			foreach (var ability in request.Abilities)
			{
				if (string.IsNullOrEmpty(ability.Name))
					throw new ExceptionRequestFieldNull<CreateCreatureTemplateRequestAbility>(nameof(ability.Name));

				_ = game.Parameters.FirstOrDefault(x => x.Id == ability.AttackParameterId)
					?? throw new ExceptionEntityNotFound<Parameter>(ability.AttackParameterId);

				if (ability.AttackDiceQuantity < 0)
					throw new ExceptionRequestFieldIncorrectData<CreateCreatureTemplateRequestAbility>(nameof(ability.AttackDiceQuantity));

				if (ability.AttackSpeed < 0)
					throw new ExceptionRequestFieldIncorrectData<CreateCreatureTemplateRequestAbility>(nameof(ability.AttackSpeed));

				foreach (var appliedCondition in ability.AppliedConditions)
				{
					_ = game.Conditions.FirstOrDefault(x => x.Id == appliedCondition.ConditionId)
						?? throw new ExceptionEntityNotFound<Condition>(appliedCondition.ConditionId);

					if (appliedCondition.ApplyChance < 0 || appliedCondition.ApplyChance > 100)
						throw new ExceptionRequestFieldIncorrectData<CreateCreatureTemplateCommand>(nameof(appliedCondition.ApplyChance));
				}
			}
		}

		/// <summary>
		/// Созадние списка частей шаблона тела
		/// </summary>
		/// <param name="bodyTemplate">Шаблон тела</param>
		/// <param name="data">Данные</param>
		/// <returns>Список частей шаблона тела</returns>
		private List<(BodyTemplatePart BodyTemplatePart, int Armor)> CreateArmorList(BodyTemplate bodyTemplate, List<CreateCreatureTemplateRequestArmorList> data)
		{
			var result = new List<(BodyTemplatePart BodyTemplatePart, int Armor)>();
			foreach (var item in data)
				result.Add((
					bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Id == item.BodyTemplatePartId),
					item.Armor));
			return result;
		}
	}
}
