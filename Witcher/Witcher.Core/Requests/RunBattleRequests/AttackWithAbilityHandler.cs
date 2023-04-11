﻿using Microsoft.EntityFrameworkCore;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;
using Witcher.Core.Exceptions;
using Witcher.Core.Logic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Witcher.Core.ExtensionMethods;
using System.ComponentModel.DataAnnotations;

namespace Witcher.Core.Requests.RunBattleRequests
{
	public class AttackWithAbilityHandler : BaseHandler<AttackWithAbilityCommand, Unit>
	{
		/// <summary>
		/// Бросок параметра
		/// </summary>
		private readonly IRollService _rollService;

		public AttackWithAbilityHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IRollService rollService)
			: base(appDbContext, authorizationService)
		{
			_rollService = rollService;
		}

		public override async Task<Unit> Handle(AttackWithAbilityCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.CharacterOwnerFilter(_appDbContext.Games, request.Id)
				.GetCreaturesAndCharactersFormBattle(request.BattleId)
				.FirstOrDefaultAsync(cancellationToken)
				?? throw new NoAccessToEntityException<Game>();

			var battle = game.Battles.FirstOrDefault()
				?? throw new EntityNotFoundException<Battle>(request.BattleId);

			var attackData = CheckAndFormData(request, battle);

			var attack = new AttackProcess(_rollService);

			var attackLog = attack.RunAttack(attackData, request.DamageValue, request.AttackValue, request.DefenseValue);

			battle.BattleLog += attackLog;

			RunBattle.RemoveDeadBodies(battle);
			TurnStateProcessing.EndOfAttackProcessing(attackData);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}

		/// <summary>
		/// Проверка запроса и формирование данных
		/// </summary>
		/// <param name="request">Запрос</param>
		/// <param name="battle">Бой</param>
		/// <returns>Данные для расчета атаки</returns>
		private AttackData CheckAndFormData(AttackWithAbilityCommand request, Battle battle)
		{
			var attacker = battle.Creatures.FirstOrDefault(x => x.Id == request.Id)
				?? throw new EntityNotFoundException<Creature>(request.Id);

			var target = battle.Creatures.FirstOrDefault(x => x.Id == request.TargetId)
				?? throw new EntityNotFoundException<Creature>(request.TargetId);

			var aimedPart = request.CreaturePartId == null
				? null
				: target.CreatureParts.FirstOrDefault(x => x.Id == request.CreaturePartId)
					?? throw new EntityNotFoundException<BodyTemplatePart>(request.CreaturePartId.Value);

			if (!attacker.Abilities.Any())
				throw new RequestValidationException($"У существа {attacker.Name} отсутствуют способности, атака невозможна.");

			var ability = request.AbilityId == null
				? attacker.DefaultAbility()
				: attacker.Abilities.FirstOrDefault(x => x.Id == request.AbilityId)
					?? throw new EntityNotFoundException<Ability>(request.AbilityId.Value);

			if (ability == null && request.DefensiveSkill != null)
				throw new RequestFieldIncorrectDataException<AttackWithAbilityCommand>(nameof(request.DefensiveSkill), null);

			var defensiveSkill = request.DefensiveSkill == null
				? null
				: target.CreatureSkills.FirstOrDefault(x => x.Skill == request.DefensiveSkill)
					?? throw new RequestFieldIncorrectDataException<AttackWithAbilityCommand>(nameof(request.DefensiveSkill));

			return AttackData.CreateData(
				attacker: attacker,
				target: target,
				aimedPart: aimedPart,
				attackFormula: ability,
				defensiveSkill: defensiveSkill,
				specialToHit: request.SpecialToHit,
				specialToDamage: request.SpecialToDamage);
		}
	}
}