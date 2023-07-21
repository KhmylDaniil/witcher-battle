using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Witcher.Core.Abstractions;
using Witcher.Core.Contracts.RunBattleRequests;
using Witcher.Core.Entities;
using Witcher.Core.Exceptions.EntityExceptions;
using Witcher.Core.Exceptions.RequestExceptions;
using Witcher.Core.Logic;
using Microsoft.EntityFrameworkCore;
using Witcher.Core.ExtensionMethods;

namespace Witcher.Core.Requests.RunBattleRequests
{
	public class AttackHandler : BaseHandler<AttackCommand, Unit>
	{
		private readonly IRollService _rollService;

		public AttackHandler(IAppDbContext appDbContext, IAuthorizationService authorizationService, IRollService rollService)
			: base(appDbContext, authorizationService)
		{
			_rollService = rollService;
		}

		public override async Task<Unit> Handle(AttackCommand request, CancellationToken cancellationToken)
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

			RunBattle.RemoveDeadBodies(battle);

			TurnStateProcessing.EndOfAttackProcessing(attackData);

			await _appDbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;

			AttackData CheckAndFormData()
			{
				var attacker = battle.Creatures.FirstOrDefault(x => x.Id == request.Id)
					?? throw new EntityNotFoundException<Character>(request.Id);

				var target = battle.Creatures.FirstOrDefault(x => x.Id == request.TargetId)
					?? throw new EntityNotFoundException<Creature>(request.TargetId);

				var aimedPart = request.CreaturePartId == null
					? null
					: target.CreatureParts.FirstOrDefault(x => x.Id == request.CreaturePartId)
						?? throw new EntityNotFoundException<CreaturePart>(request.CreaturePartId.Value);

				var defensiveSkill = request.DefensiveSkill == null
					? null
					: target.CreatureSkills.FirstOrDefault(x => x.Skill == request.DefensiveSkill)
						?? throw new RequestFieldIncorrectDataException<AttackCommand>(nameof(request.DefensiveSkill));

				return AttackData.CreateData(
					attacker: attacker,
					target: target,
					aimedPart: aimedPart,
					attackFormula: GetAttackFormula(request, attacker),
					isStrongAttack: request.IsStrongAttack,
					defensiveSkill: defensiveSkill,
					specialToHit: request.SpecialToHit,
					specialToDamage: request.SpecialToDamage);
			}
		}

		private IAttackFormula GetAttackFormula(AttackCommand request, Creature attacker)
		{
			if (request.AttackType == BaseData.Enums.AttackType.Weapon && attacker is Character character)
			{
				var weapon = character.Items
					.FirstOrDefault(i => i.ItemType == BaseData.Enums.ItemType.Weapon && i.IsEquipped.Value && i.ItemTemplateId == request.AttackFormulaId)
						?? throw new EntityNotFoundException<Weapon>(request.AttackFormulaId);

				return weapon.ItemTemplate as WeaponTemplate
					?? throw new EntityNotIncludedException<Weapon>(nameof(WeaponTemplate));
			}
			else if (request.AttackType == BaseData.Enums.AttackType.Ability)
			{
				return attacker.Abilities.FirstOrDefault(x => x.Id == request.AttackFormulaId)
					?? throw new EntityNotFoundException<Ability>(request.AttackFormulaId);
			}
			else
				throw new NotImplementedException();
		}
	}
}
