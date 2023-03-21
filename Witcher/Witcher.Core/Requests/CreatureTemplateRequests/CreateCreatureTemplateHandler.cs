using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CreatureTemplateRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Witcher.Core.Requests.CreatureTemplateRequests
{
	/// <summary>
	/// Обработчик создания шаблона существа
	/// </summary>
	public class CreateCreatureTemplateHandler : BaseHandler<CreateCreatureTemplateCommand, CreatureTemplate>
	{
		public CreateCreatureTemplateHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService)
			: base(appDbContext, authorizationService)
		{
		}

		/// <summary>
		/// Создание шаблона существа
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Шаблон существа</returns>
		public override async Task<CreatureTemplate> Handle(CreateCreatureTemplateCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(x => x.BodyTemplates.Where(bt => bt.Id == request.BodyTemplateId))
					.ThenInclude(x => x.BodyTemplateParts)
				.Include(x => x.CreatureTemplates)
				.Include(x => x.Abilities)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			var imgFile = request.ImgFileId == null
				? null
				: await _appDbContext.ImgFiles.FirstOrDefaultAsync(x => x.Id == request.ImgFileId, cancellationToken)
				?? throw new EntityNotFoundException<ImgFile>(request.ImgFileId.Value);

			CheckRequest(request, game, out BodyTemplate bodyTemplate);

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

			newCreatureTemplate.UpdateAbililities(CreateAbilityList(request, game));

			if (request.CreatureTemplateSkills is not null)
				newCreatureTemplate.UpdateCreatureTemplateSkills(request.CreatureTemplateSkills);

			_appDbContext.CreatureTemplates.Add(newCreatureTemplate);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return newCreatureTemplate;
		}

		/// <summary>
		/// Проверка запроса
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="game">Игра</param>
		void CheckRequest(CreateCreatureTemplateCommand request, Game game, out BodyTemplate bodyTemplate)
		{
			bodyTemplate = game.BodyTemplates.FirstOrDefault(x => x.Id == request.BodyTemplateId)
				?? throw new EntityNotFoundException<BodyTemplate>(request.BodyTemplateId);

			if (game.CreatureTemplates.Any(x => string.Equals(x.Name, request.Name, StringComparison.Ordinal)))
				throw new RequestNameNotUniqException<CreateCreatureTemplateCommand>(nameof(request.Name));

			if (request.ArmorList is not null)
				foreach (var id in request.ArmorList.Select(x => x.BodyTemplatePartId))
					_ = bodyTemplate.BodyTemplateParts.FirstOrDefault(x => x.Id == id)
						?? throw new EntityNotFoundException<BodyTemplatePart>(id);

			if (request.Abilities is not null)
				foreach (var id in request.Abilities)
					_ = game.Abilities.FirstOrDefault(x => x.Id == id)
						?? throw new EntityNotFoundException<Ability>(id);
		}

		/// <summary>
		/// Создание списка частей шаблона тела
		/// </summary>
		/// <param name="bodyTemplate">Шаблон тела</param>
		/// <param name="data">Данные</param>
		/// <returns>Список частей шаблона тела</returns>
		List<(BodyTemplatePart BodyTemplatePart, int Armor)> CreateArmorList(BodyTemplate bodyTemplate, List<UpdateCreatureTemplateRequestArmorList> data)
		{
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
		List<Ability> CreateAbilityList(CreateCreatureTemplateCommand request, Game game)
		{
			var result = new List<Ability>();

			if (request.Abilities is null)
				return result;

			foreach (var id in request.Abilities)
				result.Add(game.Abilities.FirstOrDefault(x => x.Id == id));
			return result;
		}
	}
}
