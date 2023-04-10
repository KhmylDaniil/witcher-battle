using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.AbilityRequests;
using Witcher.Core.Entities;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Exceptions.RequestExceptions;
using System;

namespace Witcher.Core.Requests.AbilityRequests
{
	/// <summary>
	/// Обработчик создания способности
	/// </summary>
	public class CreateAbilityHandler : BaseHandler<CreateAbilityCommand, Guid>
	{
		public CreateAbilityHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService) { }

		/// <summary>
		/// Обработчик создания способности
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Способность</returns>
		public override async Task<Guid> Handle(CreateAbilityCommand request, CancellationToken cancellationToken)
		{

			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Abilities)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			var newAbility = Ability.CreateAbility(
				game: game,
				name: request.Name,
				description: request.Description,
				attackDiceQuantity: request.AttackDiceQuantity,
				damageModifier: request.DamageModifier,
				attackSpeed: request.AttackSpeed,
				accuracy: request.Accuracy,
				attackSkill: request.AttackSkill,
				defensiveSkills: request.DefensiveSkills,
				damageType: request.DamageType,
				appliedConditions: request.AppliedConditions);

			_appDbContext.Abilities.Add(newAbility);
			await _appDbContext.SaveChangesAsync(cancellationToken);
			return newAbility.Id;
		}
	}
}
