using MediatR;
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
	/// Обработчик изменения шаблона существа
	/// </summary>
	public sealed class ChangeCreatureTemplateHandler : BaseHandler<ChangeCreatureTemplateCommand, Unit>
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
				.Include(x => x.CreatureTemplates)
					.ThenInclude(x => x.Abilities)
				.Include(x => x.Abilities)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			var imgFile = request.ImgFileId == null
				? null
				: await _appDbContext.ImgFiles.FirstOrDefaultAsync(x => x.Id == request.ImgFileId, cancellationToken)
				?? throw new EntityNotFoundException<ImgFile>(request.ImgFileId.Value);

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
				description: request.Description);

			if (request.Abilities is not null)
				creatureTemplate.UpdateAbililities(CreateAbilityList());

			await _appDbContext.SaveChangesAsync(cancellationToken);
			return Unit.Value;

			/// <summary>
			/// Проверка запроса
			/// </summary>
			/// <param name="request">Запрос</param>
			/// <param name="game">Игра</param>
			void CheckRequest(out CreatureTemplate creatureTemplate, out BodyTemplate bodyTemplate)
			{
				creatureTemplate = game.CreatureTemplates.Find(x => x.Id == request.Id)
					?? throw new EntityNotFoundException<CreatureTemplate>(request.Id);

				if (game.CreatureTemplates.Exists(x => string.Equals(x.Name, request.Name, StringComparison.Ordinal) && x.Id != request.Id))
					throw new RequestNameNotUniqException<ChangeCreatureTemplateCommand>(nameof(request.Name));

				bodyTemplate = game.BodyTemplates.Find(x => x.Id == request.BodyTemplateId)
					?? throw new EntityNotFoundException<BodyTemplate>(request.BodyTemplateId);

				if (request.Abilities is not null)
					foreach (var id in request.Abilities)
						_ = game.Abilities.Find(x => x.Id == id)
							?? throw new EntityNotFoundException<Ability>(id);
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
					result.Add(game.Abilities.Find(x => x.Id == id));
				return result;
			}
		}
	}
}
