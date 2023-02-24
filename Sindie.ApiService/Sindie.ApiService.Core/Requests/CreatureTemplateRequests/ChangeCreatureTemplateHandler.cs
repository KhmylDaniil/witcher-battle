using MediatR;
using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.CreatureTemplateRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.CreatureTemplateRequests
{
	/// <summary>
	/// Обработчик изменения шаблона существа
	/// </summary>
	public class ChangeCreatureTemplateHandler : BaseHandler<ChangeCreatureTemplateCommand, Unit>
	{
		public ChangeCreatureTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService)
		{
		}

		/// <summary>
		/// Изменение шаблона существа
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Шаблон существа</returns>
		public override async Task<Unit> Handle(ChangeCreatureTemplateCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(x => x.BodyTemplates.Where(bt => bt.Id == request.BodyTemplateId))
					.ThenInclude(x => x.BodyTemplateParts)
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

			CheckRequest(out CreatureTemplate creatureTemplate, out BodyTemplate bodyTemplate);

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
				armorList: CreateArmorList(request.ArmorList));

			if (request.Abilities is not null)
				creatureTemplate.UpdateAbililities(CreateAbilityList());

			if (request.CreatureTemplateSkills is not null)
				creatureTemplate.UpdateCreatureTemplateSkills(request.CreatureTemplateSkills);

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;

			/// <summary>
			/// Проверка запроса
			/// </summary>
			/// <param name="request">Запрос</param>
			/// <param name="game">Игра</param>
			void CheckRequest(out CreatureTemplate creatureTemplate, out BodyTemplate bodyTemplate)
			{
				creatureTemplate = game.CreatureTemplates.FirstOrDefault(x => x.Id == request.Id)
					?? throw new ExceptionEntityNotFound<CreatureTemplate>(request.Id);

				if (game.CreatureTemplates.Any(x => string.Equals(x.Name, request.Name, StringComparison.Ordinal) && x.Id != request.Id))
					throw new RequestNameNotUniqException<ChangeCreatureTemplateCommand>(nameof(request.Name));

				bodyTemplate = game.BodyTemplates.FirstOrDefault(x => x.Id == request.BodyTemplateId)
					?? throw new ExceptionEntityNotFound<BodyTemplate>(request.BodyTemplateId);

				if (request.ArmorList is not null)
					foreach (var id in request.ArmorList.Select(x => x.BodyTemplatePartId))
						_ = bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Id == id)
							?? throw new ExceptionEntityNotFound<BodyTemplatePart>(id);

				if (request.Abilities is not null)
					foreach (var id in request.Abilities)
						_ = game.Abilities.FirstOrDefault(x => x.Id == id)
							?? throw new ExceptionEntityNotFound<Ability>(id);

				if (request.CreatureTemplateSkills is null)
					return;

				foreach (var skill in request.CreatureTemplateSkills)
				{
					if (skill.Id != default)
						_ = creatureTemplate.CreatureTemplateSkills
							.FirstOrDefault(x => x.Id == skill.Id && x.Skill == skill.Skill)
								?? throw new ExceptionEntityNotFound<CreatureTemplateSkill>(skill.Id.Value);
					else
						if (creatureTemplate.CreatureTemplateSkills.Any(x => x.Skill == skill.Skill))
						throw new RequestNotUniqException<CreatureTemplateSkill>(Enum.GetName(skill.Skill));
				}
			}

			/// <summary>
			/// Создание списка частей шаблона тела
			/// </summary>
			/// <param name="bodyTemplate">Шаблон тела</param>
			/// <param name="data">Данные</param>
			/// <returns>Список частей шаблона тела</returns>
			List<(BodyTemplatePart BodyTemplatePart, int Armor)> CreateArmorList(List<UpdateCreatureTemplateRequestArmorList> data)
			{
				if (data is null && bodyTemplate.Id == creatureTemplate.BodyTemplateId)
					return null;

				var result = new List<(BodyTemplatePart BodyTemplatePart, int Armor)>();

				foreach (var item in bodyTemplate.BodyTemplateParts)
				{
					var correspondingPart = data?.FirstOrDefault(x => x.BodyTemplatePartId == item.Id);

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
			List<Ability> CreateAbilityList()
			{
				var result = new List<Ability>();

				foreach (var id in request.Abilities)
					result.Add(game.Abilities.FirstOrDefault(x => x.Id == id));
				return result;
			}
		}
	}
}
