using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.ChangeCreatureTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.CreatureTemplateRequests.ChangeCreatureTemplate
{
	/// <summary>
	/// Обработчик изменения шаблона существа
	/// </summary>
	public class ChangeCreatureTemplateHandler : IRequestHandler<ChangeCreatureTemplateCommand>
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
		/// Конструктор обработчика изменения шаблона существа
		/// </summary>
		/// <param name="appDbContext"></param>
		/// <param name="authorizationService"></param>
		public ChangeCreatureTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
		{
			_appDbContext = appDbContext;
			_authorizationService = authorizationService;
		}

		/// <summary>
		/// Изменение шаблона существа
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Шаблон существа</returns>
		public async Task<Unit> Handle(ChangeCreatureTemplateCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.RoleGameFilter(_appDbContext.Games, request.GameId, BaseData.GameRoles.MasterRoleId)
				.Include(x => x.BodyTemplates.Where(bt => bt.Id == request.BodyTemplateId))
					.ThenInclude(x => x.BodyTemplateParts)
					.ThenInclude(x => x.BodyPartType)
				.Include(x => x.Conditions)
				.Include(x => x.Parameters)
				.Include(x => x.CreatureTemplates)
					.ThenInclude(x => x.CreatureType)
				.Include(x => x.CreatureTemplates)
					.ThenInclude(x => x.CreatureTemplateParameters)
				.Include(x => x.CreatureTemplates)
					.ThenInclude(x => x.Abilities)
						.ThenInclude(x => x.AppliedConditions)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			var imgFile = request.ImgFileId == null
				? null
				: await _appDbContext.ImgFiles.FirstOrDefaultAsync(x => x.Id == request.ImgFileId, cancellationToken)
				?? throw new ExceptionEntityNotFound<ImgFile>(request.ImgFileId.Value);

			var creatureTypes = await _appDbContext.CreatureTypes.ToListAsync(cancellationToken);

			CheckRequest(request, game, creatureTypes);

			var bodyTemplate = game.BodyTemplates.FirstOrDefault(x => x.Id == request.BodyTemplateId);
			var creatureTemplate = game.CreatureTemplates.FirstOrDefault(x => x.Id == request.Id);

			creatureTemplate.ChangeCreatureTemplate(
				game: game,
				imgFile: imgFile,
				bodyTemplate: bodyTemplate,
				creatureType: creatureTypes.FirstOrDefault(x => x.Id == request.CreatureTypeId),
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
				armorList: CreateArmorList(bodyTemplate, request.ArmorList));

			creatureTemplate.UpdateAlibilities(AbilityData.CreateAbilityData(request, game));

			creatureTemplate.UpdateCreatureTemplateParameters(
				CreatureTemplateParameterData.CreateCreatureTemplateParameterData(request, game));

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		/// <summary>
		/// Проверка запроса
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		/// <param name="creatureTypes">Типы существ</param>
		private void CheckRequest(ChangeCreatureTemplateCommand request, Game game, List<CreatureType> creatureTypes)
		{
			var creatureTemplate = game.CreatureTemplates.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<CreatureTemplate>(request.Id);

			if (game.CreatureTemplates.Any(x => x.Name == request.Name && x.Id != request.Id))
				throw new ExceptionRequestNameNotUniq<ChangeCreatureTemplateCommand>(nameof(request.Name));

			var bodyTemplate = game.BodyTemplates.FirstOrDefault(x => x.Id == request.BodyTemplateId)
				?? throw new ExceptionEntityNotFound<BodyTemplate>(request.BodyTemplateId);

			_ = creatureTypes.FirstOrDefault(x => x.Id == request.CreatureTypeId)
				?? throw new ExceptionEntityNotFound<CreatureType>(request.CreatureTypeId);

			foreach (var item in request.ArmorList)
			{
				_ = bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Id == item.BodyTemplatePartId)
					?? throw new ExceptionEntityNotFound<BodyPartType>(item.BodyTemplatePartId);

				if (item.Armor < 0)
					throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateCommand>(nameof(item.Armor));
			}

			foreach (var parameter in request.CreatureTemplateParameters)
			{
				_ = game.Parameters.FirstOrDefault(x => x.Id == parameter.ParameterId)
					?? throw new ExceptionEntityNotFound<Parameter>(parameter.ParameterId);

				if (parameter.Id != default)
					_ = creatureTemplate.CreatureTemplateParameters
						.FirstOrDefault(x => x.Id == parameter.Id && x.ParameterId == parameter.ParameterId)
						?? throw new ExceptionEntityNotFound<CreatureTemplateParameter>(parameter.Id.Value);
				else
					if (creatureTemplate.CreatureTemplateParameters.Any(x => x.ParameterId == parameter.ParameterId))
						throw new ExceptionRequestNotUniq<CreatureTemplateParameter>(parameter.ParameterId);

				if (parameter.Value < 1)
					throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateCommand>(nameof(parameter.Value));
			}

			foreach (var ability in request.Abilities)
			{
				if (ability.Id != default)
					_ = creatureTemplate.Abilities.FirstOrDefault(x => x.Id == ability.Id)
							?? throw new ExceptionEntityNotFound<Ability>(ability.Id.Value);

				if (string.IsNullOrEmpty(ability.Name))
					throw new ExceptionRequestFieldNull<ChangeCreatureTemplateCommand>(nameof(ability.Name));

				_ = game.Parameters.FirstOrDefault(x => x.Id == ability.AttackParameterId)
					?? throw new ExceptionEntityNotFound<Parameter>(ability.AttackParameterId);

				if (ability.AttackDiceQuantity < 0)
					throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateCommand>(nameof(ability.AttackDiceQuantity));

				if (ability.AttackSpeed < 0)
					throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateCommand>(nameof(ability.AttackSpeed));

				foreach (var appliedCondition in ability.AppliedConditions)
				{
					if (appliedCondition.Id != default)
						_ = creatureTemplate.Abilities.SelectMany(x => x.AppliedConditions).FirstOrDefault(x => x.Id == appliedCondition.Id)
							?? throw new ExceptionEntityNotFound<AppliedCondition>(appliedCondition.Id.Value);

					_ = game.Conditions.FirstOrDefault(x => x.Id == appliedCondition.ConditionId)
						?? throw new ExceptionEntityNotFound<Condition>(appliedCondition.ConditionId);

					if (appliedCondition.ApplyChance < 0 || appliedCondition.ApplyChance > 100)
						throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateCommand>(nameof(appliedCondition.ApplyChance));
				}
			}
		}

		/// <summary>
		/// Создание списка частей шаблона тела
		/// </summary>
		/// <param name="bodyTemplate">Шаблон тела</param>
		/// <param name="data">Данные</param>
		/// <returns>Список частей шаблона тела</returns>
		private List<(BodyTemplatePart BodyTemplatePart, int Armor)> CreateArmorList(BodyTemplate bodyTemplate, List<ChangeCreatureTemplateRequestArmorList> data)
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
