using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
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
			var creatureTemplate = game.CreatureTemplates.FirstOrDefault(x => x.Id == request.Id);

			creatureTemplate.ChangeCreatureTemplate(
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
				armorList: request.ArmorList);

			creatureTemplate.UpdateAlibilities(AbilityData.CreateAbilityData(request, game));

			creatureTemplate.UpdateCreatureTemplateParameters(
							CreateParameterList(game.Parameters, request.CreatureTemplateParameters));

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		/// <summary>
		/// Проверка запроса
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		private void CheckRequest(ChangeCreatureTemplateCommand request, Game game)
		{
			var creatureTemplate = game.CreatureTemplates.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<CreatureTemplate>(request.Id);

			if (game.CreatureTemplates.Any(x => x.Name == request.Name && x.Id != request.Id))
				throw new ExceptionRequestNameNotUniq<ChangeCreatureTemplateCommand>(nameof(request.Name));

			var bodyTemplate = game.BodyTemplates.FirstOrDefault(x => x.Id == request.BodyTemplateId)
				?? throw new ExceptionEntityNotFound<BodyTemplate>(request.BodyTemplateId);

			var bodyTemplatePartsList = bodyTemplate.BodyTemplateParts.Select(x => x.Name).ToList();

			foreach (var item in request.ArmorList)
				if(!bodyTemplatePartsList.Contains(item.BodyPartName)
					|| item.Armor < 0)
					throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateCommand>(nameof(request.ArmorList));

			foreach (var parameter in request.CreatureTemplateParameters)
			{
				_ = game.Parameters.FirstOrDefault(x => x.Id == parameter.ParameterId)
					?? throw new ExceptionEntityNotFound<Parameter>(parameter.ParameterId);

				if (parameter.Value < 1)
					throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateCommand>(nameof(parameter.Value));
			}

			foreach (var ability in request.Abilities)
			{
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
					_ = creatureTemplate.Abilities.SelectMany(x => x.AppliedConditions).FirstOrDefault(x => x.Id == appliedCondition.Id)
						?? throw new ExceptionEntityNotFound<AppliedCondition>(appliedCondition.Id);

					_ = game.Conditions.FirstOrDefault(x => x.Id == appliedCondition.ConditionId)
						?? throw new ExceptionEntityNotFound<Condition>(appliedCondition.ConditionId);

					if (appliedCondition.ApplyChance < 0 || appliedCondition.ApplyChance > 100)
						throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateCommand>(nameof(appliedCondition.ApplyChance));
				}
			}
		}

		/// <summary>
		/// Создание списка параметров
		/// </summary>
		/// <param name="entities">Данные из БД</param>
		/// <param name="data">Список айди</param>
		/// <returns>Список параметров</returns>
		private List<(Parameter Parameter, int Value)> CreateParameterList(List<Parameter> entities, List<(Guid ParameterId, int Value)> data)
		{
			var result = new List<(Parameter Parameter, int Value)>();
			foreach (var dataItem in data)
				result.Add((
					entities.FirstOrDefault(y => y.Id == dataItem.ParameterId),
					dataItem.Value));
			return result;
		}
	}
}
