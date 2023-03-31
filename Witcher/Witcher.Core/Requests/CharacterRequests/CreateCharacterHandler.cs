using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.CharacterRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;

namespace Witcher.Core.Requests.CharacterRequests
{
	public class CreateCharacterHandler : BaseHandler<CreateCharacterCommand, Character>
	{
		private readonly IUserContext _userContext;

		public CreateCharacterHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IUserContext userContext)
			: base(appDbContext, authorizationService)
		{
			_userContext = userContext;
		}

		public async override Task<Character> Handle(CreateCharacterCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Characters)
				.Include(g => g.UserGames.Where(ug => ug.UserId == _userContext.CurrentUserId))
				.Include(g => g.CreatureTemplates.Where(ct => ct.Id == request.CreatureTemplateId))
					.ThenInclude(ct => ct.CreatureTemplateParts)
				.Include(g => g.CreatureTemplates.Where(ct => ct.Id == request.CreatureTemplateId))
					.ThenInclude(ct => ct.CreatureTemplateSkills)
			.FirstOrDefaultAsync(cancellationToken)
				?? throw new NoAccessToEntityException<Game>();

			if (game.Characters.Any(ct => ct.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
				throw new RequestNameNotUniqException<CreateCharacterHandler>(request.Name);

			var creatureTemplate = game.CreatureTemplates.FirstOrDefault(x => x.Id == request.CreatureTemplateId)
				?? throw new EntityNotFoundException<CreatureTemplate>(request.CreatureTemplateId);

			var userGame = game.UserGames.FirstOrDefault(x => x.UserId == _userContext.CurrentUserId);

			var newCharacter = new Character(game, creatureTemplate, null, request.Name, request.Description, userGame);

			_appDbContext.Characters.Add(newCharacter);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return newCharacter;
		}
	}
}
