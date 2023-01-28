using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests.ChangeCreatureTemplate;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using Sindie.ApiService.Core.Requests.BodyTemplateRequests.ChangeBodyTemplate;
using System;
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
				.Include(x => x.CreatureTemplates)
				.Include(x => x.CreatureTemplates)
					.ThenInclude(x => x.CreatureTemplateSkills)
				.Include(x => x.CreatureTemplates)
					.ThenInclude(x => x.Abilities)
				.Include(x => x.Abilities)
					.ThenInclude(x => x.AppliedConditions)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

			var imgFile = request.ImgFileId == null
				? null
				: await _appDbContext.ImgFiles.FirstOrDefaultAsync(x => x.Id == request.ImgFileId, cancellationToken)
				?? throw new ExceptionEntityNotFound<ImgFile>(request.ImgFileId.Value);

			var skills = await _appDbContext.Skills.ToListAsync(cancellationToken);

			CheckRequest(request, game, skills);

			var bodyTemplate = game.BodyTemplates.FirstOrDefault(x => x.Id == request.BodyTemplateId);
			var creatureTemplate = game.CreatureTemplates.FirstOrDefault(x => x.Id == request.Id);

			creatureTemplate.ChangeCreatureTemplate(
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

			creatureTemplate.UpdateAlibilities(CreateAbilityList(request, game));

			creatureTemplate.UpdateCreatureTemplateSkills(
				CreatureTemplateSkillData.CreateCreatureTemplateSkillData(request, skills));

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
		private void CheckRequest(ChangeCreatureTemplateCommand request, Game game, List<Skill> skills)
		{
			var creatureTemplate = game.CreatureTemplates.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<CreatureTemplate>(request.Id);

			if (game.CreatureTemplates.Any(x => string.Equals(x.Name, request.Name, System.StringComparison.Ordinal) && x.Id != request.Id))
				throw new ExceptionRequestNameNotUniq<ChangeCreatureTemplateCommand>(nameof(request.Name));

			var bodyTemplate = game.BodyTemplates.FirstOrDefault(x => x.Id == request.BodyTemplateId)
				?? throw new ExceptionEntityNotFound<BodyTemplate>(request.BodyTemplateId);

			foreach (var item in request.ArmorList)
			{
				_ = bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Id == item.BodyTemplatePartId)
					?? throw new ExceptionEntityNotFound<BodyTemplatePart>(item.BodyTemplatePartId);

				if (item.Armor < 0)
					throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateCommand>(nameof(item.Armor));
			}

			foreach (var skill in request.CreatureTemplateSkills)
			{
				_ = skills.FirstOrDefault(x => x.Id == skill.SkillId)
					?? throw new ExceptionEntityNotFound<Skill>(skill.SkillId);

				if (skill.Id != default)
					_ = creatureTemplate.CreatureTemplateSkills
						.FirstOrDefault(x => x.Id == skill.Id && x.SkillId == skill.SkillId)
						?? throw new ExceptionEntityNotFound<CreatureTemplateSkill>(skill.Id.Value);
				else
					if (creatureTemplate.CreatureTemplateSkills.Any(x => x.SkillId == skill.SkillId))
						throw new ExceptionRequestNotUniq<CreatureTemplateSkill>(skill.SkillId);

				if (skill.Value < 0)
					throw new ExceptionRequestFieldIncorrectData<ChangeCreatureTemplateCommand>(nameof(skill.Value));
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
		private List<(BodyTemplatePart BodyTemplatePart, int Armor)> CreateArmorList(BodyTemplate bodyTemplate, List<ChangeCreatureTemplateRequestArmorList> data)
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
		private List<Ability> CreateAbilityList(ChangeCreatureTemplateCommand request, Game game)
		{
			var result = new List<Ability>();

			foreach (var id in request.Abilities)
				result.Add(game.Abilities.FirstOrDefault(x => x.Id == id));
			return result;
		}
	}
}
