﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.CreateCreatureTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using Sindie.ApiService.Core.Requests.CreatureTemplateRequests.ChangeCreatureTemplate;
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
				.Include(x => x.CreatureTemplates)
				.Include(x => x.Abilities)
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
				creatureType: request.CreatureType,
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

			newCreatureTemplate.UpdateAlibilities(CreateAbilityList(request, game));
			newCreatureTemplate.UpdateCreatureTemplateSkills(
				CreatureTemplateSkillData.CreateCreatureTemplateSkillData(request));

			_appDbContext.CreatureTemplates.Add(newCreatureTemplate);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;
		}

		/// <summary>
		/// Проверка запроса
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		/// <param name="creatureTypes">Типы существ</param>
		/// <param name="skills">Навыки</param>
		private void CheckRequest(CreateCreatureTemplateCommand request, Game game)
		{
			if (game.CreatureTemplates.Any(x => string.Equals(x.Name, request.Name, StringComparison.Ordinal)))
				throw new RequestNameNotUniqException<CreateCreatureTemplateCommand>(nameof(request.Name));

			var bodyTemplate = game.BodyTemplates.FirstOrDefault(x => x.Id == request.BodyTemplateId)
				?? throw new ExceptionEntityNotFound<BodyTemplate>(request.BodyTemplateId);

			foreach (var item in request.ArmorList)
			{
				_ = bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Id == item.BodyTemplatePartId)
					?? throw new ExceptionEntityNotFound<BodyTemplatePart>(item.BodyTemplatePartId);
				
				if (item.Armor < 0)
					throw new RequestFieldIncorrectDataException<CreateCreatureTemplateCommand>(nameof(item.Armor));
			}

			foreach (var skill in request.CreatureTemplateSkills)
			{
				if (!Enum.IsDefined(skill.Skill))
					throw new RequestFieldIncorrectDataException<CreateCreatureTemplateCommand>(nameof(skill.Skill));

				if (skill.Value < 0 || skill.Value > BaseData.DiceValue.Value)
					throw new RequestFieldIncorrectDataException<CreateCreatureTemplateCommand>(nameof(skill.Value));
			}

			foreach (var id in request.Abilities)
				_ = game.Abilities.FirstOrDefault(x => x.Id == id)
					?? throw new ExceptionEntityNotFound<Ability>(id);
		}

		/// <summary>
		/// Создание списка частей шаблона тела
		/// </summary>
		/// <param name="bodyTemplate">Шаблон тела</param>
		/// <param name="data">Данные</param>
		/// <returns>Список частей шаблона тела</returns>
		private List<(BodyTemplatePart BodyTemplatePart, int Armor)> CreateArmorList(BodyTemplate bodyTemplate, List<CreateCreatureTemplateRequestArmorList> data)
		{
			var result = new List<(BodyTemplatePart BodyTemplatePart, int Armor)>();
			foreach (var item in bodyTemplate.BodyTemplateParts)
			{
				var correspondingPart = data.FirstOrDefault(x => x.BodyTemplatePartId == item.Id);
				
				var armor = correspondingPart == null ? 0 : correspondingPart.Armor;

				result.Add((item, armor));
			}
			return result;
		}

		/// <summary>
		/// Создать список способностей
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		/// <returns>Список способностей</returns>
		private List<Ability> CreateAbilityList(CreateCreatureTemplateCommand request, Game game)
		{
			var result = new List<Ability>();

			foreach (var id in request.Abilities)
				result.Add(game.Abilities.FirstOrDefault(x => x.Id == id));
			return result;
		}
	}
}
