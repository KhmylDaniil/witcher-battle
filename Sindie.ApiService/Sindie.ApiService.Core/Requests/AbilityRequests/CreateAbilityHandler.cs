using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.AbilityRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.AbilityRequests
{
	/// <summary>
	/// Обработчик создания способности
	/// </summary>
	public class CreateAbilityHandler : BaseHandler<CreateAbilityCommand, Ability>
	{
		public CreateAbilityHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService) : base(appDbContext, authorizationService) { }

		/// <summary>
		/// Обработчик создания способности
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <returns>Способность</returns>
		public override async Task<Ability> Handle(CreateAbilityCommand request, CancellationToken cancellationToken)
		{

			var game = await _authorizationService.AuthorizedGameFilter(_appDbContext.Games)
				.Include(g => g.Abilities)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Game>();

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
			return newAbility;
		}
	}
}
