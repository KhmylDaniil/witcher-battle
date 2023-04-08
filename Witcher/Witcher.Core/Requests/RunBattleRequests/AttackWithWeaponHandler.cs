using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;
using Witcher.Core.Exceptions;
using Witcher.Core.Logic;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.ExtensionMethods;

namespace Witcher.Core.Requests.RunBattleRequests
{
	public class AttackWithWeaponHandler : BaseHandler<AttackWithWeaponCommand, Unit>
	{
		/// <summary>
		/// Бросок параметра
		/// </summary>
		private readonly IRollService _rollService;

		public AttackWithWeaponHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IRollService rollService)
			: base(appDbContext, authorizationService)
		{
			_rollService = rollService;
		}

		public override async Task<Unit> Handle(AttackWithWeaponCommand request, CancellationToken cancellationToken)
		{
			var game = await _authorizationService.CharacterOwnerFilter(_appDbContext.Games, request.Id)
				.GetCreaturesAndCharactersFormBattle(request.BattleId)
				.FirstOrDefaultAsync(cancellationToken)
					?? throw new NoAccessToEntityException<Game>();

			var battle = game.Battles.FirstOrDefault()
				?? throw new EntityNotFoundException<Battle>(request.BattleId);

			var attackData = CheckAndFormData();

			var attack = new AttackProcess(_rollService);

			var attackLog = attack.RunAttack(attackData, request.DamageValue, request.AttackValue, request.DefenseValue);

			battle.BattleLog += attackLog;

			AttackProcess.RemoveDeadBodies(battle);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;

			AttackData CheckAndFormData()
			{
				var attacker = game.Characters.FirstOrDefault(x => x.Id == request.Id)
					?? throw new EntityNotFoundException<Character>(request.Id);

				var target = battle.Creatures.FirstOrDefault(x => x.Id == request.TargetId)
					?? throw new EntityNotFoundException<Creature>(request.TargetId);

				var aimedPart = request.CreaturePartId == null
					? null
					: target.CreatureParts.FirstOrDefault(x => x.Id == request.CreaturePartId)
						?? throw new EntityNotFoundException<BodyTemplatePart>(request.CreaturePartId.Value);

				if (!attacker.EquippedWeapons.Any())
					throw new RequestValidationException($"У существа {attacker.Name} нет экипированного оружия, атака невозможна.");

				var weapon = attacker.EquippedWeapons.FirstOrDefault(x => x.Id == request.WeaponId)
						?? throw new EntityNotFoundException<Weapon>(request.WeaponId);

				var defensiveSkill = request.DefensiveSkill == null
					? null
					: target.CreatureSkills.FirstOrDefault(x => x.Skill == request.DefensiveSkill)
						?? throw new RequestFieldIncorrectDataException<AttackWithWeaponCommand>(nameof(request.DefensiveSkill));

				return AttackData.CreateData(
					attacker: attacker,
					target: target,
					aimedPart: aimedPart,
					weapon: weapon,
					isStrongAttack: request.IsStrongAttack,
					defensiveSkill: defensiveSkill,
					specialToHit: request.SpecialToHit,
					specialToDamage: request.SpecialToDamage);
			}
		}


		
	}
}
