using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CharacterRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.CharacterRequests
{
	public sealed class CreateCharacterHandler : BaseHandler<CreateCharacterCommand, Character>
	{
		private readonly IUserContext _userContext;

		public CreateCharacterHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IUserContext userContext)
			: base(appDbContext, authorizationService)
		{
			_userContext = userContext;
		}

		public async override Task<Character> Handle(CreateCharacterCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.UserGameFilter(_appDbContext.Games)
				.Include(g => g.Characters)
				.Include(g => g.UserGames)
				.Include(g => g.CreatureTemplates.Where(ct => ct.Id == request.CreatureTemplateId))
					.ThenInclude(ct => ct.CreatureTemplateParts)
				.Include(g => g.CreatureTemplates.Where(ct => ct.Id == request.CreatureTemplateId))
					.ThenInclude(ct => ct.CreatureTemplateSkills)
				.Include(g => g.CreatureTemplates.Where(ct => ct.Id == request.CreatureTemplateId))
					.ThenInclude(ct => ct.Abilities)
				.Include(g => g.CreatureTemplates.Where(ct => ct.Id == request.CreatureTemplateId))
					.ThenInclude(ct => ct.DamageTypeModifiers)
			.FirstOrDefaultAsync(cancellationToken)
				?? throw new NoAccessToEntityException<Game>();

			if (game.Characters.Exists(ct => ct.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
				throw new RequestNameNotUniqException<CreateCharacterHandler>(request.Name);

			var creatureTemplate = game.CreatureTemplates.Find(x => x.Id == request.CreatureTemplateId)
				?? throw new EntityNotFoundException<CreatureTemplate>(request.CreatureTemplateId);

			var newCharacter = new Character(game, creatureTemplate, null, request.Name, request.Description);
			newCharacter.AddUserGameCharacters(game, _userContext.CurrentUserId);

			_appDbContext.Characters.Add(newCharacter);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return newCharacter;
		}
	}
}
