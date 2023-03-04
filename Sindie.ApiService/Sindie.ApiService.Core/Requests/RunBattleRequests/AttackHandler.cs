using Microsoft.EntityFrameworkCore;
using Sindie.ApiService.Core.Abstractions;
using Sindie.ApiService.Core.Contracts.RunBattleRequests;
using Sindie.ApiService.Core.Entities;
using Sindie.ApiService.Core.Exceptions.EntityExceptions;
using Sindie.ApiService.Core.Exceptions.RequestExceptions;
using Sindie.ApiService.Core.Exceptions;
using Sindie.ApiService.Core.Logic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Sindie.ApiService.Core.Requests.RunBattleRequests
{
	public class AttackHandler : BaseHandler<AttackCommand, AttackResult>
	{
		/// <summary>
		/// Бросок параметра
		/// </summary>
		private readonly IRollService _rollService;

		public AttackHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IRollService rollService)
			: base(appDbContext, authorizationService)
		{
			_rollService = rollService;
		}

		public override async Task<AttackResult> Handle(AttackCommand request, CancellationToken cancellationToken)
		{
			var battle = await _authorizationService.BattleMasterFilter(_appDbContext.Battles, request.BattleId)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.CreatureSkills)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.CreatureParts)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.Abilities)
					.ThenInclude(a => a.AppliedConditions)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.Abilities)
					.ThenInclude(a => a.DefensiveSkills)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.DamageTypeModifiers)
				.Include(i => i.Creatures)
					.ThenInclude(c => c.Effects)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new ExceptionNoAccessToEntity<Battle>();

			var attackData = CheckAndFormData(request, battle);

			var attack = new Attack(_rollService);

			var attackResult = attack.RunAttack(attackData, request.DamageValue, request.AttackValue, request.DefenseValue);

			Attack.RemoveDeadBodies(battle);

			battle.NextInitiative++;
			
			await _appDbContext.SaveChangesAsync(cancellationToken);

			return new AttackResult { Message = attackResult };
		}

		/// <summary>
		/// Проверка запроса и формирование данных
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="battle">Бой</param>
		/// <returns>Данные для расчета атаки</returns>
		private AttackData CheckAndFormData(AttackCommand request, Battle battle)
		{
			var attacker = battle.Creatures.FirstOrDefault(x => x.Id == request.Id)
				?? throw new ExceptionEntityNotFound<Creature>(request.Id);

			var target = battle.Creatures.FirstOrDefault(x => x.Id == request.TargetCreatureId)
				?? throw new ExceptionEntityNotFound<Creature>(request.TargetCreatureId);

			var aimedPart = request.CreaturePartId == null
				? null
				: target.CreatureParts.FirstOrDefault(x => x.Id == request.CreaturePartId)
					?? throw new ExceptionEntityNotFound<BodyTemplatePart>(request.CreaturePartId.Value);

			if (!attacker.Abilities.Any())
				throw new RequestValidationException($"У существа {attacker.Name} отсутствуют способности, атака невозможна.");

			var ability = request.AbilityId == null
				? attacker.DefaultAbility()
				: attacker.Abilities.FirstOrDefault(x => x.Id == request.AbilityId)
					?? throw new ExceptionEntityNotFound<Ability>(request.AbilityId.Value);

			if (ability == null && request.DefensiveSkill != null)
				throw new RequestFieldIncorrectDataException<AttackCommand>(nameof(request.DefensiveSkill), null);

			var defensiveSkill = request.DefensiveSkill == null
				? null
				: target.CreatureSkills.FirstOrDefault(x => x.Skill == request.DefensiveSkill)
					?? throw new RequestFieldIncorrectDataException<AttackCommand>(nameof(request.DefensiveSkill));

			return AttackData.CreateData(
				attacker: attacker,
				target: target,
				aimedPart: aimedPart,
				ability: ability,
				defensiveSkill: defensiveSkill,
				specialToHit: request.SpecialToHit,
				specialToDamage: request.SpecialToDamage);
		}
	}
}
